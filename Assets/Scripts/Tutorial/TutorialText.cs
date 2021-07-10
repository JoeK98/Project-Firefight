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
    private TutorialManager tM;

    [SerializeField]
    [Tooltip("Number from TMPro")]
    private int tutorialNumber;

    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (tM.CityScene)
        {
            if (tM.EnglishTexts)
            {
                textMeshPro.text = tutorialText._tutTextsEnglishCity[tutorialNumber];
            }
            else if (tM.GermanTexts)
            {
                textMeshPro.text = tutorialText._tutTextsGermanCity[tutorialNumber];
            }
        }
        else if (tM.ForestScene)
        {
            if (tM.EnglishTexts)
            {
                textMeshPro.text = tutorialText._tutTextsEnglishForest[tutorialNumber];
            }
            else if (tM.GermanTexts)
            {
                textMeshPro.text = tutorialText._tutTextsGermanForest[tutorialNumber];
            }
        }
    }




}
