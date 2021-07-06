using UnityEngine;

/// <summary>
/// GameObject that sets initial values that are not available in the project settings
/// (Currently disabling collisions between connections, "default" objects and opener
/// <author> Joe Koelbel </author>
/// </summary>
public class SetupManager : MonoBehaviour
{
    /// <summary>
    /// The default collision layer
    /// </summary>
    [Tooltip("The default collision layer")]
    [SerializeField]
    private int collisionLayer1 = 0;

    /// <summary>
    /// The connection collision layer
    /// </summary>
    [Tooltip("The connection collision layer")]
    [SerializeField]
    private int collisionLayer2 = 1;

    /// <summary>
    /// The opener collision layer
    /// </summary>
    [Tooltip("The opener collision layer")]
    [SerializeField]
    private int collisionLayer3 = 2;

    /// <summary>
    /// Awake is called before the Start Method
    /// </summary>
    private void Awake()
    {
        // Disable collisions between default and connection colliders
        Physics.IgnoreLayerCollision(collisionLayer1, collisionLayer2);
        // Disable collisions between default and opener colliders
        Physics.IgnoreLayerCollision(collisionLayer1, collisionLayer3);
        // Disable collisions between connection and opener colliders
        Physics.IgnoreLayerCollision(collisionLayer2, collisionLayer3);

        // TODO: Set display refresh rate
        if (!Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(120.0f))
        {
            // This should only happen if the exoerimental setting for 120Hz support is turned off in the Quest 2 settings
            if (!Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(90.0f))
            {
                // Usually this should not happen when playing on Quest 2 since 90Hz should always be available
                Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(80.0f);
            }
        }
    }
}
