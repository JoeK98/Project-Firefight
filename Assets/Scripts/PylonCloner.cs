using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonCloner : MonoBehaviour
{
    [SerializeField]
    private GameObject _pylonOriginal;

    public void OnGrabCloneObject()
    {
        GameObject _pylonClone = Instantiate(_pylonOriginal);
    }
}
