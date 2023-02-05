using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace ggj.rootbeer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _GAMEMANAGER;

        [Header("Game Tuning")]
        public int numberTries;
        [HideInInspector] public int triesLeft;
        public float requiredScoreDistanceForWinEnding = 0.3f;
        public float requiredScoreDistanceForOkayEnding = 0.6f;
        public float happyZone = .85f;
        public float mediumZone = .7f;

        [Header("Game Object References")]
        [HideInInspector] public Patron[] activePatrons =  new Patron[2];
        public Patron[] Patrons;
        public Transform[] patronOrigins;
        public Transform[] endTargets;
        private FlavorProfile targetFlavorProfile;
        public float PatronStartingDistanceToTarget;
        public Transform[] patronEntryAnimOrigin;
        public Transform charactersGrouping;
        public EmojiBubble[] emojiBubbles;
        public TMPro.TextMeshProUGUI triesLeftText;
        public CanvasGroup winBlurb;
        public CanvasGroup helpBlurb;
        public GameObject[] heartsLeft;
        public GameObject[] winHearts;
        public CanvasGroup endOfGameGroup;
        bool firstTime = true;

        int currentLevel=0;
        int wins = 0;

        [SerializeField] private List<Ingredient> _ingredients;
        [SerializeField] List<Color> _ingredientColors;
        [SerializeField] List<Material> _toppingMaterials;

        FMODUnity.StudioEventEmitter emitter;

        private void Awake()
        {
            _GAMEMANAGER = this;
            activePatrons = new Patron[2];

            _ingredients[0]._juice = new Juice("Sassafras Juice", _ingredientColors[0], "", new FlavorProfile(0f, 1f, 0f, 1f));
            _ingredients[1]._juice = new Juice("Ginger Juice", _ingredientColors[1], "", new FlavorProfile(1f, 1f, 0f, 0f));
            _ingredients[2]._juice = new Juice("Orange Juice", _ingredientColors[2], "", new FlavorProfile(1f, 0f, 1f, 0f));


            _ingredients[3]._syrup = new Syrup("Grenadine Syrup", _ingredientColors[3], "", new FlavorProfile(0f, 1f, 1f, 0f));
            _ingredients[4]._syrup = new Syrup("Coconut Creamer", _ingredientColors[4], "", new FlavorProfile(1f, 0f, 0f, 1f));
            _ingredients[5]._syrup = new Syrup("Blueberry Syrup", _ingredientColors[5], "", new FlavorProfile(0f, 0f, 1f, 01f));

            _ingredients[6]._topping = new Topping("Mint Garnish", _toppingMaterials[0], "", new FlavorProfile(0f, 0.5f, 0.5f, 0.5f));
            _ingredients[7]._topping = new Topping("Cherry Garnish", _toppingMaterials[1], "", new FlavorProfile(0.5f, 0.5f, 0.5f, 0f));
            _ingredients[8]._topping = new Topping("Lime Garnish", _toppingMaterials[2], "", new FlavorProfile(0.5f, 0f, 0.5f, 0.5f));
        }


        public void tryToServe()
        {
            //compare to preferences to generate a score (0 to 1.0)
            float[] scores = activePatrons.Select(sel => sel.Score(Drink.Instance)).ToArray();
            foreach (var score in scores)
            {
                Debug.Log("Score: " + score);
            }

            // FMOD Parameter Trigger is stored on the main camera
            var mainCamera = GameObject.Find("Main Camera");
            emitter = GameObject.Find("Main Camera").GetComponent<FMODUnity.StudioEventEmitter>();

            //do we need to reject a served drink because player forgot something in it?

            //process the submission
            triesLeft--;;
            updateTriesString();
            //for each patron, compare the served drinks then see how they react
            for (int p= 0; p<activePatrons.Length; p++)
            {
                Debug.Log("patron " +p+ " scores " + (scores[p]));

                //move the patron closer to the drink if liked or closer to their origin point if disliked
                //pass a float 0 to 1.0 along with 
                activePatrons[p].Scooch(scores[p], patronOrigins[p].transform.position, endTargets[p].transform.position, p*.1f);
                //
                if (scores[p] >= happyZone)
                {
                    //characters are close enough to win
                    //show emotion emoji
                    activePatrons[p].doublePopEmoji(activePatrons[p].hint, activePatrons[p].closeEmoji);
                    emitter.SetParameter("Proximity", 2.0f);
                }
                else if (scores[p] >=  mediumZone)
                {
                    //we made it to okay territory
                    //show emotion emoji and show hint emoji
                    activePatrons[p].doublePopEmoji(activePatrons[p].hint, activePatrons[p].mediumEmoji);
                    emitter.SetParameter("Proximity", 1.0f);
                }
                else
                {
                    //we're far away
                    //show emotion emoji and show hint emoji
                    activePatrons[p].doublePopEmoji(activePatrons[p].hint, activePatrons[p].mediumEmoji);
                    emitter.SetParameter("Proximity", 0.0f);
                }




            }

            Bar.Instance.ClearDrink();

            bool levelOver = false;
            //check if scores are close enough
            if ((1-scores[0])+(1-scores[1])< requiredScoreDistanceForWinEnding)
            {
                levelOver = true;
                wins++;
                //characters are close enough to win
                //show emotion emoji for both
                foreach (Patron p in activePatrons)
                {
                    p.popEmoji(p.closestEnd);
                }
                Sequence sq = DOTween.Sequence();
                sq.AppendInterval(3f);
                sq.Append(winBlurb.DOFade(1, .4f));
                sq.AppendInterval(.4f);
                sq.Append(winBlurb.DOFade(0, .4f));

            }
            //are we out of tries?
            else if (triesLeft <= 0)
            {
                levelOver = true;
                if ((1 - scores[0]) + (1 - scores[1]) < requiredScoreDistanceForOkayEnding)
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
                foreach (Patron p in activePatrons)
                {
                    //p.ExitSeat(0);
                    p.satisfied = true;
                    //they'll exit their seats after their anim
                    if (Patrons.Length > (currentLevel + 1 * 2))
                    {
                        p.toBeReplaced = true;
                    }
                }

                emitter.SetParameter("Proximity", 0.0f);
                
                
                if(Patrons.Length <= (currentLevel * 2))
                {//the game is over

                    endOfGameGroup.DOFade(1f, 1f).SetDelay(2f);
                    for(int i = wins; i<heartsLeft.Length; i++)
                    {
                        heartsLeft[i].gameObject.SetActive(false);
                    }
                    emitter.SetParameter("Result", 1);
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
        public float GetNormalizedDistance(Patron p, FlavorProfile otherFlavorProfile)
        {
            return p.FlavorProfile.GetDistance(otherFlavorProfile) / PatronStartingDistanceToTarget;
        }


        // Start is called before the first frame update
        void Start()
        {
            TryShowNextCharacters();
        }

        // Update is called once per frame
        void Update()
        {

        }


        //take a dependency tht there are 2+ charas and an even number
        public void TryShowNextCharacters()
        {
            for(int i=0; i<2; i++)
            {
                if (activePatrons[i] != null)
                {
                    continue;
                    
                }
                else
                {
                    activePatrons[i] = Instantiate(Patrons[(currentLevel * 2) + i], patronEntryAnimOrigin[i].position, Quaternion.identity);
                    activePatrons[i].anchorBubble(emojiBubbles[i]);
                    activePatrons[i].EnterSeat(patronOrigins[i].transform.position, i * .3f);
                    activePatrons[i].transform.SetParent(charactersGrouping);
                }
            }

            //UpdateTargetFlavorProfile();
            //GetStartingDistance(targetFlavorProfile);
            triesLeft = numberTries;
            updateTriesString();
            if(firstTime == true)
            {
                firstTime = false;
                Sequence sq = DOTween.Sequence();
                sq.AppendInterval(3f);
                sq.Append(helpBlurb.DOFade(1, 1f));
                sq.AppendInterval(2f);
                sq.Append(helpBlurb.DOFade(0, 2f));

            }
        }



        void updateTriesString()
        {
            string chancesString = "";
            for (int i = 0; i < numberTries; i++)
            {
                chancesString += "[";
                if (i < (numberTries-triesLeft))
                {
                    chancesString += "x";
                    heartsLeft[i].SetActive(false);
                }
                else
                {
                    chancesString += " ";
                    heartsLeft[i].SetActive(true);
                }
                chancesString += "]";
            }

            triesLeftText.text = chancesString;
        }
        #region Drinks

        public void NewDrink() {
            UpdateTargetFlavorProfile();
            GetStartingDistance(targetFlavorProfile);
            tryToServe();
        }

        #endregion
    }
}