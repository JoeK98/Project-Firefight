using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to overlook the tutorial and change the progress
/// <author> Vincent Holtorf </author>
/// </summary>
public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// Activate random the first fire at the start of the level
    /// </summary>
    [SerializeField]
    private List<GameObject> tutTexts = new List<GameObject>();

    /// <summary>
    /// Int to check where the player is in the Tutorial
    /// </summary>
    [SerializeField]
    private int tutProgress = 0;

    [SerializeField]
    private bool cityScene = false;
    [SerializeField]
    private bool forestScene = false;
    [SerializeField]
    private bool englishTexts= false;
    [SerializeField]
    private bool germanTexts = false;

    public int TutProgress { get => tutProgress; set => tutProgress = value; }
    public bool GermanTexts { get => germanTexts; set => germanTexts = value; }
    public bool EnglishTexts { get => englishTexts; set => englishTexts = value; }
    public bool ForestScene { get => forestScene; set => forestScene = value; }
    public bool CityScene { get => cityScene; set => cityScene = value; }

    /// <summary>
    /// Function to change the Task, its activate the next task and deactivated the last task 
    /// </summary>
    public void ChangeTask()
    {

        tutTexts[tutProgress].gameObject.SetActive(true);
        tutTexts[tutProgress - 1].gameObject.SetActive(false);

    }

}
