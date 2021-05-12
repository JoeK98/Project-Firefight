using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{

    [SerializeField] private GameObject[] windows;

    // Start is called before the first frame update
    void Start()
    {
        int windowIndex = Random.Range(0, windows.Length);
        windows[windowIndex].SetActive(true);
    }

}
