using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Changed the actuelle Scene from Menue to the Forest Level
    /// </summary>
    public void OnPlayForest()
    {
        SceneManager.LoadScene("ForestScene");
    }

    /// <summary>
    /// Changed the actuelle Scene from Menue to the City Level
    /// </summary>
    public void OnPlayCity()
    {
        SceneManager.LoadScene("CityScene");
    }

    /// <summary>
    /// End the Application
    /// </summary>
    public void OnQuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
