using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialConnectionControler : MonoBehaviour
{
    [SerializeField][Tooltip ("Number from TMPro-1")]
    private int _tutorialNumber;
    [SerializeField]
    private TutorialManager _tutManger;

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
    }

    //Interactable Event from the XR-Interactable Toolkit
    public void OnSelectEntered()
    {
        if(_tutorialNumber == _tutManger.TutProgress)
        {
            _tutManger.TutProgress += 1;
            _tutManger.ChangeTask();
        }
    }

}
