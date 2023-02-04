using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private FlavorTooltip _flavorTooltip;

    private void Awake() {
        _flavorTooltip = GameObject.Find("IngredientFlavorTooltip").GetComponent<FlavorTooltip>();
        _flavorTooltip.gameObject.SetActive(false);
    }

    private void OnMouseEnter() {

        // set the _flavorTooltip position to the screen position
        Vector3 screenPosition = Input.mousePosition;

        screenPosition.z = _flavorTooltip.gameObject.transform.position.z;
        Debug.Log(screenPosition);

        _flavorTooltip.gameObject.transform.position = screenPosition;
        _flavorTooltip.gameObject.SetActive(true);


    }

    private void OnMouseExit() {
        _flavorTooltip.gameObject.SetActive(false);
    }
}
