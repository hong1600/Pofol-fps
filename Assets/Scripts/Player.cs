using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Camera mainCam;
    private CapsuleCollider cap;
    private Rigidbody rigid;
    private Slider hpBar;

    [SerializeField] private Transform cameraTrs;
    [SerializeField] private Transform playerTrs;
    [SerializeField] private Transform bulletTrs;
    [SerializeField] private Transform displayUI;

    private Vector3 moveDir;
    private float gravity = 9.81f;
    private float verticalVelocity;
    private bool isJump = false;
    private bool isRun = false;
    private bool aim;
    private bool isControll = true;
    private bool noHit;
    private bool isDie = false;
    public bool hasWeapon = false;
    

    [SerializeField] private bool isGround;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float runSpeed;

    [SerializeField] GameObject weapon1;
    [SerializeField] GameObject weapon2;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject hitEffect;

    [SerializeField] private float curHp = 100;
    private float maxHp = 100;
    private static int damage = 5;
    

    private void Awake()
    {
        hpBar = displayUI.GetComponent<Slider>();
        mainCam = Camera.main;
        cap = playerTrs.GetComponent<CapsuleCollider>();
        rigid = playerTrs.GetComponent<Rigidbody>();
        anim = playerTrs.GetComponent<Animator>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        hpBar.value = 100;
    }

    void Update()
    {
        move();
        lookAround();
        checkGround();
        jump();
        checkMouseLock();
        swap();
        shoot();
        aiming();
        hpUI();
    }

    private void move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));//Ű�����Է°�
        bool isMove = moveInput.magnitude != 0f;//�Է°� Ȯ��
        if(isMove && isControll == true) 
        {
            Vector3 lookForward =  new Vector3(cameraTrs.forward.x, 0f, cameraTrs.forward.z).normalized;//�������
            Vector3 lookRight = new Vector3(cameraTrs.right.x, 0f, cameraTrs.right.z).normalized;//�����ʹ���
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;//�̵� �� ����

            playerTrs.forward = lookForward;

            transform.position += moveDir * Time.deltaTime * moveSpeed;//�̵��ӵ�
        }

        if (Input.GetKey(KeyCode.LeftShift) && isMove)//�⺻ �޸���
        {
            isRun = true;
            anim.SetBool("isRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            anim.SetBool("isRun", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && hasWeapon == true && isMove)//���� �޸���
        {
            anim.SetBool("isRifleRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("isRifleRun", false);
        }

        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    private void lookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraTrs.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraTrs.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

    }

    private void checkGround()
    {
        if (rigid.velocity.y <= 0)//�� Ȯ��
        {
            isGround = Physics.Raycast(cap.bounds.center, Vector3.down,
                cap.height * 0.55f, LayerMask.GetMask("Ground"));
        }
        else 
        {
            isGround = false;
        }
    }

    private void jump()
    {
        if (isGround == true)//���� ������
        {
            isJump = false;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJump == false && hasWeapon == false)//�⺻ ����
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("doJump");
            isJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isJump == false && hasWeapon == true)//������ ����
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("isRifleJump");
            isJump = true;
        }
        else if (isJump == true && isGround == false) //���߿� ������
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        rigid.velocity = new Vector3(rigid.velocity.x, verticalVelocity, rigid.velocity.z);
    }

    private void checkMouseLock()
    {
        if (Input.GetKeyDown(KeyCode.Tab))//���� ������
        {
            if (Cursor.lockState == CursorLockMode.Locked)//Ŀ�� ��� ������
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else//Ŀ�� ����� ������
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }


    public void swap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))//1�� ����
        {
            hasWeapon = true;
            anim.SetBool("hasWeapon", true);
        }
        else if (Input.GetKeyDown(KeyCode.X))//���� ���� ����
        {
            hasWeapon = false;
            anim.SetBool("hasWeapon", false);
        }
    }

    private void aiming()
    {
        if (Input.GetMouseButton(1) && hasWeapon == true)
        {
            anim.SetBool("isAim", true);
        }
        if ((Input.GetMouseButtonUp(1) && hasWeapon == true))
        {
            anim.SetBool("isAim", false);
        }
    }

    private void shoot()
    {
        if (Input.GetMouseButtonDown(0) && hasWeapon == true)
        {
            if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, 100f))
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Instantiate(bullet, bulletTrs.position, transform.rotation);
            }
            anim.SetBool("isFire", true);
        }
        if (Input.GetMouseButtonUp(0) && hasWeapon == true)
        {
            anim.SetBool("isFire", false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAttack") && curHp > 0 && noHit == false)
        {
            StartCoroutine(Hit());
        }
        else if (other.CompareTag("EnemyAttack") && curHp < 1 && noHit == false && isDie == false)
        {
            isControll = false;
            anim.SetTrigger("isDie");
            isDie = true;
            GameManager.instance.IsGameOver = true;
        }
    }

    IEnumerator Hit()
    {
        curHp -= 5;
        noHit = true;

        yield return new WaitForSeconds(2);

        noHit = false;
    }

    private void hpUI()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            curHp -= 10f;
        }
        hpBar.value = curHp / maxHp;
    }
}
