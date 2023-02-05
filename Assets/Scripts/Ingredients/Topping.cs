using UnityEngine;

namespace ggj.rootbeer {
    public enum ToppingName { Orange_Peel, Coconut, Candy_Cane }
    public class Topping {
        public string Name { get; set; }
        public string Text { get; set; }
        public FlavorProfile FlavorProfile { get; }
        public Material ToppingMaterial { get; }

        public Topping(string name, Material material, string text = "", FlavorProfile flavorProfile = null) {
            Name = name;
            ToppingMaterial = material;
            Text = text;
            FlavorProfile = flavorProfile ?? new FlavorProfile();
        }
    }
}
