using UnityEngine;

public class TriggerPlayerVisible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
            transform.parent.GetComponent<PlayerController>()?.SetPlayerSeen(other.transform.parent.GetComponent<PlayerController>(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.CompareTag("Player"))
            transform.parent.GetComponent<PlayerController>()?.SetPlayerSeen(other.transform.parent.GetComponent<PlayerController>(), false);
    }
}
