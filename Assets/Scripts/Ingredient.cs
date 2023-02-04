using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    private FlavorTooltip _flavorTooltip;
    private bool _mouseInside;

    private Vector3 _originalPosition;

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
            if (Input.GetMouseButton(0)) {
                Dragging();
            }

            if (Input.GetMouseButtonUp(0)) {
                Released();
            }
        }
    }
    #endregion

    #region Private helpers

    private void Dragging() {
        Debug.Log("Dragging");
    }

    private void Released() {
        Debug.Log("Released");

        transform.DOMove(_originalPosition, 1f);
    }

    #endregion

    #region Mouse controls
    private void OnMouseEnter() {
        _mouseInside = true;

        // set the _flavorTooltip position to the screen position
        Vector3 screenPosition = Input.mousePosition;
        screenPosition.z = _flavorTooltip.gameObject.transform.position.z;
        screenPosition.x += 0.5f;
        screenPosition.y += 0.5f;
        _flavorTooltip.gameObject.transform.position = screenPosition;

        // Fade in the object
        _flavorTooltip.gameObject.SetActive(true);
        _flavorTooltip.GetComponent<CanvasGroup>().alpha = 0f;
        _flavorTooltip.GetComponent<CanvasGroup>().DOFade(1, 1);
    }

    private void OnMouseExit() {
        Debug.Log("here");
        _mouseInside = false;

        _flavorTooltip.GetComponent<CanvasGroup>().alpha = 1f;
        _flavorTooltip.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => {
            _flavorTooltip.gameObject.SetActive(false);
        });
        
    }
    #endregion
}
