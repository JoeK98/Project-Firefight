using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _tutTexts = new List<GameObject>();

    private int _tutProgress = 0;

    public void ChangeTask()
    {

        _tutTexts[_tutProgress].gameObject.SetActive(true);
        _tutTexts[_tutProgress - 1].gameObject.SetActive(false);

        //TODO Delete when Code above works
        //switch (_tutProgress)
        {
          /*case 0:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                break;
            case 1:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress-1].gameObject.SetActive(false);
                break;
            case 2:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 3:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 4:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 5:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 6:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 7:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 8:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 9:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 10:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 11:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 12:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 13:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 14:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 15:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 16:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 17:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 18:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;
            case 19:
                _tutorialTexts[_tutProgress].gameObject.SetActive(true);
                _tutorialTexts[_tutProgress - 1].gameObject.SetActive(false);
                break;*/
        }
    }
    
}
