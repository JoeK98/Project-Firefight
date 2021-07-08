using UnityEngine;

public class SuctionBasketInput : MonoBehaviour
{
    [SerializeField]
    private SuctionBasketController suctionBasket;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            suctionBasket.SetConnectionToLake(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            suctionBasket.SetConnectionToLake(false);
        }
    }
}
