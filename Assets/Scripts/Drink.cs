using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ggj.rootbeer
{
    public class Drink : MonoBehaviour
    {
        public static Drink Instance;

        public List<Sprite> _baseSprites;
        public List<Sprite> _drinkSprites;

        public FlavorProfile FlavorProfile;
        // Drink parts
        public Juice Juice;
        public Syrup Syrup;
        public Topping Topping;

        [SerializeField] GameObject _strawGO;

        #region Unity

        private void Awake() {
            Instance = this;
        }

        #endregion

        public Drink()
        {
        }

        //public FlavorProfile GetFlavor()
        //{
        //    // get average of all flavors present
        //    // can also add flavors to toppings, but ignoring for now until further clarification, also the toppings would have less influence so that would need to be accounted for
        //    List<float[]> flavorArrays = Syrups.Select(sel => sel.FlavorProfile.GetAsArray()).ToList();
        //    var AverageFlavor = Enumerable.Range(0, flavorArrays[0].Length).Select(i => flavorArrays.Select(flavor => flavor[i]).Sum() / flavorArrays.Count).ToArray();
        //    return new FlavorProfile(AverageFlavor);
        //}

        public void UpdateRender() {
            if (Juice != null && Topping != null) {
                int spriteIdx = 0;
                if (Juice.Name.Equals("Root Beer"))
                    spriteIdx += 0;
                else if (Juice.Name.Equals("Ginger Ale"))
                    spriteIdx += 3;
                else if (Juice.Name.Equals("Sparkling OJ"))
                    spriteIdx += 6;

                if (Topping.Name.Equals("Mint"))
                    spriteIdx += 0;
                else if (Topping.Name.Equals("Cherry"))
                    spriteIdx += 1;
                else if (Topping.Name.Equals("Lime"))
                    spriteIdx += 2;

                GetComponent<SpriteRenderer>().sprite = _drinkSprites[spriteIdx];
            }
            else if (Juice != null) {
                int spriteIdx = 0;
                if (Juice.Name.Equals("Root Beer"))
                    spriteIdx = 0;
                else if (Juice.Name.Equals("Ginger Ale"))
                    spriteIdx = 1;
                else if (Juice.Name.Equals("Sparkling OJ"))
                    spriteIdx = 2;


                GetComponent<SpriteRenderer>().sprite = _baseSprites[spriteIdx];
            }
        }

        public Color GetColor()
        {
            // try different blend methods?
            // some examples altered from here: https://stackoverflow.com/questions/1351442/is-there-an-algorithm-for-color-mixing-that-works-like-mixing-real-colors
            Color returnColor = Color.black;
            int blendMode = 0;
            Color[] colors = new Color[] { Juice.Color, Syrup.Color };

            switch(blendMode)
            {
                case 1:
                    // simple subtractive?
                    returnColor = colors.
                        Aggregate((c1, c2) => new Color(c1.r * c2.r, c1.g * c2.g, c1.b * c2.g));
                    break;
                case 2:
                    // dilute subtractive?
                    returnColor.r = 1 - Mathf.Sqrt(colors.Select(sel => Mathf.Pow(1 - sel.r, 2)).Average());
                    returnColor.g = 1 - Mathf.Sqrt(colors.Select(sel => Mathf.Pow(1 - sel.g, 2)).Average());
                    returnColor.b = 1 - Mathf.Sqrt(colors.Select(sel => Mathf.Pow(1 - sel.b, 2)).Average());
                    break;
                default:
                    // basic average
                    returnColor.r = colors.Select(sel => sel.r).Average();
                    returnColor.g = colors.Select(sel => sel.g).Average();
                    returnColor.b = colors.Select(sel => sel.b).Average();
                    break;
            }
            return returnColor;
        }

        public void SetStrawVisibility(bool state) {
            _strawGO.SetActive(state);
        }
    }
}
