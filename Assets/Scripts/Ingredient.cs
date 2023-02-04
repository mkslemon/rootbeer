using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour
{
    private FlavorTooltip _flavorTooltip;
    private bool _mouseInside;

    #region Unity
    private void Awake() {
        _flavorTooltip = GameObject.Find("IngredientFlavorTooltip").GetComponent<FlavorTooltip>();
        _flavorTooltip.gameObject.SetActive(false);
    }

    private void Update() {

        if (_mouseInside) {
            if (Input.GetMouseButton(0)) {
                Debug.Log("Mouse down");

            }

            if (Input.GetMouseButtonUp(0)) {
                Debug.Log("Mouse up");
            }
        }
    }
    #endregion

    #region Mouse controls
    private void OnMouseEnter() {
        _mouseInside = true;

        // set the _flavorTooltip position to the screen position
        Vector3 screenPosition = Input.mousePosition;

        screenPosition.z = _flavorTooltip.gameObject.transform.position.z;
        Debug.Log(screenPosition);

        _flavorTooltip.gameObject.transform.position = screenPosition;
        _flavorTooltip.gameObject.SetActive(true);
        _flavorTooltip.GetComponent<CanvasGroup>().alpha = 0f;
        _flavorTooltip.GetComponent<CanvasGroup>().DOFade(1, 1);

    }

    private void OnMouseExit() {
        _mouseInside = false;

        _flavorTooltip.GetComponent<CanvasGroup>().alpha = 1f;
        _flavorTooltip.GetComponent<CanvasGroup>().DOFade(0, 1).OnComplete(() => {
            _flavorTooltip.gameObject.SetActive(false);
        });
        
    }
    #endregion
}
