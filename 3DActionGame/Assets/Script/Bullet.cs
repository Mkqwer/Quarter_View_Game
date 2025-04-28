using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool IsMelee; 


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
      
    }
    private void OnTriggerEnter(Collider other)
    {
       if (!IsMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
