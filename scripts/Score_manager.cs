using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Score_manager : MonoBehaviour
{
    //dichiarazione di variabili
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

    public void Changescore(int value)
    {
        punteggio += value;      //aggiorno il punteggio
        score.text = "SCORE:" + punteggio; //aggiorno il testo del punteggio
    }


}
