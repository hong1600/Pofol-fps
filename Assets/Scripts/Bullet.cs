using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    Rigidbody rigid;

    [SerializeField] float damage;
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
