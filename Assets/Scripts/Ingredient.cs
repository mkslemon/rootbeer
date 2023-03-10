using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace ggj.rootbeer {
    public class Ingredient : MonoBehaviour {
        public static Ingredient ActiveInstance;

        [SerializeField] private IngredientGroup _ingredientGroup;

        public Juice _juice;
        public Syrup _syrup;
        public Topping _topping;

        private string _name;

        private FlavorTooltip _flavorTooltip;
        private bool _mouseInside;
        private bool _mouseInsideCup;
        private bool _dragging;

        public bool Highlighted = false;
        private SpriteRenderer _spriteRenderer;

        private Vector3 _originalPosition;
        private Vector3 _positionOffset;
        private Vector3 _mousePositionWorld;

        private readonly Vector2 TOOLTIP_OFFSET = new Vector2(150f, 150f);

        private FlavorProfile _flavorProfile;

        #region Unity
        private void Awake() {
            _flavorTooltip = GameObject.Find("IngredientFlavorTooltip").GetComponent<FlavorTooltip>();
            _originalPosition = transform.position;

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            _flavorTooltip.gameObject.SetActive(false);

            if (_juice != null) {
                _flavorProfile = _juice.FlavorProfile;
                _name = _juice.Name;
            }
            else if (_syrup != null) {
                _flavorProfile = _syrup.FlavorProfile;
                _name = _syrup.Name;
            }
            else if (_topping != null) {
                _flavorProfile = _topping.FlavorProfile;
                _name = _topping.Name;
            }
        }

        private void Update() {
            if (Highlighted)
                _spriteRenderer.color = Color.white;
            else
                _spriteRenderer.color = Color.white;

            if (_mouseInside) {

                // Get the mouse position in world coordinates
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = -Camera.main.transform.position.z - 1;
                _mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePos);

                // Handle mouse clicks and dragging/releasing
                if (Input.GetMouseButtonDown(0)) {
                    transform.DOKill();
                    SoundManager.instance.pickupIngredient.Play();
                    _positionOffset = _mousePositionWorld - transform.position;
                    _flavorTooltip.gameObject.SetActive(false);
                    _dragging = true;
                }

                if (Input.GetMouseButton(0)) {
                    Dragging();
                }

                if (Input.GetMouseButtonUp(0)) {
                    _dragging = false;
                    SoundManager.instance.dropIngredient.Play();
                    Released();
                }
            }
        }
        #endregion

        #region Public

        public void Select() {
            _ingredientGroup.ClearSelection();
            Highlighted = true;
        }

        #endregion

        #region Private helpers

        private void Dragging() {
            // When we drag, we snap to the position in the world 
            transform.position = _mousePositionWorld - _positionOffset;

            // Check if we're inside of the cup
            if (transform.position.x > -1f && transform.position.x < 1f && transform.position.y > -1.5f && transform.position.y < 0.75f)
            {
                if (_mouseInsideCup == false) {
                    // we're within the cup boundary
                    _mouseInsideCup = true;

                    if (_juice != null) {
                        SoundManager.instance.addBase.Play();
                        Bar.Instance.SetJuice(_juice, this);
                    }
                    else if (_syrup != null) {
                        SoundManager.instance.addMixin.Play();
                        Bar.Instance.SetSyrup(_syrup, this);
                    }
                    else if (_topping != null) {
                        SoundManager.instance.addGarnish.Play();
                        Bar.Instance.SetTopping(_topping, this);
                    }

                    Select();
                }
            }
            else
            {
                if (_mouseInsideCup) {
                    _mouseInsideCup = false;

                    // If we *were* in the cup boundary but we dragged out
                    // then the user wants us not to use this ingredient, so revert
                    // back to the previous ingredients
                    Bar.Instance.CancelNewMix();
                }
            }

        }

        private void Released() {
            // When released, if we were in the cup jump back instantly
            if (_mouseInsideCup)
                transform.position = _originalPosition;
            // otherwise, wander back slowly
            else
                transform.DOMove(_originalPosition, 1f);
            _mouseInsideCup = false;
        }

        #endregion

        #region Mouse controls
        private void OnMouseEnter() {
            if (!_dragging && ActiveInstance == null) {
                ActiveInstance = this;

                //if (!_mouseInside) {
                    _mouseInside = true;

                    _flavorTooltip.SetFlavorProfile(_flavorProfile, _name);

                    // set the _flavorTooltip position to the screen position
                    Vector3 ingredientScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
                    if (ingredientScreenPosition.x > 1000)
                        ingredientScreenPosition += new Vector3(-TOOLTIP_OFFSET.x, TOOLTIP_OFFSET.y, _flavorTooltip.gameObject.transform.position.z);
                    else
                        ingredientScreenPosition += new Vector3(TOOLTIP_OFFSET.x, TOOLTIP_OFFSET.y, _flavorTooltip.gameObject.transform.position.z);
                _flavorTooltip.gameObject.transform.position = ingredientScreenPosition;

                    // Fade in the object
                    _flavorTooltip.gameObject.SetActive(true);
                    _flavorTooltip.GetComponent<CanvasGroup>().DOKill();
                    _flavorTooltip.GetComponent<CanvasGroup>().alpha = 0f;
                    _flavorTooltip.GetComponent<CanvasGroup>().DOFade(1, 1);
                //}
            }
        }

        private void OnMouseExit() {
            if (!_dragging && ActiveInstance == this) {
                ActiveInstance = null;
                _mouseInside = false;
                Released();

                _flavorTooltip.GetComponent<CanvasGroup>().alpha = 1f;
                _flavorTooltip.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => {
                    if (ActiveInstance == null)
                        _flavorTooltip.gameObject.SetActive(false);
                });
            }
        }
        #endregion

    }
}
