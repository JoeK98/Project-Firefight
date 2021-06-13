using UnityEngine;

/// <summary>
/// Controls the Jet pipe
/// <author> Joe Koelbel, Vincent Holtorf </author>
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

    /// <summary>
    /// The line renderer that is water
    /// </summary>
    [SerializeField]
    private LineRenderer lineRenderer = null;

    // TODO: REMOVE WHEN WATER PARTICLES ARE WORKING
    [SerializeField]
    private Renderer debugRenderer = null;

    /// <summary>
    /// How much hp the fire loses on a hit
    /// </summary>
    private int dmg = 2;

    /// <summary>
    /// Flag whether the pipe is open
    /// </summary>
    private bool isActivated = false;

    protected override void Update()
    {
        UpdateWaterPressure();

        //TODO: Only for testing purpose:
        if (isActivated && OutputWaterPressure > 0.5f)
        {
            lineRenderer.enabled = true;

            HitFire();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    protected override void UpdateWaterPressure()
    {
        InputWaterPressure = inputConnection.OutputWaterPressure;

        OutputWaterPressure = InputWaterPressure * JET_PIPE_MULTIPLIER;
    }

    /// <summary>
    /// Cast a ray and check if it hits a fire object
    /// If it hits remove hp from the fire
    /// </summary>
    private void HitFire()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        //Debug.DrawRay(origin, direction * 10f, Color.blue);
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, origin + direction * 10.0f);
        Ray ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 20, 1<<3))
        {
            FireRules frcollider = raycastHit.collider.GetComponent<FireRules>();

            if (frcollider != null)
            {
                frcollider.fireHP -= dmg;
            }
        }
    }

    /// <summary>
    /// Callback for the Activated Event of the XR Interactable
    /// </summary>
    public void OnActivate()
    {
        isActivated = true;
        debugRenderer.materials[2].color = Color.magenta;
    }

    /// <summary>
    /// Callback for the Deactivated Event of the XR Interactable
    /// </summary>
    public void OnDeactivate()
    {
        isActivated = false;
        debugRenderer.materials[2].color = Color.cyan;
    }
}
