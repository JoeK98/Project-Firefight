using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control a single fire
/// <author> Vincent Holtorf, Joe Koelbel </author>
/// </summary>
public class FireController : MonoBehaviour
{
    private const float ERROR_MARGIN = 1.0f;

    [SerializeField]
    private FireRulesSO fireRulesVariables = null;

    [SerializeField]
    private List<FireController> neighbours = new List<FireController>();

    [SerializeField]
    private FireStates state = FireStates.NONE;

    [SerializeField]
    private ParticleSystem smokeParticleSystem = null;

    [SerializeField]
    private ParticleSystem sparksParticleSystem = null;

    private ParticleSystem.EmissionModule smokeEmission;

    private ParticleSystem.EmissionModule sparksEmission;

    private float fireHP = 0.0f;

    private Material fireMaterial;

    private int alphaID;

    public FireSound fireSound;

    // Start is called before the first frame update
    private void Awake()
    {
        fireMaterial = GetComponent<Renderer>().material;
        alphaID = Shader.PropertyToID("Vector1_4341703ff49e4488933ac95a1c03527f");

        smokeEmission = smokeParticleSystem.emission;
        sparksEmission = sparksParticleSystem.emission;
    }

    private void OnEnable()
    {
        state = FireStates.ONFIRE;
        fireHP = fireRulesVariables.fireLowerBorder + 0.001f;
        fireSound.AddActiveFire();
        UpdateVisuals();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case FireStates.NONE:
                gameObject.SetActive(false);
                break;
            case FireStates.ONFIRE:
                if (fireHP < fireRulesVariables.fireUpperBorder && fireHP >= fireRulesVariables.fireLowerBorder)
                {
                    fireHP = Mathf.Clamp(fireHP + fireRulesVariables.fireMultiplicator * Time.deltaTime, fireRulesVariables.fireLowerBorder - ERROR_MARGIN, fireRulesVariables.fireUpperBorder + ERROR_MARGIN);
                    UpdateVisuals();
                }

                // This will enable all neighbours in one frame (might enable a random one and apply a cooldown to enable the next neighbour)
                else if (fireHP >= fireRulesVariables.fireUpperBorder)
                {
                    foreach (FireController neighbour in neighbours)
                    {
                        neighbour.gameObject.SetActive(true);
                    }
                }
                break;
            default:
                break;
        }
    }

    private void UpdateVisuals()
    {
        float multiplier = fireHP.Map(fireRulesVariables.fireLowerBorder, fireRulesVariables.fireUpperBorder, 0.0f, 1.0f);

        fireMaterial.SetFloat(alphaID, multiplier);
        smokeEmission.rateOverTime = new ParticleSystem.MinMaxCurve(multiplier * fireRulesVariables.maxSmokeParticles);
        sparksEmission.rateOverTime = new ParticleSystem.MinMaxCurve(multiplier * fireRulesVariables.maxSparkParticles);
    }

    private void OnParticleCollision(GameObject other)
    {
        // The fire only needs to be put out when it is on fire
        if (state == FireStates.ONFIRE)
        {
            // Potential for better performance when saving the Particle System, but theoretically multiple particle systems could hit the object
            int numCollisions = ParticlePhysicsExtensions.GetCollisionEvents(other.GetComponent<ParticleSystem>(), gameObject, new List<ParticleCollisionEvent>());

            fireHP = Mathf.Clamp(fireHP - numCollisions * fireRulesVariables.particleDamage, fireRulesVariables.fireLowerBorder - ERROR_MARGIN, fireRulesVariables.fireUpperBorder + ERROR_MARGIN);

            if (fireHP < fireRulesVariables.fireLowerBorder)
            {
                state = FireStates.PUTOUT;
                fireSound.RemoveActiveFire();
                UpdateVisuals();
                smokeParticleSystem.Stop();
                sparksParticleSystem.Stop();
            }
        }
    }

}
