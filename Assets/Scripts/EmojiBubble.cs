using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Emojimotion
{
    Happy = 0,
    Sad = 1
}

public class EmojiBubble : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image emoji;
    public Image rockingBubble;
    public Sprite[] emojiIcons;

    


    // Start is called before the first frame update
    void Start()
    {
        
        Sequence sq = DOTween.Sequence();
        sq.Append(rockingBubble.transform.DORotate(new Vector3(0, 0, 5.0f), 5f));
        sq.AppendInterval(.1f);
        sq.Append(rockingBubble.transform.DORotate(new Vector3(0, 0, -5.0f), 5f));
        sq.AppendInterval(.1f);
        sq.SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sequence popEmoji(Emojimotion emote)
    {
        
        Sequence sq = DOTween.Sequence();
        this.gameObject.SetActive(true);
        
        sq.AppendCallback(() =>
        {
            emoji.sprite = emojiIcons[(int)emote];
            emoji.color = new Color(emoji.color.r, emoji.color.g, emoji.color.b, 0);
        }
        );
        sq.Append(canvasGroup.DOFade(1, .4f));
        sq.Insert(0.2f, emoji.DOFade(1, .4f));
        sq.AppendInterval(1f);
        

        return sq;
    }

    public Sequence fadeEmoji()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(canvasGroup.DOFade(0, .4f));
        sq.Insert(0.2f, emoji.DOFade(0, .4f));
        return sq;
    }

    public Sequence popEmojiThenReturnToSecondEmoji(Emojimotion emote, Emojimotion emote2)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(fadeEmoji());
        sq.AppendInterval(.3f);
        sq.Append(popEmoji(emote));
        sq.AppendInterval(2f);
        sq.Append(fadeEmoji());
        sq.Append(popEmoji(emote2));
        return sq;
    }
     
}
