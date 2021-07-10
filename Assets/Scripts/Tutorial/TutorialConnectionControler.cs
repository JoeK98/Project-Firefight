using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class that controls when the Firesound need to stop
/// <author> Vincent Holtorf </author>
/// </summary>
public class TutorialConnectionControler : MonoBehaviour
{
    /// <summary>
    /// Connection for the Manager and the step the connection has in the tutprial
    /// </summary>
    [SerializeField]
    private TutorialManager tutManger;

    [SerializeField][Tooltip ("Number from TMPro-1")]
    private int tutorialNumber;

    public int TutorialNumber { get => tutorialNumber; set => tutorialNumber = value; }

    /// <summary>
    /// double check if the connection that the player maked has the right tag and he make it at the right time
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("TubeB"))
        {
            if (TutorialNumber == tutManger.TutProgress)
            {
                tutManger.TutProgress += 1;
                tutManger.ChangeTask();
            }
            
        }
        else if(other.gameObject.CompareTag("TubeC"))
        {
            if (TutorialNumber == tutManger.TutProgress)
            {
                tutManger.TutProgress += 1;
                tutManger.ChangeTask();
            }
        }
        else if (other.gameObject.CompareTag("Connection"))
        {
            if (TutorialNumber == tutManger.TutProgress)
            {
                tutManger.TutProgress += 1;
                tutManger.ChangeTask();
            }
        }
        else if (other.gameObject.CompareTag("HydrantKey"))
        {
            if (TutorialNumber == tutManger.TutProgress)
            {
                tutManger.TutProgress += 1;
                tutManger.ChangeTask();
            }
        }
    }

    /// <summary>
    /// Interactable Event from the XR-Interactable Toolkit & double checked if the action is maked on the right time
    /// </summary>
    public void OnSelectEntered()
    {
        if(TutorialNumber == tutManger.TutProgress)
        {
            tutManger.TutProgress += 1;
            tutManger.ChangeTask();
        }
    }

}
