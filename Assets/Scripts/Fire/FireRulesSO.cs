using UnityEngine;

/// <summary>
/// Scriptable Object that saves shared values for the fire rules
/// <author> Joe Koelbel </author>
/// </summary>
[CreateAssetMenu]
public class FireRulesSO : ScriptableObject
{
    /// <summary>
    /// How much HP a single Particle removes on hit
    /// </summary>
    [Tooltip("How much HP a single Particle removes on hit")]
    public float particleDamage;

    /// <summary>
    /// Maximal fire HP when neighbouring fires will be ignited
    /// </summary>
    [Tooltip("Maximal fire HP when neighbouring fires will be ignited")]
    public float fireUpperBorder;

    /// <summary>
    /// Minimal fire HP when a fire is put out
    /// </summary>
    [Tooltip("Minimal fire HP when a fire is put out")]
    public float fireLowerBorder;

    /// <summary>
    /// How many HP a fire gains per second
    /// </summary>
    [Tooltip("How many HP a fire gains per second")]
    public float fireMultiplicator;

    /// <summary>
    /// Maximal RateOverTime value of smoke particle system
    /// </summary>
    [Tooltip("Maximal RateOverTime value of smoke particle system")]
    public int maxSmokeParticles;

    /// <summary>
    /// Maximal RateOverTime value of spark particle system
    /// </summary>
    [Tooltip("Maximal RateOverTime value of spark particle system")]
    public int maxSparkParticles;
}
