using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ggj.rootbeer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _GAMEMANAGER;

        [Header("Game Tuning")]
        public int numberTries;
        [HideInInspector] public int triesLeft;
        public float requiredScoreDistanceForWinEnding;
        public float requiredScoreDistanceForOkayEnding;

        [Header("Game Object References")]
        public Patron[] activePatrons;
        public Drink drink;
        public Transform[] patronOrigins;

    private void Awake()
        {
            _GAMEMANAGER = this;
        }


        public void tryToServe()
        {
            float[] scores = new float[2];
            //for each patron, compare the served drinks then see how they react
            for(int p= 0; p<activePatrons.Length; p++)
            {
                //compare to preferences to generate a score (0 to 1.0)
                //scores[p] = activePatrons[p]Score(drink);


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




        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}