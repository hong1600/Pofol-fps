using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    BoxCollider box;
    NavMeshAgent nav;
    [SerializeField] GameObject attackBox;
    [SerializeField] GameObject coin;

    [SerializeField] Transform enemyTrs;
    [SerializeField] Transform playerTrs;
    [SerializeField] Transform spawnPoint;
    
    public bool isGround = false;
    private float verticalVelocity;
    private float gravity = 9.81f;
    private bool isChase;
    private bool isAttack;
    private bool hitReady;

    float maxHp = 10f;
    [SerializeField] float curHp = 10f;
    [SerializeField] float damage = 1f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        box = GetComponent<BoxCollider>();  
    }

    private void Start()
    {

    }

    void Update()
    {
        move();
        checkGround();
        checkGravity();
        chaseStart();
        tracking();
        targeting();
    }

    private void move()
    {

    }

    private void checkGround()
    {
        if (verticalVelocity <= 0)
        {
            isGround = Physics.Raycast(box.bounds.center, Vector3.down,
                box.size.y * 0.55f, LayerMask.GetMask("Ground"));
        }
        else isGround = false;
    }

    private void checkGravity()
    {
        if(isGround == true) 
        {
            verticalVelocity = 0;
        }
        else if (isGround == false) 
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        rigid.velocity = new Vector3(rigid.velocity.x, verticalVelocity, rigid.velocity.z);
    }


    private void chaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    private void tracking()
    {
        if (nav.enabled && isChase == true) 
        {
            nav.SetDestination(playerTrs.position);
            nav.isStopped = !isChase;
        }
    }

    private void targeting()
    {
        float targetRange = 2f;

        if (Physics.Raycast(transform.position, transform.forward, targetRange, LayerMask.GetMask("Player")))
        {
            hitReady = true;
        }

        if (hitReady == true && !isAttack)
        {
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        attackBox.SetActive(true);
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(2f);

        attackBox.SetActive(false);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }


    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("PlayerBullet") && curHp > 0f)
        {
            StartCoroutine(Hit());
        }
        else if (coll.collider.gameObject.CompareTag("PlayerBullet") && curHp < 1f)
        {
            StartCoroutine(Die());
        }

    }

    IEnumerator Hit()
    {
        curHp -= 2f;
        yield return null;
    }

    IEnumerator Die()
    {
        anim.SetTrigger("isDie");
        isChase = false;
        hitReady = false;
        nav.enabled = false;

        Instantiate(coin, gameObject.transform.position * 10f, Quaternion.identity);

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
