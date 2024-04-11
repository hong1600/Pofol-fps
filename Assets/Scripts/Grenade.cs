using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    Rigidbody rigid;
    SphereCollider col;

    [SerializeField] float speed;

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.AddForce(transform.forward * speed);
    }
}
