using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class On_Click_Options : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void main_menu()
    {
        SceneManager.LoadScene(0);
    }
    public void level1()
    {
        SceneManager.LoadScene(2);
    }
    
    public void level2()
    {
        SceneManager.LoadScene(3);
    }
    public void level3()
    {
        SceneManager.LoadScene(4);
    }
    public void level4()
    {
        SceneManager.LoadScene(5);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Back_selection_close_socket()
    {
        FindObjectOfType<UDP_reciver>().isGameOver=true;
        SceneManager.LoadScene(1);
    }
}
