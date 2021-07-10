using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PylonCloner : MonoBehaviour
{
    
    [SerializeField]
    private TutorialManager tM;


    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject pylonPrefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> _poolDictionary;

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

    public GameObject GetPylonInstance()
    {
        foreach (KeyValuePair<string, Queue<GameObject>> entry in _poolDictionary)
        {
            foreach (GameObject tmp in entry.Value)
            {
                if (!tmp.active)
                {
                    tmp.SetActive(true);
                    return tmp;
                }
            }
          
        }

        return null;

    }

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
