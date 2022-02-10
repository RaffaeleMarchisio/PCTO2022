using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Score_manager instance;
    public Text score;
    public int punteggio;
    void Start()
    { 
        punteggio = 0;
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Changescore(int value)
    {
        punteggio += value;
        score.text = "SCORE:" + punteggio;
    }


}
