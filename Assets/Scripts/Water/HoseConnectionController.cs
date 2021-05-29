using UnityEngine;

/// <summary>
/// Controls the connections of a hose
/// <author> Joe Koelbel </author>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HoseConnectionController : ConnectionController
{
    /// <summary>
    /// For Hoses the input and output connections arent initially defined
    /// which means that this value needs to be changed accordingly
    /// </summary>
    public bool WaterPressureViaConnection { set { waterPressureViaConnection = value; } }

    /// <summary>
    /// A hose is currently only grabbable at its connections (TODO: check later if this is still true)
    /// This means the connection needs a rigidbody
    /// </summary>
    private Rigidbody rigidBody = null;

    /// <summary>
    /// Start is called before the first frame update
    /// Search for the Rigidbody
    /// </summary>
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Check if a connection can be build with the colliding object
    /// </summary>
    /// <param name="other"> the other collider </param>
    private void OnTriggerEnter(Collider other)
    {
        if (connectedObject == null && other.CompareTag("Connection"))
        {
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if (connection.CheckOnTriggerEnter(this))
            {
                connectedObject = connection;
                transform.position = connectedObject.transform.position;
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    public override void ClearConnection()
    {
        if (!isClearing && connectedObject != null)
        {
            rigidBody.constraints = RigidbodyConstraints.None;
        }
        isClearing = false;
    }
}
