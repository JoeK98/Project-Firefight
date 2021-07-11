using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectActivat : MonoBehaviour
{
    [SerializeField, Tooltip ("Put here the field for the task")]
    private GameObject obj;

    private void OnEnable()
    {
        obj.SetActive(true);
    }

    private void OnDisable()
    {
        obj.SetActive(false);
    }



}
