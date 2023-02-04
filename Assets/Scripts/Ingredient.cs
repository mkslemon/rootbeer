using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    private FlavorTooltip _flavorTooltip;

    private void Awake() {
        _flavorTooltip = GameObject.Find("FlavorTooltip").GetComponent<FlavorTooltip>();
    }

    private void OnMouseEnter() {
        _flavorTooltip.gameObject.SetActive(true);
    }

    private void OnMouseExit() {
        _flavorTooltip.gameObject.SetActive(false);
    }
}
