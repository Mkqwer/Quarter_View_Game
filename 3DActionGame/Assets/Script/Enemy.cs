using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int maxHealth;
    public int curHealth;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Material Mat;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        Mat = GetComponent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
        else if (other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec));
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
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

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;

            rigid.AddForce(reactVec * 5,ForceMode.Impulse);

            Destroy(gameObject, 4);
        }
    }
}
