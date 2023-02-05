using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

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

        private (Juice, Syrup, Topping) prevIngredients;

        [SerializeField] GameObject _strawGO;
        [SerializeField] private Material _drinkMaterial;
        [SerializeField] private ParticleSystem _toppingSystem;
        [SerializeField] private ParticleSystemRenderer _toppingSystemRenderer;
        [SerializeField] private Transform _planeTransform;
        [SerializeField] private Transform _icePlaneTransform;

        [SerializeField] private ParticleSystem _iceSystem;

        private float[] _fillPercs = { -15, -5, 0 };

        #region Unity

        private void Awake() {
            Instance = this;
            _drinkMaterial.SetFloat("_FillPerc", _fillPercs[0]);
            _toppingSystem.Clear();
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
            _drinkMaterial.DOKill();

            // Set the height of the drink
            if (Juice != null && Syrup != null) {
                _drinkMaterial.DOFloat(_fillPercs[2], "_FillPerc", 1);
                _planeTransform.localPosition = Vector3.up* 0.9f;
                //_icePlaneTransform.localPosition = Vector3.up;
            }
            else if (Juice != null || Syrup != null) {
                _drinkMaterial.DOFloat(_fillPercs[1], "_FillPerc", 1);
                _planeTransform.localPosition = Vector3.up * 0.45f;
                //_icePlaneTransform.localPosition = Vector3.up * 0.55f;
            }
            else {
                _drinkMaterial.SetFloat("_FillPerc", _fillPercs[0]);
                _planeTransform.localPosition = Vector3.zero;
                //_icePlaneTransform.localPosition = Vector3.zero;
            }

            // Set the color by mixing the ingredients
            Color liquidColor = GetColor();
            _drinkMaterial.SetColor("_BaseColor", liquidColor);


            // Add ice
            //if (Juice != null || Syrup != null) {
            //    _iceSystem.Clear();
            //    //_iceSystem.Emit(3);
            //}

            // Set the topping material and emit a topping particle
            if (Topping != null) {
                _toppingSystem.Clear();
                _toppingSystemRenderer.material = Topping.ToppingMaterial;
                _toppingSystem.Emit(1);
            }

        }

        public void SaveIngredients() {
            prevIngredients = (Juice, Syrup, Topping);
        }

        public void RevertIngredients() {
            Juice = prevIngredients.Item1;
            Syrup = prevIngredients.Item2;
            Topping = prevIngredients.Item3;
        }

        public void EmptyRender() {
            _drinkMaterial.DOKill();
            _drinkMaterial.DOFloat(_fillPercs[0], "_FillPerc", 5);

            _toppingSystem.Clear();
            //_iceSystem.Clear();
            prevIngredients = (null, null, null);
        }

        public Color GetColor()
        {
            // try different blend methods?
            // some examples altered from here: https://stackoverflow.com/questions/1351442/is-there-an-algorithm-for-color-mixing-that-works-like-mixing-real-colors
            Color returnColor = Color.black;
            int blendMode = 0;

            List<Color> colors = new List<Color>();

            if (Juice != null)
                colors.Add(Juice.Color);
            if (Syrup != null)
                colors.Add(Syrup.Color);

            if (colors.Count > 0) {

                switch (blendMode) {
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
            }

            return returnColor;
        }

        public void SetStrawVisibility(bool state) {
            _strawGO.SetActive(state);
        }
    }
}
