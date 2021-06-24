using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireRules : MonoBehaviour
{
    private const float FIRE_LOWER_BORDER = 0.0f;

    private float fireHP = 0.0f;
    private Material fireMaterial;

    [SerializeField]
    private float particleDamage = 1.0f;

    [SerializeField]
    private float fireUpperBorder = 1000.0f;

    [SerializeField]
    private float fireMultiplicator = 2.0f;

    [SerializeField]
    private List<FireRules> neighbours;

    [SerializeField]
    private FireStates state = FireStates.NONE;

    // Start is called before the first frame update
    private void Start()
    {
        fireMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckState();
    }

    private void OnEnable()
    {
        state = FireStates.ONFIRE;
        fireHP += 0.001f;
    }

    private void CheckState()
    {
        switch (state)
        {
            case FireStates.NONE:
                gameObject.SetActive(false);
                break;
            case FireStates.ONFIRE:
                if (fireHP < fireUpperBorder && fireHP >= FIRE_LOWER_BORDER)
                {
                    fireHP = Mathf.Clamp(fireHP + fireMultiplicator * Time.deltaTime, FIRE_LOWER_BORDER, fireUpperBorder);
                    fireMaterial.color = new Color(1.0f / fireUpperBorder * fireHP, 0.0f, 0.0f);
                }

                else if (fireHP >= fireUpperBorder)
                {
                    int neighbourIndex = Random.Range(0, neighbours.Count);
                    neighbours[neighbourIndex].gameObject.SetActive(true);
                }

                else if (fireHP <= FIRE_LOWER_BORDER)
                {
                    state = FireStates.PUTOUT;
                }
                break;
            case FireStates.PUTOUT:
                if (fireHP <= FIRE_LOWER_BORDER)
                {
                    fireMaterial.color = Color.blue;
                }
                break;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        //TODO: Potential for better performance when saving the Particle System
        int numCollisions = ParticlePhysicsExtensions.GetCollisionEvents(other.GetComponent<ParticleSystem>(), gameObject, new List<ParticleCollisionEvent>());  //ps.GetCollisionEvents(other, new List<ParticleCollisionEvent>());

        fireHP -= numCollisions * particleDamage;
    }

}
