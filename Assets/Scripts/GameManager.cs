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
        public float requiredScoreDistanceForWinEnding = 0.4f;
        public float requiredScoreDistanceForOkayEnding = 0.6f;
        public float happyZone = .7f;
        public float mediumZone = .4f;

        [Header("Game Object References")]
        [HideInInspector] public Patron[] activePatrons =  new Patron[2];
        public Patron[] Patrons;
        public Drink drink;
        public Transform[] patronOrigins;
        private FlavorProfile targetFlavorProfile;
        public float PatronStartingDistanceToTarget;
        public Transform[] patronEntryAnimOrigin;
        public Transform charactersGrouping;
        public EmojiBubble[] emojiBubbles;

        int currentLevel=0;

        [SerializeField] private List<Ingredient> _ingredients;


        private void Awake()
        {
            _GAMEMANAGER = this;
            activePatrons = new Patron[2];

            _ingredients[0]._juice = new Juice("Root Beer", Color.red, "", new FlavorProfile(0f, 0f, 1f, 1f));
            _ingredients[1]._juice = new Juice("Ginger Ale", Color.red, "", new FlavorProfile(0f, 1f, 0f, 1f));
            _ingredients[2]._juice = new Juice("Sparkling OJ", Color.red, "", new FlavorProfile(1f, 0f, 1f, 0f));

            _ingredients[3]._syrup = new Syrup("Vanilla", Color.red, "", new FlavorProfile(0f, 1f, 1f, 0f));
            _ingredients[4]._syrup = new Syrup("Grenadine", Color.red, "", new FlavorProfile(0f, 0f, 1f, 0f));
            _ingredients[5]._syrup = new Syrup("Coconut Creamer", Color.red, "", new FlavorProfile(0f, 1f, 0f, 1f));

            _ingredients[6]._topping = new Topping("Mint", "", new FlavorProfile(0f, 1f, 1f, 0f));
            _ingredients[7]._topping = new Topping("Cherry", "", new FlavorProfile(1f, 0f, 0f, 1f));
            _ingredients[8]._topping = new Topping("Lime", "", new FlavorProfile(1f, 1f, 0f, 0f));
        }


        public void tryToServe()
        {
            float[] scores = activePatrons.Select(sel => GetDistanceToTarget(sel, drink.GetFlavor()) / PatronStartingDistanceToTarget).ToArray();

            //do we need to reject a served drink because player forgot something in it?

            //process the submission
            triesLeft--;
            //for each patron, compare the served drinks then see how they react
            for(int p= 0; p<activePatrons.Length; p++)
            {
                //compare to preferences to generate a score (0 to 1.0)
                scores[p] = activePatrons[p].Score(drink);

                //move the patron closer to the drink if liked or closer to their origin point if disliked
                //pass a float 0 to 1.0 along with 
                activePatrons[p].Scooch(scores[p], patronOrigins[p].transform.position, drink.transform.position);
                //
                if (scores[p] >= happyZone)
                {
                    //characters are close enough to win
                    //show emotion emoji
                    activePatrons[p].popEmoji(activePatrons[p].closeEmoji);
                }
                else if (scores[p] >=  mediumZone)
                {
                    //we made it to okay territory
                    //show emotion emoji and show hint emoji
                    activePatrons[p].doublePopEmoji(activePatrons[p].mediumEmoji, activePatrons[p].hint);
                }
                else
                {
                    //we're far away
                    //show emotion emoji and show hint emoji
                    activePatrons[p].doublePopEmoji(activePatrons[p].mediumEmoji, activePatrons[p].hint);
                }




            }

            bool levelOver = false;
            //check if scores are close enough
            if (scores[0]-scores[1]< requiredScoreDistanceForWinEnding)
            {
                levelOver = true;
                //characters are close enough to win
                //show emotion emoji for both
                foreach (Patron p in activePatrons)
                {
                    p.popEmoji(p.closestEnd);
                }

            }
            //are we out of tries?
            else if (triesLeft <= 0)
            {
                levelOver = true;
                if (scores[0] - scores[1] < requiredScoreDistanceForOkayEnding)
                {
                    //out of tries and we made it to okay ending territory
                    foreach (Patron p in activePatrons)
                    {
                        p.popEmoji(p.mediumEnd);
                    }
                }
                else
                {
                    //out of tries and didn't even make it to the okay ending!
                    //out of tries and we made it to okay ending territory
                    foreach (Patron p in activePatrons)
                    {
                        p.popEmoji(p.farEnd);
                    }
                }
            }

            //is there another level?
            if (levelOver)
            {
                currentLevel++;
                if (Patrons.Length > (currentLevel+1 * 2))
                {
                    //todo show fanfare


                    ShowNextCharacters();

                }
                else
                {//the game is over

                    //todo show fanfare

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
                if (activePatrons[i] != null)
                {
                    Destroy(activePatrons[i]);
                }
                activePatrons[i] = Instantiate(Patrons[(currentLevel*2)+i], patronEntryAnimOrigin[i].position, Quaternion.identity);
                activePatrons[i].emojiBubble = emojiBubbles[i];
                activePatrons[i].EnterSeat(patronOrigins[i].transform.position, i*.3f);
                activePatrons[i].transform.SetParent(charactersGrouping);
                

            }

            UpdateTargetFlavorProfile();
            GetStartingDistance(targetFlavorProfile);
            triesLeft = numberTries;
            

        }

        #region Drinks

        public void NewDrink() {
            // TODO
            tryToServe();
        }

        #endregion
    }
}