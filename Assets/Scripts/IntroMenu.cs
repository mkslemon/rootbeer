using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchGame()
    {
        if (FMODUnity.RuntimeManager.HasBankLoaded("Master Bank"))
        {
            Debug.Log("Master Bank Loaded");
        }
        SceneManager.LoadScene(1);
    }
}
