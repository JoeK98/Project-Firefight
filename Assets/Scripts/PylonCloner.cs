using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objectpool Class for the Pylon 
/// <author> Vincent Holtorf </author>
/// </summary>
public class PylonCloner : MonoBehaviour
{

    /// <summary>
    /// Connection for the Manager and the step the connection has in the tutprial
    /// </summary>
    [SerializeField]
    private TutorialManager tM;

    /// <summary>
    /// Defined the pools that are used, the tag is for the diffrent pools, the Gameobject what Object are in the pool and the size how big the pool is
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject pylonPrefab;
        public int size;
    }

    /// <summary>
    /// A List of the pools in the Scene, at the moment only for the Pylon
    /// </summary>
    public List<Pool> pools;

    /// <summary>
    /// In the Dictonary all objects from one pool are saved
    /// </summary>
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

    /// <summary>
    /// Filled the Dictonary with the Objects at the start of the Scene
    /// </summary>
    void Start()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

         foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.pylonPrefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// Checked what instnce in the pool is the next to active
    /// </summary>
    public GameObject GetPylonInstance()
    {
        foreach (KeyValuePair<string, Queue<GameObject>> entry in _poolDictionary)
        {
            foreach (GameObject tmp in entry.Value)
            {
                if (!tmp.activeSelf)
                {
                    tmp.SetActive(true);
                    return tmp;
                }
            }
          
        }

        return null;

    }

    /// <summary>
    /// Changed the progres for the Tutorial
    /// </summary>
    public void OnGrabObject()
    {
        GetPylonInstance();

        if (tM.TutProgress == 0)
        {
            tM.TutProgress += 1;
            tM.ChangeTask();
        }
        else if(tM.TutProgress == 2)
        {
            tM.TutProgress += 1;
            tM.ChangeTask();
        }
    }



}
