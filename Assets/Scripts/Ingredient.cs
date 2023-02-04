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
        private bool _dragging;

        private Vector3 _originalPosition;
        private Vector3 _positionOffset;
        private Vector3 _mousePositionWorld;

        #region Unity
        private void Awake() {
            _flavorTooltip = GameObject.Find("IngredientFlavorTooltip").GetComponent<FlavorTooltip>();
            _originalPosition = transform.position;
        }

        private void Start() {
            _flavorTooltip.gameObject.SetActive(false);
        }

        private void Update() {
            if (_mouseInside) {

                // Create the plane to project onto
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = -Camera.main.transform.position.z - 1;
                _mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePos);

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
            transform.position = _mousePositionWorld - _positionOffset;

            if (transform.position.x > -1f && transform.position.x < 1f && transform.position.y > -1.25f && transform.position.y < 1f) {
                // we're within the cup boundary
                if (_juice != null)
                    Bar.Instance.SetJuice(_juice);
                else if (_syrup != null)
                    Bar.Instance.SetSyrup(_syrup);
                else if (_topping != null)
                    Bar.Instance.SetTopping(_topping);
            }

        }

        private void Released() {
            transform.DOMove(_originalPosition, 1f);
        }

        #endregion

        #region Mouse controls
        private void OnMouseEnter() {
            ActiveInstance = this;
            _mouseInside = true;

            // set the _flavorTooltip position to the screen position
            Vector3 screenPosition = Input.mousePosition;
            screenPosition.z = _flavorTooltip.gameObject.transform.position.z;
            screenPosition.x += 75f;
            screenPosition.y += 75f;
            _flavorTooltip.gameObject.transform.position = screenPosition;

            // Fade in the object
            _flavorTooltip.gameObject.SetActive(true);
            _flavorTooltip.GetComponent<CanvasGroup>().alpha = 0f;
            _flavorTooltip.GetComponent<CanvasGroup>().DOFade(1, 1);
        }

        private void OnMouseExit() {
            if (!_dragging) {
                _mouseInside = false;
                Released();

                _flavorTooltip.GetComponent<CanvasGroup>().alpha = 1f;
                _flavorTooltip.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => {
                    if (ActiveInstance == this)
                        _flavorTooltip.gameObject.SetActive(false);
                });
            }
        }
        #endregion

    }
}
