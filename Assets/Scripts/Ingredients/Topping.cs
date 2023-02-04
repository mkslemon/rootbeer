using UnityEngine;

namespace ggj.rootbeer {
    public enum ToppingName { Orange_Peel, Coconut, Candy_Cane }
    public class Topping {
        public string Name { get; set; }
        public string Text { get; set; }
        public FlavorProfile FlavorProfile { get; }

        public Topping(string name, string text = "", FlavorProfile flavorProfile = null) {
            Name = name;
            //Color = color ?? Color.black; // doing this weird thing due to the value for this parameter needing to be a compile time constant, might not be necessary to make it an optional parameter, but this works
            Text = text;
            FlavorProfile = flavorProfile ?? new FlavorProfile();
        }
    }
}
