using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ggj.rootbeer {
    public class Juice {
        public string Name { get; set; }
        public Color Color { get; set; }
        public string Text { get; set; }
        public FlavorProfile FlavorProfile { get; }

        public Juice(string name, Color? color = null, string text = "", FlavorProfile flavorProfile = null) {
            Name = name;
            Color = color ?? Color.black; // doing this weird thing due to the value for this parameter needing to be a compile time constant, might not be necessary to make it an optional parameter, but this works
            Text = text;
            FlavorProfile = flavorProfile ?? new FlavorProfile();
        }
    }
}
