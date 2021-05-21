using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoseConnectionController : ConnectionController
{
    // TODO: REMOVE DEBUG

    private Rigidbody rigidBody = null;

    // Start is called before the first frame update
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //DEBUG("OnTriggerEnter");
        if (connectedObject == null && other.CompareTag("Connection"))
        {
            //DEBUG("EnteredConnection");
            // Cast to HoseConnectionController to access connectedObject
            ConnectionController connection = other.GetComponent<ConnectionController>();
            if (connection.CheckOnTriggerEnter(this, connectionSize))
            //if (connection.connectedObject == null || connection.connectedObject == this)
            {
                //DEBUG("BUILDING CONNECTION");
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
