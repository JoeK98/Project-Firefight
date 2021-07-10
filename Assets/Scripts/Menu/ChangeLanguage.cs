using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class to change the language of the tutorial
/// <author> Vincent Holtorf </author>
/// </summary>
public class ChangeLanguage : MonoBehaviour
{
    /// <summary>
    /// Connection to give the TMpro the diffrent strings for the texts
    /// </summary>
    [SerializeField]
    private TutorialTextSO SOLangugeBool = null;

    /// <summary>
    /// Unity Event, changed the language of the tutorial to english
    /// </summary>
    public void OnClickEnglish()
    {
        SOLangugeBool.textIsEnglish = true;
    }

    /// <summary>
    /// Unity Event, changed the language of the tutorial to german
    /// </summary>
    public void OnClickGerman()
    {
        SOLangugeBool.textIsEnglish = false;
    }
}
