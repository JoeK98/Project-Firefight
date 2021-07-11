using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control a single fire
/// <author> Vincent Holtorf, Joe Koelbel </author>
/// </summary>
public class FireController : MonoBehaviour
{

    #region Constants

    /// <summary>
    /// Error margin for min and max fire hp
    /// </summary>
    private const float ERROR_MARGIN = 1.0f;

    #endregion

    #region Serialized Fields

    [Header("Fire values")]

    /// <summary>
    /// Multiple "constants" saved in a scriptable object
    /// </summary>
    [SerializeField, Tooltip("Multiple 'constants' saved in a scriptable object")]
    private FireRulesSO fireRulesVariables = null;

    /// <summary>
    /// The neighbouring FireControllers
    /// </summary>
    [SerializeField, Tooltip("The neighbouring FireControllers")]
    private FireController[] neighbours = new FireController[0];

    /// <summary>
    /// The current FireState
    /// </summary>
    [SerializeField, Tooltip("The current FireState")]
    private FireStates state = FireStates.NONE;

    [Header("Sound")]

    /// <summary>
    /// The Sound controller of the fires
    /// </summary>
    [SerializeField, Tooltip("The Sound controller of the fires")]
    private FireSoundController fireSound;


    [Header("Particle Systems")]

    /// <summary>
    /// The smoke particle system
    /// </summary>
    [SerializeField, Tooltip("The smoke particle system")]
    private ParticleSystem smokeParticleSystem = null;

    /// <summary>
    /// The sparks particle system
    /// </summary>
    [SerializeField, Tooltip("The sparks particle system")]
    private ParticleSystem sparksParticleSystem = null;

    #endregion

    #region Private Attributes

    /// <summary>
    /// The emission module of the smoke particle system
    /// </summary>
    private ParticleSystem.EmissionModule smokeEmission;

    /// <summary>
    /// The emission module of the sparks particle system
    /// </summary>
    private ParticleSystem.EmissionModule sparksEmission;

    /// <summary>
    /// The current HP of this fire
    /// </summary>
    private float fireHP = 0.0f;

    /// <summary>
    /// The material of this fire
    /// </summary>
    private Material fireMaterial;

    /// <summary>
    /// The ID of the alpha value of the used Material
    /// </summary>
    private int alphaID;

    #endregion

    #region MonoBehaviour implementation

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    private void Awake()
    {
        // save the material of this fire
        fireMaterial = GetComponent<Renderer>().material;

        // save the id of the alpha property
        alphaID = Shader.PropertyToID("Vector1_4341703ff49e4488933ac95a1c03527f");

        // save the emission modules of the particle systems
        smokeEmission = smokeParticleSystem.emission;
        sparksEmission = sparksParticleSystem.emission;
    }

    /// <summary>
    /// OnEnable is called when the Object is enabling
    /// </summary>
    private void OnEnable()
    {
        // Set the state to ONFIRE
        state = FireStates.ONFIRE;

        // Set the fireHP to the lowest possible value
        fireHP = fireRulesVariables.fireLowerBorder + 0.001f;

        // Increase the amount of active fires
        fireSound.AddActiveFire();

        // update the visuals of the fire
        UpdateVisuals();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        switch (state)
        {
            case FireStates.NONE:
                gameObject.SetActive(false);
                break;
            case FireStates.ONFIRE:
                // if the current fire HP are in the viable range
                if (fireHP < fireRulesVariables.fireUpperBorder && fireHP >= fireRulesVariables.fireLowerBorder)
                {
                    // Add fire HP according to delta time
                    fireHP = Mathf.Clamp(fireHP + fireRulesVariables.fireMultiplicator * Time.deltaTime, fireRulesVariables.fireLowerBorder - ERROR_MARGIN, fireRulesVariables.fireUpperBorder + ERROR_MARGIN);
                    
                    // update the visuals of the fire
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

    /// <summary>
    /// OnParticleCollision is called when a particle hits a collider
    /// </summary>
    /// <param name="other"> the gameobject that holds the particle system</param>
    private void OnParticleCollision(GameObject other)
    {
        // The fire only needs to be put out when it is on fire
        if (state == FireStates.ONFIRE)
        {
            // Potential for better performance when saving the Particle System, but theoretically multiple particle systems could hit the object (e.g. one jetpipe in each hand)
            int numCollisions = ParticlePhysicsExtensions.GetCollisionEvents(other.GetComponent<ParticleSystem>(), gameObject, new List<ParticleCollisionEvent>());

            // Remove fire HP according to how many particles hit the collider
            fireHP = Mathf.Clamp(fireHP - numCollisions * fireRulesVariables.particleDamage, fireRulesVariables.fireLowerBorder - ERROR_MARGIN, fireRulesVariables.fireUpperBorder + ERROR_MARGIN);

            // if the fire HP reaches its lower border, it will be put out
            if (fireHP < fireRulesVariables.fireLowerBorder)
            {
                // set the state to PUTOUT
                state = FireStates.PUTOUT;

                // Decrease the amount of active fires
                fireSound.RemoveActiveFire();

                // update the fires visuals
                UpdateVisuals();

                // stop the particle systems
                smokeParticleSystem.Stop();
                sparksParticleSystem.Stop();
            }
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the visuals of the fire
    /// </summary>
    private void UpdateVisuals()
    {
        // calculate the multiplier (also the alpha value of the material) for the particles
        float multiplier = fireHP.Map(fireRulesVariables.fireLowerBorder, fireRulesVariables.fireUpperBorder, 0.0f, 1.0f);

        // Set the alpha value of the material
        fireMaterial.SetFloat(alphaID, multiplier);

        // Set the emission of both particle systems
        smokeEmission.rateOverTime = new ParticleSystem.MinMaxCurve(multiplier * fireRulesVariables.maxSmokeParticles);
        sparksEmission.rateOverTime = new ParticleSystem.MinMaxCurve(multiplier * fireRulesVariables.maxSparkParticles);
    }

    #endregion

}
