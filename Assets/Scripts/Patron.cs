using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace ggj.rootbeer
{
    public class Patron : MonoBehaviour
    {
        public FlavorProfile FlavorProfile = new FlavorProfile(0.5f, 0.5f, 0.5f, 0.5f);
        public List<ToppingName> PreferredToppings;
        public List<ToppingName> HatedToppings;
        
        // Start is called before the first frame update
        void Start()
        {
            if (FlavorProfile == null)
            {
                FlavorProfile = new FlavorProfile();
            }
            if (PreferredToppings == null)
            {
                PreferredToppings = new List<ToppingName>();
            }
            if (HatedToppings == null)
            {
                HatedToppings = new List<ToppingName>();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GeneratePreferences()
        {
            // use this if the Patron's preferences are not statically set.
            FlavorProfile.GenerateRandomProfile();

            // TODO: get the toppings from the game manager and grab random values to fill Preferred and Hated Toppings
            // consider filling this in a way that manual filling is less likely to result in errors? Maybe use enum? ///// Done
        }

        public float Score(Drink d)
        {

            return 0;
        }

        public void Scooch(Vector3 target)
        {
            Sequence sequence = DOTween.Sequence();
        }
    }
}
