using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object that saves the text for the english and german tutorial
/// <author> Vincent Holtorf </author>
/// </summary>
[CreateAssetMenu]
public class TutorialTextSO : ScriptableObject
{
    /// <summary>
    /// A List of all german texts for the german City tutorial
    /// </summary>
    public List<string> _tutTextsGermanCity = new List<string>();

    /// <summary>
    /// A List of all english texts for the english City tutorial
    /// </summary>
    public List<string> _tutTextsEnglishCity = new List<string>();

    /// <summary>
    /// A List of all german texts for the german Forest tutorial
    /// </summary>
    public List<string> _tutTextsGermanForest = new List<string>();

    /// <summary>
    /// A List of all english texts for the english Forest tutorial
    /// </summary>
    public List<string> _tutTextsEnglishForest = new List<string>();

    public bool textIsEnglish = false;
}
