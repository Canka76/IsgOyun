using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    public GameObject credit_panel;
    
    public void Exit()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("kosor");
    }

    public void ToCredits()
    {
        credit_panel.active = true;
    }

    public void FromCredits()
    {
        credit_panel.active = false;
    }

    
}
