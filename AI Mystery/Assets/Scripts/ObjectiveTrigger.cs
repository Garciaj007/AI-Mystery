using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponentInParent<PlayerController>();
            if (player.IsSeeker) player.IsSeeker = false;
            GameManager.Instance.HideObjective();
        }
    }
}
