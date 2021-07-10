using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    /// <summary>
    /// Connection to give the TMpro the diffrent strings for the texts
    /// </summary>
    [SerializeField]
    private TutorialTextSO tutorialText = null;

    [SerializeField]
    [Tooltip("Number from TMPro")]
    private int tutorialNumber;

    private TextMeshPro textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = tutorialText._tutTextsGermanCity[tutorialNumber];
    }




}
