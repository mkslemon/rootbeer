using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource pickupIngredient;
    public AudioSource dropIngredient;
    public AudioSource addBase;
    public AudioSource addGarnish;
    public AudioSource addMixin;
    public AudioSource drinkingSound;
    public AudioSource strawsPopin;
    public AudioSource characterMovesIn;
    public AudioSource characterMovesOut;


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
