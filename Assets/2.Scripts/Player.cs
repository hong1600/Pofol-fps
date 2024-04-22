using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Camera mainCam;
    private Rigidbody rigid;
    private BoxCollider box;

    [SerializeField] Transform player;
    [SerializeField] Transform cameraTrs;
    [SerializeField] Transform playerTrs;
    [SerializeField] Transform bulletTrs;
    [SerializeField] Transform displayUI;

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
    private bool inventoryon = false;
    private bool isPause = false;

    Vector2 mouse;

    [SerializeField] bool isGround;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float runSpeed;

    [SerializeField] GameObject weapon1;
    [SerializeField] GameObject weapon2;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject grenade;
    [SerializeField] GameObject inventoryUI;
    [SerializeField] GameObject muzzleFlash;

    [SerializeField] private float curHp = 100;
    private float maxHp = 100;
    private Slider hpBar;
    [SerializeField]private float haveCoin = 0; 
    

    private void Awake()
    {
        box = playerTrs.GetComponent<BoxCollider>();
        hpBar = displayUI.GetComponent<Slider>();
        mainCam = Camera.main;
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
        swap();
        shoot();
        aiming();
        hpUI();
        inventory();
    }

    private void move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));//키보드입력값
        //bool isMove = moveInput.magnitude != 0f;//입력값 확인
        if(isControll == true) 
        {
            Vector3 lookForward =  new Vector3(cameraTrs.forward.x, 0f, cameraTrs.forward.z).normalized;//정면방향
            Vector3 lookRight = new Vector3(cameraTrs.right.x, 0f, cameraTrs.right.z).normalized;//오른쪽방향
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;//이동 할 방향

            playerTrs.forward = lookForward;

            transform.position += moveDir * Time.deltaTime * moveSpeed;//이동속도
        }

        if (Input.GetKey(KeyCode.LeftShift))//기본 달리기 
        {
            anim.SetBool("isRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("isRun", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && hasWeapon == true)//무기 달리기
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
        mouse.x += Input.GetAxis("Mouse X");
        mouse.y += Input.GetAxis("Mouse Y");

        float x = mouse.y;

        if (x > 0)
        {
            Mathf.Clamp(x, -1f, 45f);
        }
        else if (x < 0)
        {
            Mathf.Clamp(x, 1f, -45f);
        }

        transform.localRotation = Quaternion.Euler(-x, mouse.x, 0);
    }

    private void checkGround()
    {
        if (rigid.velocity.y <= 0)//땅 확인
        {
            isGround = Physics.Raycast(box.bounds.center, Vector3.down,
               box.size.y * 0.55f, LayerMask.GetMask("Ground"));
        }
        else 
        {
            isGround = false;
        }
    }

    private void jump()
    {
        if (isGround == true)//땅에 있을때
        {
            isJump = false;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isJump == false && hasWeapon == false)//기본 점프
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("doJump");
            isJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && isJump == false && hasWeapon == true)//라이플 점프
        {
            verticalVelocity = jumpForce;
            anim.SetTrigger("isRifleJump");
            isJump = true;
        }
        else if (isJump == true && isGround == false) //공중에 있을때
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        rigid.velocity = new Vector3(rigid.velocity.x, verticalVelocity, rigid.velocity.z);
    }


    public void swap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))//1번 무기
        {
            hasWeapon = true;
            anim.SetBool("hasWeapon", true);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hasWeapon = true;
            anim.SetBool("isPistolAim", true) ;
        }

        else if (Input.GetKeyDown(KeyCode.X))//무기 장착 해제
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
            StartCoroutine(shootbullet());
        }

        if (Input.GetKeyDown(KeyCode.F) && hasWeapon == true)
        {
            StartCoroutine (shootgrenade());
        }
    }
    IEnumerator shootbullet()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, 1000f))
        {
            Instantiate(bullet, bulletTrs.position, bulletTrs.rotation);
            GameObject flash = Instantiate(muzzleFlash, bulletTrs.position, bulletTrs.rotation);

            Destroy(flash, 0.1f);

            anim.SetBool("isFire", true);
        }
        yield return new WaitForSeconds(0.5f);

        anim.SetBool("isFire", false);
    }

    IEnumerator shootgrenade()
    {
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out RaycastHit hit, 100f))
        {
            Instantiate(grenade, bulletTrs.position, bulletTrs.rotation);
            anim.SetBool("isFire", true);
        }

        yield return null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack") && curHp > 0 && noHit == false)
        {
            StartCoroutine(Hit());
        }

        else if (other.CompareTag("Attack") && curHp < 1 && noHit == false && isDie == false)
        {
            isControll = false;
            anim.SetTrigger("isDie");
            isDie = true;
            GameManager.instance.IsGameOver = true;
        }

        if (other.CompareTag("Coin"))
        {
            haveCoin += 10;

            Destroy(other.gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("Coin"))
        {
            haveCoin += 100f;
            Destroy(gameObject.gameObject);
        }
    }
    

    IEnumerator Hit()
    {
        curHp -= 20;
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

    private void inventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && inventoryon == false)
        {
            inventoryUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            inventoryon = true;

            if (isPause == false)
            {
                Time.timeScale = 0f;
                isPause = true;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Tab) && inventoryon == true)
        {
            inventoryUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            inventoryon = false;

            if (isPause == true)
            {
                Time.timeScale = 1f;
                isPause = false;
            }
        }
    }
}
