using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rigid;

    [SerializeField] float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rigid.AddForce(transform.forward * speed);
    }
}
