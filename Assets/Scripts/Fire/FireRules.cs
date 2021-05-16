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

    // Start is called before the first frame update
    void Start()
    {
        fireMaterial = GetComponent<Renderer>().material;
   
    }

    // Update is called once per frame
    void Update()
    {
        Burning();
    }

    void Burning()
    {
        if(fireHP <=100)
        {
            fireHP += fireEvolution*Time.deltaTime*10;    
        }
        
        if(fireHP >= 100)
        {
            int neighbourIndex = Random.Range(0, neighbour.Count);
            neighbour[neighbourIndex].gameObject.SetActive(true);
        }      
            
        fireMaterial.color = new Color(0.01f * fireHP, 0.0f, 0.0f);
    }


}
