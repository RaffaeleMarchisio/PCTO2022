//includo le librerie che mi servono
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class On_Click_Options : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);  //carico la selezione dei livelli
    }
    public void main_menu()
    {
        SceneManager.LoadScene(0);  //carico il menù principale
    }
    public void level1()
    {
        SceneManager.LoadScene(2);  //carico il livello 1 
    }
    
    public void level2()
    {
        SceneManager.LoadScene(3); //carico il livello 2
    }
    public void level3()
    {
        SceneManager.LoadScene(4); //carico il livello 3
    }
    public void level4()
    {
        SceneManager.LoadScene(5); //carico il livello 4
    }
    public void Exit()
    {
        Application.Quit(); //esco dal gioco
    }
    public void Back_selection_close_socket()
    {
        FindObjectOfType<UDP_reciver>().isGameOver=true;  //cerco un oggetto di tipo UDP_reciver e pongo isGameover a true per chiudere il socket UDP di openpose
        SceneManager.LoadScene(1);  //carico la selezione dei livelli
    }
}
