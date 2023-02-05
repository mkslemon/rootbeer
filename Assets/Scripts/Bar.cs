using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ggj.rootbeer
{
    /// <summary>
    /// Handles drink mixing
    /// </summary>
    public class Bar : MonoBehaviour
    {
        public static Bar Instance;

        [SerializeField] private GameManager _gameManager;

        [SerializeField] FlavorTooltip _drinkFlavorTooltip;
        [SerializeField] GameObject _serverButtonGO;

        private List<FlavorProfile> _currentFlavorProfiles = new List<FlavorProfile>();
        private List<FlavorProfile> _prevFlavorProfiles = new List<FlavorProfile>();

        private Ingredient[] _ingredients = new Ingredient[3];
        private Ingredient[] _prevIngredients;

        #region Unity
        private void Awake() {
            if (Instance == null)
                Instance = this;
            else
                throw new System.Exception("Only one bar can be in the scene");
        }
        #endregion

        #region     Public
        public void SetJuice(Juice juice, Ingredient ingredient) {
            Drink.Instance.Juice = juice;
            _prevIngredients = _ingredients;
            _ingredients[0] = ingredient;
            Mix();
        }

        public void ClearJuice() {
            Drink.Instance.Juice = null;
            Mix();
        }

        public void SetSyrup(Syrup syrup, Ingredient ingredient) {
            Drink.Instance.Syrup = syrup;
            _prevIngredients = _ingredients;
            _ingredients[1] = ingredient;
            Mix();
        }

        public void ClearSyrup() {
            Drink.Instance.Syrup = null;
            Mix();
        }

        public void SetTopping(Topping topping, Ingredient ingredient) {
            Drink.Instance.Topping = topping;
            _prevIngredients = _ingredients;
            _ingredients[1] = ingredient;
            Mix();
        }

        public void ClearTopping() {
            Drink.Instance.Topping = null;
            Mix();
        }

        // 
        public void Mix(bool getProfiles = true) {
            if (getProfiles)
                GetFlavorProfiles();

            if (_currentFlavorProfiles.Count == 1)
                Drink.Instance.FlavorProfile = _currentFlavorProfiles[0];
            else {
                Drink.Instance.FlavorProfile = FlavorProfile.GetAverages(_currentFlavorProfiles);
            }

            if (_currentFlavorProfiles.Count >= 3)
                _serverButtonGO.SetActive(true);
            else
                _serverButtonGO.SetActive(false);

            Drink.Instance.UpdateRender();
            _drinkFlavorTooltip.SetFlavorProfile(Drink.Instance.FlavorProfile);
        }

        public void ClearDrink() {
            Drink.Instance.Juice = null;
            Drink.Instance.Syrup = null;
            Drink.Instance.Topping = null;
            Drink.Instance.FlavorProfile = null;
            Drink.Instance.EmptyRender();
            _drinkFlavorTooltip.ClearProfile();
            _serverButtonGO.SetActive(false);
        }

        public void ServeDrink() {
            _gameManager.NewDrink();
        }

        public void CancelNewMix() {
            _ingredients = _prevIngredients;
            _currentFlavorProfiles = _prevFlavorProfiles;

            foreach (Ingredient ingredient in _ingredients)
                if (ingredient != null)
                    ingredient.Select();

            Mix(false);
        }

        #endregion

        #region Private helpers
        private void GetFlavorProfiles() {
            _prevFlavorProfiles = _currentFlavorProfiles;
            _currentFlavorProfiles = new List<FlavorProfile>();
            if (Drink.Instance.Juice != null)
                _currentFlavorProfiles.Add(Drink.Instance.Juice.FlavorProfile);
            if (Drink.Instance.Syrup != null)
                _currentFlavorProfiles.Add(Drink.Instance.Syrup.FlavorProfile);
            if (Drink.Instance.Topping != null) {
                float[] profile = Drink.Instance.Topping.FlavorProfile.GetAsArray();
                if (_currentFlavorProfiles.Count > 0)
                {
                    var currentAverage = FlavorProfile.GetAverages(_currentFlavorProfiles);
                    var weightedProfile = currentAverage.GetAsArray().Zip(profile, (c, t) => t + (c - t)).ToArray();

                    _currentFlavorProfiles.Add(new FlavorProfile(weightedProfile));
                }
                else
                {
                    _currentFlavorProfiles.Add(new FlavorProfile(profile));
                }
                
            }
        }

        #endregion
    }
}
