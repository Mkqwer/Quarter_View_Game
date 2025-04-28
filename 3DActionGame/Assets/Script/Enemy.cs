using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C };
    public Type EnemyType;

    public int maxHealth;
    public int curHealth;
    public Transform Target;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public bool IsChase;
    public bool IsAttack;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material Mat;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        Mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        IsChase = true;
        anim.SetBool("IsWalk", true);
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(Target.position);
            nav.isStopped = !IsChase;
        }
    }

    void FreezeVelocity()
    {
        if (IsChase)
        {
            rigid.linearVelocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()
    {
        float targetRadius = 1.5f;
        float targetRange = 3f;

        switch (EnemyType)
        {
            case Type.A:
                targetRadius = 1.5f;
                targetRange = 3f;
                break;
            case Type.B:
                targetRadius = 1f;
                targetRange = 12f;
                break;
            case Type.C:
                targetRadius = 0.5f;
                targetRange = 25f;

                break;
        }

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !IsAttack) {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        IsChase = false;
        IsAttack = true;
        anim.SetBool("IsAttack", true);


        switch (EnemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f); 
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.linearVelocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f); 
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.linearVelocity = transform.forward * 20;

                yield return new WaitForSeconds(2f); 
                break;
        }
        IsChase = true;
        IsAttack = false;
        anim.SetBool("IsAttack", false);
    }
    void FixedUpdate()
    {
        Targerting();
        FreezeVelocity();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine (OnDamage(reactVec, true));
    }
    IEnumerator OnDamage(Vector3 reactVec, bool IsGrenade)
    {
        Mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        
        if(curHealth > 0)
        {
            Mat.color = Color.white;
        }
        else
        {
            Mat.color = Color.black;
            gameObject.layer = 14;
            IsChase = false;
            nav.enabled = false;
            anim.SetTrigger("DoDie");

            if (IsGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                rigid.freezeRotation = false; // 좌표고정 비활성화
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }


                Destroy(gameObject, 4);
        }
    }
}
