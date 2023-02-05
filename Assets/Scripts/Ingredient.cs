using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ggj.rootbeer {
    public class Ingredient : MonoBehaviour {
        public static Ingredient ActiveInstance;

        public Juice _juice;
        public Syrup _syrup;
        public Topping _topping;

        private FlavorTooltip _flavorTooltip;
        private bool _mouseInside;
        private bool _mouseInsideCup;
        private bool _dragging;

        private Vector3 _originalPosition;
        private Vector3 _positionOffset;
        private Vector3 _mousePositionWorld;

        private readonly Vector2 TOOLTIP_OFFSET = new Vector2(150f, 150f);

        private FlavorProfile _flavorProfile;

        #region Unity
        private void Awake() {
            _flavorTooltip = GameObject.Find("IngredientFlavorTooltip").GetComponent<FlavorTooltip>();
            _originalPosition = transform.position;
        }

        private void Start() {
            _flavorTooltip.gameObject.SetActive(false);

            if (_juice != null)
                _flavorProfile = _juice.FlavorProfile;
            else if (_syrup != null)
                _flavorProfile = _syrup.FlavorProfile;
            else if (_topping != null)
                _flavorProfile = _topping.FlavorProfile;
        }

        private void Update() {
            if (_mouseInside) {

                // Get the mouse position in world coordinates
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = -Camera.main.transform.position.z - 1;
                _mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePos);

                // Handle mouse clicks and dragging/releasing
                if (Input.GetMouseButtonDown(0)) {
                    _positionOffset = _mousePositionWorld - transform.position;
                    _flavorTooltip.gameObject.SetActive(false);
                    _dragging = true;
                }

                if (Input.GetMouseButton(0)) {
                    Dragging();
                }

                if (Input.GetMouseButtonUp(0)) {
                    _dragging = false;
                    Released();
                }
            }
        }
        #endregion

        #region Private helpers

        private void Dragging() {
            // When we drag, we snap to the position in the world 
            transform.position = _mousePositionWorld - _positionOffset;

            // Check if we're inside of the cup
            if (transform.position.x > -1f && transform.position.x < 1f && transform.position.y > -1.25f && transform.position.y < 1f)
            {

                // we're within the cup boundary
                _mouseInsideCup = true;

                if (_juice != null)
                    Bar.Instance.SetJuice(_juice);
                else if (_syrup != null)
                    Bar.Instance.SetSyrup(_syrup);
                else if (_topping != null)
                    Bar.Instance.SetTopping(_topping);
            }
            else
            {
                _mouseInsideCup = false;
                // If we *were* in the cup boundary but we dragged out
                // then the user wants us not to use this ingredient, so revert
                // back to the previous ingredients
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

                if (!_mouseInside) {
                    _mouseInside = true;

                    _flavorTooltip.SetFlavorProfile(_flavorProfile);

                    // set the _flavorTooltip position to the screen position
                    Vector3 ingredientScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
                    ingredientScreenPosition += new Vector3(TOOLTIP_OFFSET.x, TOOLTIP_OFFSET.y, _flavorTooltip.gameObject.transform.position.z);
                    _flavorTooltip.gameObject.transform.position = ingredientScreenPosition;

                    // Fade in the object
                    _flavorTooltip.gameObject.SetActive(true);
                    _flavorTooltip.GetComponent<CanvasGroup>().alpha = 0f;
                    _flavorTooltip.GetComponent<CanvasGroup>().DOFade(1, 1);
                }
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
