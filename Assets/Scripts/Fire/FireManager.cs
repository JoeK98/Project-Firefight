using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{

    [SerializeField] private FireRules[] windows;

    public FireRules[] Windows { get => windows; set => windows = value; }

    // Start is called before the first frame update
    void Start()
    {
        int windowIndex = Random.Range(0, Windows.Length);
        Windows[windowIndex].gameObject.SetActive(true);
    }

}
