using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ggj.rootbeer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _GAMEMANAGER;
        public Patron[] activePatrons;

    private void Awake()
        {
            _GAMEMANAGER = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}