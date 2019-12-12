using UnityEngine;

public class TriggerPlayerVisible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.CompareTag("Player"))
            transform.parent.GetComponent<PlayerController>()?.SetPlayerSeen(other.GetComponent<PlayerController>(), true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (transform.parent.CompareTag("Player"))
            transform.parent.GetComponent<PlayerController>()?.SetPlayerSeen(other.GetComponent<PlayerController>(), false);
    }
}
