using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private FlavorProfile _drinkFlavorProfile;

        // Drink parts
        private Juice _juice;
        private Syrup _syrup;
        private Topping _topping;

        private List<FlavorProfile> _currentFlavorProfiles;

        #region Unity
        private void Awake() {
            if (Instance == null)
                Instance = this;
            else
                throw new System.Exception("Only one bar can be in the scene");

            _drinkFlavorTooltip.gameObject.SetActive(false);
        }
        #endregion

        #region     Public
        public void SetJuice(Juice juice) {
            _juice = juice;
            Mix();
        }

        public void ClearJuice() {
            _juice = null;
            Mix();
        }

        public void SetSyrup(Syrup syrup) {
            _syrup = syrup;
            Mix();
        }

        public void ClearSyrup() {
            _syrup = null;
            Mix();
        }

        public void SetTopping(Topping topping) {
            _topping = topping;
            Mix();
        }

        public void ClearTopping() {
            _topping = null;
            Mix();
        }

        // 
        public void Mix() {
            GetFlavorProfiles();

            if (_currentFlavorProfiles.Count == 1)
                _drinkFlavorProfile = _currentFlavorProfiles[0];
            else {
                _drinkFlavorProfile = FlavorProfile.GetAverages(_currentFlavorProfiles);
            }

            RenderDrink();
            _drinkFlavorTooltip.SetFlavorProfile(_drinkFlavorProfile);
        }

        public void ClearDrink() {
            _juice = null;
            _syrup = null;
            _topping = null;
            _drinkFlavorProfile = null;
        }

        public void RenderDrink() {
            // TODO
            Debug.Log("Not implemented");
        }

        public void ServerDrink() {

            _gameManager.NewDrink();
        }

        #endregion

        #region Private helpers
        private void GetFlavorProfiles() {
            _currentFlavorProfiles = new List<FlavorProfile>();
            if (_juice != null)
                _currentFlavorProfiles.Add(_juice.FlavorProfile);
            if (_syrup != null)
                _currentFlavorProfiles.Add(_syrup.FlavorProfile);
            if (_topping != null)
                _currentFlavorProfiles.Add(_topping.FlavorProfile);
        }

        #endregion
    }
}
