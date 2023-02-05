using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class CreditsHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _PlayGO;

    public void OnPointerEnter(PointerEventData eventData) {
        _PlayGO.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _PlayGO.SetActive(true);
    }
}
