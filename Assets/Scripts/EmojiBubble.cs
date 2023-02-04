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
    public Sprite[] emojiIcons;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sequence popEmoji(Emojimotion emote)
    {
        
        Sequence sq = DOTween.Sequence();
        this.gameObject.SetActive(true);
        canvasGroup.alpha = 0;
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
        sq.Insert(sq.Duration() - 0.2f, emoji.DOFade(0, .4f));
        return sq;
    }

    public Sequence popEmojiThenReturnToSecondEmoji(Emojimotion emote, Emojimotion emote2)
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(fadeEmoji());
        sq.AppendInterval(.3f);
        sq.Append(popEmoji(emote));
        sq.AppendInterval(3f);
        sq.Append(fadeEmoji());
        sq.Append(popEmoji(emote2));
        return sq;
    }
     
}
