using System.Collections;
using System.Collections.Generic;
using ggj.rootbeer;
using UnityEngine;
using UnityEngine.UI;

public class FlavorTooltip : MonoBehaviour
{
    [SerializeField] private FlavorProfile _flavorProfile;
    [SerializeField] private List<Image> _flavorSliders;

    public void SetFlavorProfile(FlavorProfile flavorProfile) {
        _flavorProfile = flavorProfile;

        _flavorSliders[0].fillAmount = _flavorProfile.Sweet;
        _flavorSliders[1].fillAmount = _flavorProfile.Sour;
        _flavorSliders[2].fillAmount = _flavorProfile.Salty;
        _flavorSliders[3].fillAmount = _flavorProfile.Bitter;
    }
}
