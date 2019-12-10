using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerColliderController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Hiders") && gameObject.GetComponent<PlayerController>().IsSeeker)
        {
            collision.gameObject.GetComponent<PlayerController>().IsSeeker = true;
        }
    }
}
