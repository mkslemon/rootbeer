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
    public List<AudioSource> drinkingSound;
    public AudioSource strawsPopin;
    public List<AudioSource> characterMoves;
    public AudioSource characterMovesSlow;
    public AudioSource characterMovesFast;


    public AudioSource pickRandom(List<AudioSource> audioSources)
    {
        return audioSources[Random.Range(0, audioSources.Count)];

    }

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
