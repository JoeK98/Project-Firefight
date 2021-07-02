using UnityEngine;

public abstract class ParentWaterObject : WaterObjectController
{
    public virtual void AdjustTransformOnConnection(Transform currentConnectionTransform, Transform targetTransform, bool fixedConnection)
    {
        // For some Objects nothing happens
    }


    public virtual void OnClearConnection(bool wasFixedConnection)
    {
        // For some Objects nothing happens
    }
}
