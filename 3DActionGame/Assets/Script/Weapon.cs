using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{ 
    public enum Type { Melee, Range };
    public Type type;
    public float Rate;
    public int Damage;
    public int maxAmmo;
    public int CurAmmo;

    public BoxCollider MeleeArea;
    public TrailRenderer TrailEffect;
    public Transform BulletPos;
    public GameObject Bullet;
    public Transform BulletCasePos;
    public GameObject BulletCase;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Range && CurAmmo > 0)
        {
            CurAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        MeleeArea.enabled = true;
        TrailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        MeleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        TrailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // √—æÀ πﬂªÁ
        GameObject intantBullet = Instantiate(Bullet, BulletPos.position, BulletPos.rotation);
        Rigidbody BulletRigid = intantBullet.GetComponent<Rigidbody>();
        BulletRigid.linearVelocity = BulletPos.forward * 50;

        yield return null;

        //≈∫«« πË√‚
        GameObject IntantCase = Instantiate(BulletCase, BulletCasePos.position, BulletCasePos.rotation);
        Rigidbody CaseRigid = IntantCase.GetComponent<Rigidbody>();
        Vector3 CaseVec = BulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        CaseRigid.AddForce(CaseVec, ForceMode.Impulse);
        CaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);

    }

}


