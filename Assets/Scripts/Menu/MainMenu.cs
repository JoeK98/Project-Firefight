using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for the function of the menu
/// <author> Vincent Holtorf </author>
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Changed the current Scene from Menue to the Forest Level
    /// </summary>
    public void OnPlayForest()
    {
        SceneManager.LoadScene("ForestScene");
    }

    /// <summary>
    /// Changed the current Scene from Menue to the City Level
    /// </summary>
    public void OnPlayCity()
    {
        SceneManager.LoadScene("CityScene");
    }

    /// <summary>
    /// Changed the current Scene from one Level to the Menu
    /// </summary>
    public void OnBackToMenu()
    {
        SceneManager.LoadScene("Menue");
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
