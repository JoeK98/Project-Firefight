using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayForest()
    {
        SceneManager.LoadScene("ForestScene");
    }

    public void PlayCity()
    {
        SceneManager.LoadScene("CityScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
