using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Ammo, Coin, Grenade, Heart, Weapon
    }
    public Type ItemType;
    public int Value;


    Rigidbody Rigid;
    SphereCollider SphereCollider;

    void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        SphereCollider = GetComponent<SphereCollider>();
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Rigid.isKinematic = true;
            SphereCollider.enabled = false;
        }
    }
}
