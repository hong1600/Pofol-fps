using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    [SerializeField] GameObject bulletEffect;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("PlayerGrenade"))
        {
            Destroy(collision.gameObject);
        }

    }
}
