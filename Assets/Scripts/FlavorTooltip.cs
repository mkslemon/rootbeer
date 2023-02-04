using System.Collections;
using System.Collections.Generic;
using ggj.rootbeer;
using UnityEngine;

public class FlavorTooltip : MonoBehaviour
{
    [SerializeField] private FlavorProfile _flavorProfile;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
