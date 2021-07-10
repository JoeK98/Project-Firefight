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

    /// <summary>
    /// Connection to look in what level we are 
    /// </summary>
    [SerializeField]
    private TutorialManager tM;

    /// <summary>
    /// Check what text is needed at the moment
    /// </summary>
    [SerializeField]
    [Tooltip("Number from TMPro")]
    private int tutorialNumber;

    /// <summary>
    /// Variable to change the Text over the script
    /// </summary>
    private TextMeshProUGUI textMeshPro;

    /// <summary>
    /// To look if the Text is needed in english or german and in what level we are
    /// </summary>
    private void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();

        if (tM.CityScene)
        {
            if (tutorialText.textIsEnglish)
            {
                textMeshPro.text = tutorialText._tutTextsEnglishCity[tutorialNumber];
            }
            else if (!tutorialText.textIsEnglish)
            {
                textMeshPro.text = tutorialText._tutTextsGermanCity[tutorialNumber];
            }
        }
        else if (tM.ForestScene)
        {
            if (tutorialText.textIsEnglish)
            {
                textMeshPro.text = tutorialText._tutTextsEnglishForest[tutorialNumber];
            }
            else if (!tutorialText.textIsEnglish)
            {
                textMeshPro.text = tutorialText._tutTextsGermanForest[tutorialNumber];
            }
        }
    }




}
