using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _tutTexts = new List<GameObject>();

    [SerializeField]
    private int _tutProgress = 0;

    public int TutProgress { get => _tutProgress; set => _tutProgress = value; }

    public void ChangeTask()
    {

        _tutTexts[_tutProgress].gameObject.SetActive(true);
        _tutTexts[_tutProgress - 1].gameObject.SetActive(false);

    }

    // Nur zum testen um den Progress höher zu schalten
    public void TutDebug()
    {
        _tutProgress++;
    }

}
