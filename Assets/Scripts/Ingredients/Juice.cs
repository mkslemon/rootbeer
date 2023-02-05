using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ggj.rootbeer {
    public class Juice:  Syrup
    {

        public Juice(string name, Color? color = null, string text = "", FlavorProfile flavorProfile = null) : base(name, color, text, flavorProfile) 
        {
            // currently no new functionality for Juice
        }
    }
}
