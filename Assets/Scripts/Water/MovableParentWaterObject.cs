using UnityEngine;

/// <summary>
/// Abstract base class for movable parent water objects like the collector or distributor
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class MovableParentWaterObject : WaterObjectController
{
    /// <summary>
    /// The rigidbody of the object
    /// </summary>
    [SerializeField]
    protected Rigidbody rigidBody = null;

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

    public virtual void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection, bool isHose)
    {
        setTransform = true;

        Quaternion rotation = targetTransform.rotation * Quaternion.Inverse(currentConnectionTransform.rotation);
        transform.rotation = rotation * transform.rotation;
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

        // TODO: is currentConnectionTransform already updated at this point?
        transform.position += targetTransform.position - currentConnectionTransform.position;     

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }


    public abstract void OnClearConnection(bool wasFixedConnection);

}
