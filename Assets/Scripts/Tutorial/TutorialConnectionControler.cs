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
    private TutorialManager _tutManger;
    [SerializeField][Tooltip ("Number from TMPro-1")]
    private int _tutorialNumber;

    /// <summary>
    /// double check if the connection that the player maked has the right tag and he make it at the right time
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("TubeB"))
        {
            if (_tutorialNumber == _tutManger.TutProgress)
            {
                _tutManger.TutProgress += 1;
                _tutManger.ChangeTask();
            }
            
        }
        else if(other.gameObject.CompareTag("TubeC"))
        {
            if (_tutorialNumber == _tutManger.TutProgress)
            {
                _tutManger.TutProgress += 1;
                _tutManger.ChangeTask();
            }
        }
        else if (other.gameObject.CompareTag("Connection"))
        {
            if (_tutorialNumber == _tutManger.TutProgress)
            {
                _tutManger.TutProgress += 1;
                _tutManger.ChangeTask();
            }
        }
        else if (other.gameObject.CompareTag("HydrantKey"))
        {
            if (_tutorialNumber == _tutManger.TutProgress)
            {
                _tutManger.TutProgress += 1;
                _tutManger.ChangeTask();
            }
        }
    }

    /// <summary>
    /// Interactable Event from the XR-Interactable Toolkit & double checked if the action is maked on the right time
    /// </summary>
    public void OnSelectEntered()
    {
        if(_tutorialNumber == _tutManger.TutProgress)
        {
            _tutManger.TutProgress += 1;
            _tutManger.ChangeTask();
        }
    }

}
