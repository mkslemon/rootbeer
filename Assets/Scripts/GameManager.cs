using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ggj.rootbeer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _GAMEMANAGER;

        [Header("Game Tuning")]
        public int numberTries;
        [HideInInspector] public int triesLeft;
        public float requiredScoreDistanceForWinEnding = 0.2f;
        public float requiredScoreDistanceForOkayEnding = 0.4f;

        [Header("Game Object References")]
        [HideInInspector] public Patron[] activePatrons;
        public Patron[] Patrons;
        public Drink drink;
        public Transform[] patronOrigins;
        private FlavorProfile targetFlavorProfile;
        public float PatronStartingDistanceToTarget;
        public Transform[] patronEntryAnimOrigin;
        public Transform charactersGrouping;

        int currentLevel=0;

        private void Awake()
        {
            _GAMEMANAGER = this;
        }


        public void tryToServe()
        {

            float[] scores = activePatrons.Select(sel => GetDistanceToTarget(sel, targetFlavorProfile) / PatronStartingDistanceToTarget).ToArray();
            //for each patron, compare the served drinks then see how they react
            for(int p= 0; p<activePatrons.Length; p++)
            {
                //compare to preferences to generate a score (0 to 1.0)
                scores[p] = activePatrons[p].Score(drink);


                //move the patron closer to the drink if liked or closer to their origin point if disliked
                //pass a float 0 to 1.0 along with 
                //Patron.Scooch();
                //
            }

            //check if scores are close enough
            if (scores[0]-scores[1]< requiredScoreDistanceForWinEnding)
            {
                //characters are close enough to win


            }
            //for other endings we also need to be out of tries
            else if (triesLeft <= 0)
            {
                if (scores[0] - scores[1] < requiredScoreDistanceForOkayEnding)
                {
                    //out of tries and we made it to okay ending territory
                }
                else
                {
                    //out of tries and didn't even make it to the okay ending!
                }
            }
            
            

        }

        public void UpdateTargetFlavorProfile()
        {
            targetFlavorProfile = activePatrons[0].FlavorProfile.GetAverage(activePatrons[1].FlavorProfile);
        }

        public void GetStartingDistance(FlavorProfile targetFlavorProfile)
        {
            // call this any time new patrons are loaded to update the value
            PatronStartingDistanceToTarget = activePatrons[0].FlavorProfile.GetDistance(targetFlavorProfile); // this should be the same for both since the target is an average, update this function if this is no longer true
        }
        public float GetDistanceToTarget(Patron p, FlavorProfile targetFlavorProfile)
        {
            return p.FlavorProfile.GetDistance(targetFlavorProfile) / PatronStartingDistanceToTarget;
        }


        // Start is called before the first frame update
        void Start()
        {
            ShowNextCharacters();
        }

        // Update is called once per frame
        void Update()
        {

        }


        //take a dependency tht there are 2+ charas and an even number
        void ShowNextCharacters()
        {
            for(int i=0; i<2; i++)
            {
                activePatrons[i] = Instantiate(Patrons[currentLevel+i], patronEntryAnimOrigin[i].position, Quaternion.identity);
                activePatrons[i].EnterSeat(patronOrigins[i].transform.position, i*.3f);
                activePatrons[i].transform.SetParent(charactersGrouping);
            }

            

        }
    }
}