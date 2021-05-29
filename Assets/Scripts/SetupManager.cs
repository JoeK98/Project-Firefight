using UnityEngine;

/// <summary>
/// GameObject that sets initial values that are not available in the project settings
/// (Currently disabling collisions between connections and "default" objects
/// <author> Joe Koelbel </author>
/// </summary>
public class SetupManager : MonoBehaviour
{
    /// <summary>
    /// The default collision layer
    /// </summary>
    [SerializeField]
    private int collisionLayer1 = 0;

    /// <summary>
    /// The connection collision layer
    /// </summary>
    [SerializeField]
    private int collisionLayer2 = 1;

    /// <summary>
    /// Awake is called before the Start Method
    /// </summary>
    private void Awake()
    {
        // Disable collisions between default and connection colliders
        Physics.IgnoreLayerCollision(collisionLayer1, collisionLayer2);
    }
}
