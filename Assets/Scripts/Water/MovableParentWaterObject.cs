using UnityEngine;

/// <summary>
/// Abstract base class for movable parent water objects like the collector or distributor
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class MovableParentWaterObject : WaterObjectController
{

    #region Serialized Fields

    /// <summary>
    /// The rigidbody of the object
    /// </summary>
    [SerializeField, Tooltip("The Rigidbody of this Object (if not set, it will be retrieved in the Start method)")]
    protected Rigidbody rigidBody = null;

    #endregion

    #region Protected Attributes

    /// <summary>
    /// Flag whether the transform should be updated in late update
    /// </summary>
    protected bool setTransform = false;

    /// <summary>
    /// Position the object when updated in late update
    /// </summary>
    protected Vector3 targetPosition;

    /// <summary>
    /// Rotation the object when updated in late update
    /// </summary>
    protected Quaternion targetRotation;

    #endregion

    #region MonoBehaviour implementation

    /// <summary>
    /// Start is called before the first frame update
    /// get the rigidbody when not assigned in the editor
    /// </summary>
    protected virtual void Start()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }

    /// <summary>
    /// Update the position of the object when connected to another object
    /// </summary>
    private void LateUpdate()
    {
        if (setTransform)
        {
            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Adjusts the transform so that the connection matches its target transform
    /// </summary>
    /// <param name="currentConnectionTransform"> the current transform of the connection </param>
    /// <param name="targetTransform"> the target transform of the connection </param>
    /// <param name="fixedConnection"> whether the connection is connected to a fixated object </param>
    /// <param name="isHose"> whether the connected object is a hose </param>
    public virtual void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection, bool isHose)
    {
        // set the flag that the transform should be updated in LateUpdate
        setTransform = true;

        // First Rotate the object to match the connected object
        Quaternion rotation = targetTransform.rotation * Quaternion.Inverse(currentConnectionTransform.rotation);
        transform.rotation = rotation * transform.rotation;
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

        // currentConnectionTransform is already updated at this point

        // Then move the object to match the connected object
        transform.position += targetTransform.position - currentConnectionTransform.position;     

        // set position and rotation for the LateUpdate method
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    /// <summary>
    /// Handles the disconnection of the object
    /// </summary>
    /// <param name="wasFixedConnection"> whether the cleared connection was a fixated connection </param>
    public abstract void OnClearConnection(bool wasFixedConnection);

    #endregion

}
