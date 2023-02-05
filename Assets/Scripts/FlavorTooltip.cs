using System.Collections;
using System.Collections.Generic;
using ggj.rootbeer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlavorTooltip : MonoBehaviour
{
    private FlavorProfile _flavorProfile;
    [SerializeField] private List<Image> _flavorSliders;
    [SerializeField] private TMP_Text _text;

    private void Awake() {
        ClearProfile();
    }

    public void SetFlavorProfile(FlavorProfile flavorProfile, string name = null) {
        if (name != null && _text != null)
            _text.text = name;

        _flavorProfile = flavorProfile;

        _flavorSliders[0].fillAmount = _flavorProfile.Citrus;
        _flavorSliders[1].fillAmount = _flavorProfile.Floral;
        _flavorSliders[2].fillAmount = _flavorProfile.Sweet;
        _flavorSliders[3].fillAmount = _flavorProfile.Exotic;
    }

    public void ClearProfile() {
        _flavorProfile = null;

        _flavorSliders[0].fillAmount = 0f;
        _flavorSliders[1].fillAmount = 0f;
        _flavorSliders[2].fillAmount = 0f;
        _flavorSliders[3].fillAmount = 0f;
    }
}
