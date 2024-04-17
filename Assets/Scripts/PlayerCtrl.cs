using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    private Transform playerTrs;
    private Animator anim;

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    private void Awake()
    {
        playerTrs = GetComponent<Transform>();
        anim = GetComponent<Animator>();
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
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));//���콺�Է°�

        Vector3 moveDir = (Vector3.forward * keyInput.y) + (Vector3.right * keyInput.x);//�����¿����

        playerTrs.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);//�����¿������

        playerTrs.Rotate(Vector3.up.normalized * turnSpeed * Time.deltaTime * mouseInput.x);//x�� ȸ��

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));

    }

}
