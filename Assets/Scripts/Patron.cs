using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace ggj.rootbeer
{
    public class Patron : MonoBehaviour
    {
        public FlavorProfile FlavorProfile;
        public List<ToppingName> PreferredToppings;
        public List<ToppingName> HatedToppings;
        public Transform speakingAnchor;
        public SpriteRenderer overlayLimbs;
        [HideInInspector]public EmojiBubble emojiBubble;

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
            }
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

            return FlavorProfile.GetDistance(d.FlavorProfile);
        }

        public void Scooch(float score, Vector3 furthestPoint, Vector3 targetPoint)
        {
            Sequence sequence = DOTween.Sequence();
            transform.DOMove(Vector3.Lerp(furthestPoint, targetPoint, score), 2f).SetEase(Ease.OutSine);
        }

        public void EnterSeat(Vector3 target, float delay)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay + .5f);
            sequence.Append(transform.DOMove(target, 1f).SetEase(Ease.OutBack));
            sequence.InsertCallback(1.3f, () => { if (overlayLimbs != null) { overlayLimbs.sortingOrder = 10; }});
            sequence.AppendInterval(.2f);
            sequence.Append(popEmoji(firstImpression));
        }

        public void ExitSeat(float delay)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delay + .5f);
            sequence.Append(transform.DOMoveY(-10, 1f).SetEase(Ease.InQuad));
            sequence.InsertCallback(1.5f, () => Destroy(gameObject));
        }

        public Sequence popEmoji(Emojimotion emote)
        {
            return emojiBubble.popEmoji(emote);
        }

        public Sequence doublePopEmoji(Emojimotion emote, Emojimotion emote2)
        {
            return emojiBubble.popEmojiThenReturnToSecondEmoji(emote, emote2);
        }

        public Sequence popAndHideEmoji(Emojimotion emotion)
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(emojiBubble.fadeEmoji());
            sq.AppendInterval(.3f);
            sq.Append(emojiBubble.popEmoji(emotion));
            sq.AppendInterval(3f);
            sq.Append(emojiBubble.fadeEmoji());
            return sq;
        }


    }
}
