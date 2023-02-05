using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor.XR;

namespace ggj.rootbeer
{
    public class Patron : MonoBehaviour
    {
        public FlavorProfile FlavorProfile;
        public List<ToppingName> PreferredToppings;
        public List<ToppingName> HatedToppings;
        public Transform speakingAnchor;
        public Transform altSpeakingAnchor;
        public SpriteRenderer overlayLimbs;
        [HideInInspector]public EmojiBubble emojiBubble;
        [HideInInspector] public bool satisfied;
        [HideInInspector] public bool toBeReplaced;

        [Header("Likes/Dislikes")]
        public float flavorCitrus = .5f;
        public float flavorFloral = .5f;
        public float flavorSweet = .5f;
        public float flavorExotic = .5f;


        [Header("Emojis")]
        public Emojimotion firstImpression;
        public Emojimotion closeEmoji;
        public Emojimotion mediumEmoji;
        public Emojimotion farEmoji;
        public Emojimotion closestEnd;
        public Emojimotion mediumEnd;
        public Emojimotion farEnd;
        public Emojimotion hint;
        

        float lastScore;

        
        // Start is called before the first frame update
        void Start()
        {
            FlavorProfile = new FlavorProfile(flavorCitrus, flavorFloral, flavorSweet, flavorExotic);
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
            if(emojiBubble != null && speakingAnchor !=null)
            {
                emojiBubble.transform.position = Camera.main.WorldToScreenPoint(speakingAnchor.position);
                if (emojiBubble.isReversed)
                {
                    emojiBubble.transform.position = Camera.main.WorldToScreenPoint(altSpeakingAnchor.position);
                }
            }
            
        }


        public void anchorBubble(EmojiBubble eb)
        {
            emojiBubble= eb;
            eb.Reversed(false);
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
            lastScore = FlavorProfile.GetDistance(d.FlavorProfile);
            return lastScore;
        }

        public void Scooch(float score, Vector3 furthestPoint, Vector3 targetPoint)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(Vector3.Lerp(furthestPoint, targetPoint, score), 2f).SetEase(Ease.OutSine));
            sequence.AppendInterval(1f);
            sequence.AppendCallback(
                () =>
                {
                    if (satisfied)
                    {
                        emojiBubble.gameObject.SetActive(false);
                        ExitSeat(0);
                    }
                }
                );
        }

        public void EnterSeat(Vector3 target, float delay)
        {
            DOTween.Complete(emojiBubble.transform);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay + .5f);
            sequence.AppendCallback(() => { emojiBubble.gameObject.SetActive(true); });
            sequence.Append(transform.DOMove(target, 1f).SetEase(Ease.OutBack));
            sequence.InsertCallback(1.3f, () => { if (overlayLimbs != null) { overlayLimbs.sortingOrder = 10; }});
            sequence.AppendInterval(.2f);
            sequence.Append(popEmoji(firstImpression));
        }

        public Sequence ExitSeat(float delay)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay + .5f);
            sequence.AppendCallback(() => { if (overlayLimbs != null) { overlayLimbs.sortingOrder = 1; } });
            sequence.Append(emojiBubble.fadeEmoji());
            sequence.Append(transform.DOMoveY(-20, 1f).SetEase(Ease.InQuad));
            sequence.InsertCallback(1.5f, () => { Destroy(gameObject); });
            sequence.AppendInterval(2f);
            sequence.AppendCallback(() => { 
                if (toBeReplaced)
                {
                    GameManager._GAMEMANAGER.TryShowNextCharacters();
                }
            });
            return sequence;
        }

        public Sequence popEmoji(Emojimotion emote)
        {
            return emojiBubble.popEmoji(emote);
        }

        public Sequence doublePopEmoji(Emojimotion emote, Emojimotion emote2)
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(emojiBubble.fadeEmoji());
            sq.AppendInterval(.3f);
            sq.AppendCallback(() =>
            {
                if (lastScore > .5)
                {
                    emojiBubble.Reversed(true);
                }
                else
                {
                    emojiBubble.Reversed(false);
                }
            });
            sq.Append(emojiBubble.popEmoji(emote));
            sq.AppendInterval(.3f);
            sq.Append(emojiBubble.fadeEmoji());
            sq.AppendInterval(.3f);
            sq.Append(emojiBubble.popEmoji(emote2));
            return sq;
        }

        public Sequence popAndHideEmoji(Emojimotion emotion)
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(emojiBubble.fadeEmoji());
            sq.AppendInterval(.3f);
            sq.AppendCallback(() =>
            {
                if (lastScore > .5)
                {
                    emojiBubble.Reversed(true);
                }
                else
                {
                    emojiBubble.Reversed(false);
                }
            });
            sq.Append(emojiBubble.popEmoji(emotion));
            sq.AppendInterval(3f);
            sq.Append(emojiBubble.fadeEmoji());
            return sq;
        }


    }
}
