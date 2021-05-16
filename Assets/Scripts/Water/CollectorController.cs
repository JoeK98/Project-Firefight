using System.Collections.Generic;
using UnityEngine;

public class CollectorController : WaterObjectController
{

    private void Update()
    {
        UpdateWaterPressure();
    }

    protected override void UpdateWaterPressure()
    {
        throw new System.NotImplementedException();
    }
}
