using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    /// <summary>
    /// Connection to give the TMpro the diffrent strings for the texts
    /// </summary>
    [SerializeField]
    private TutorialTextSO SOLangugeBool = null;

   public void OnClickEnglish()
    {
        SOLangugeBool.textIsEnglish = true;
    }

    public void OnClickGerman()
    {
        SOLangugeBool.textIsEnglish = false;
    }
}
