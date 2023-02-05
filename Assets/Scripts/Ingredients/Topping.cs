using UnityEngine;

namespace ggj.rootbeer {
    public enum ToppingName { Orange_Peel, Coconut, Candy_Cane }
    public class Topping {
        public string Name { get; set; }
        public string Text { get; set; }
        public FlavorProfile FlavorProfile { get; }
        public Sprite ToppingSprite { get; }

        public Topping(string name, Sprite sprite, string text = "", FlavorProfile flavorProfile = null) {
            Name = name;
            ToppingSprite = sprite;
            Text = text;
            FlavorProfile = flavorProfile ?? new FlavorProfile();
        }
    }
}
