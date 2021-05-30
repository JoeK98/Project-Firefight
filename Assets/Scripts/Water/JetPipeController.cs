using UnityEngine;

/// <summary>
/// Controls the Jet pipe
/// <author> Joe Koelbel </author>
/// </summary>
public class JetPipeController : WaterObjectController
{
    /// <summary>
    /// Jet pipe increases pressure (we use therefor a multiplier)
    /// </summary>
    private const float JET_PIPE_MULTIPLIER = 1.5f;

    /// <summary>
    /// The input connection of the jet pipe
    /// </summary>
    [SerializeField]
    private ConnectionController inputConnection = null;

    [SerializeField]
    private LineRenderer lineRenderer = null;

    // TODO: REMOVE WHEN WATER PARTICLES ARE WORKING
    [SerializeField]
    private Renderer debugRenderer = null;
    protected override void Update()
    {
        UpdateWaterPressure();

        //TODO: Only for testing purpose:
        if (OutputWaterPressure > 0.5f)
        {
            

            debugRenderer.materials[2].color = Color.green;

            HitFire();
        }
    }

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        OutputWaterPressure = InputWaterPressure * JET_PIPE_MULTIPLIER;
    }

    private void HitFire()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        //Debug.DrawRay(origin, direction * 10f, Color.blue);
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + direction * 10.0f);
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit raycastHit) && raycastHit.collider.CompareTag("Fire"))
        {
            raycastHit.collider.GetComponent<FireRules>().fireHP -= 2;
        }
    }
}
