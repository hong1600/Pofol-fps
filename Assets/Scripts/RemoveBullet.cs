using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    [SerializeField] GameObject bulletEffect;


    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("PlayerBullet"))
        {
            ContactPoint cp = coll.GetContact(0);
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            GameObject bulleteffect = Instantiate(bulletEffect, cp.point, rot);

            Destroy(bulleteffect, 0.5f);
            Destroy(coll.gameObject);
        }

        if (coll.gameObject.CompareTag("PlayerGrenade"))
        {
            Destroy(coll.gameObject);
        }

    }
}
