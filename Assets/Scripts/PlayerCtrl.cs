using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private Transform playerTrs;
    private Animator anim;
    private Camera mainCam;
    private Rigidbody rigid;
    private BoxCollider box;

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    private Plane plane;
    private Ray ray;
    private Vector3 hitPoint;

    [SerializeField] bool isGround;

    private void Awake()
    {
        mainCam = Camera.main;
        playerTrs = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        move();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void move()
    {
        Vector2 keyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));//Ű�����Է°�

        Vector3 moveDir = (Vector3.forward * keyInput.y) + (Vector3.right * keyInput.x);//�����¿����

        playerTrs.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);//�����¿������

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void lookAround()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));//���콺�Է°�

        playerTrs.Rotate(Vector3.up * turnSpeed * Time.deltaTime * mouseInput.x);

    }

    private void checkGround()
    {
        if (Physics.Raycast(box.bounds.center, Vector3.down,
               box.size.y * 0.6f, LayerMask.GetMask("Ground")))//�� Ȯ��
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }
}


