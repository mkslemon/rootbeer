using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ggj.rootbeer {
    public class IngredientGroup : MonoBehaviour {
        [SerializeField] private List<Ingredient> _ingredients;

        public void ClearSelection() {
            foreach (Ingredient ing in _ingredients)
                ing.Highlighted = false;
        }
    }
}
