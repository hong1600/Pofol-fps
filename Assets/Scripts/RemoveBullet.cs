using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("PlayerBullet") && collision.collider.CompareTag("PlayerGrenade"))
        {
            Destroy(collision.gameObject);
        }
    }
}
