using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class MovableParentWaterObject : WaterObjectController
{
    [SerializeField]
    protected Rigidbody rigidBody = null;

    protected bool setTransform = false;

    protected Vector3 targetPosition;

    protected Quaternion targetRotation;

    protected virtual void Start()
    {
        if (!rigidBody)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }

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
        Vector3 movement = targetTransform.position - currentConnectionTransform.position;


        // TODO: Das Parent Object muss sich erst richtig drehen, dann neues movement berechnen und ausführen
        transform.position = transform.position + movement;
        transform.rotation = rotation * transform.rotation;
        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);

        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }


    public abstract void OnClearConnection(bool wasFixedConnection);

}
