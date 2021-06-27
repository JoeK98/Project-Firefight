using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;

public class FireRules : MonoBehaviour
{
    private const float ERROR_MARGIN = 1.0f;

    [SerializeField]
    private FireRulesSO fireRulesVariables = null;

    [SerializeField]
    private List<FireRules> neighbours = new List<FireRules>();

    [SerializeField]
    private FireStates state = FireStates.NONE;

    private float fireHP = 0.0f;
    private Material fireMaterial;
    private int alphaID;

    // Start is called before the first frame update
    private void Start()
    {
        fireMaterial = GetComponent<Renderer>().material;
        alphaID = Shader.PropertyToID("Vector1_4341703ff49e4488933ac95a1c03527f");
    }

    // Update is called once per frame
    private void Update()
    {
        CheckState();
    }

    private void OnEnable()
    {
        state = FireStates.ONFIRE;
        fireHP = fireRulesVariables.fireLowerBorder + 0.001f;
        SetFireAlpha();
    }

    private void CheckState()
    {
        switch (state)
        {
            case FireStates.NONE:
                gameObject.SetActive(false);
                break;
            case FireStates.ONFIRE:
                if (fireHP < fireRulesVariables.fireUpperBorder && fireHP >= fireRulesVariables.fireLowerBorder)
                {
                    fireHP = Mathf.Clamp(fireHP + fireRulesVariables.fireMultiplicator * Time.deltaTime, fireRulesVariables.fireLowerBorder, fireRulesVariables.fireUpperBorder + ERROR_MARGIN);
                    SetFireAlpha();
                    //fireMaterial.color = new Color(fireHP.Map(fireRulesVariables.fireLowerBorder, fireRulesVariables.fireUpperBorder, 0.0f, 1.0f), 0.0f, 0.0f);
                }

                else if (fireHP >= fireRulesVariables.fireUpperBorder)
                {
                    int neighbourIndex = Random.Range(0, neighbours.Count);
                    neighbours[neighbourIndex].gameObject.SetActive(true);
                }

                else if (fireHP <= fireRulesVariables.fireLowerBorder)
                {
                    state = FireStates.PUTOUT;
                }
                break;
            case FireStates.PUTOUT:
                if (fireHP <= fireRulesVariables.fireLowerBorder)
                {
                    fireMaterial.color = Color.blue;
                }
                break;
        }
    }

    private void SetFireAlpha()
    {
        fireMaterial.SetFloat(alphaID, fireHP.Map(fireRulesVariables.fireLowerBorder, fireRulesVariables.fireUpperBorder, 0.0f, 1.0f));
    }

    private void OnParticleCollision(GameObject other)
    {
        //TODO: Potential for better performance when saving the Particle System
        int numCollisions = ParticlePhysicsExtensions.GetCollisionEvents(other.GetComponent<ParticleSystem>(), gameObject, new List<ParticleCollisionEvent>());  //ps.GetCollisionEvents(other, new List<ParticleCollisionEvent>());

        fireHP = Mathf.Clamp(fireHP - numCollisions * fireRulesVariables.particleDamage, fireRulesVariables.fireLowerBorder - ERROR_MARGIN, fireRulesVariables.fireUpperBorder);
    }

}
