using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgm;
    public AudioSource bgm2;
    public AudioSource ambiance;
    public AudioSource pickupIngredient;
    public AudioSource dropIngredient;
    public AudioSource addIngredient;
    public AudioSource drinkingSound;
    public AudioSource strawsPopin;
    public AudioSource characterMoves;
    public AudioSource characterMovesSlow;
    public AudioSource characterMovesFast;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
