using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRules : MonoBehaviour
{

    public float fireHP = 0.0f;
    public float fireEvolution = 0.75f;
    private Material fireMaterial;
    [SerializeField]
    private List<FireRules> neighbour;
    [SerializeField]
    private FireStates state = FireStates.NONE;

    // Start is called before the first frame update
    void Start()
    {
        fireMaterial = GetComponent<Renderer>().material;
   
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    void OnEnable()
    {
        state = FireStates.ONFIRE;
    }

    void CheckState()
    {
        switch (state)
        {
            case FireStates.NONE:
                gameObject.SetActive(false);
                break;
            case FireStates.ONFIRE:
                if (fireHP <= 100 && fireHP >= 0)
                {
                    fireHP += fireEvolution * Time.deltaTime * 10;
                    fireMaterial.color = new Color(0.01f*fireHP,0f,0f);
                }

                if (fireHP >= 100)
                {
                    int neighbourIndex = Random.Range(0, neighbour.Count);
                    neighbour[neighbourIndex].gameObject.SetActive(true);
                }

                if (fireHP < 0)
                {
                    state = FireStates.PUTOUT;
                }
                break;
            case FireStates.PUTOUT:
                if (fireHP < 0)
                {
                    fireMaterial.color = Color.blue;
                }
                break;
        }
    }

}
