using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool IsMelee; 
    public bool IsRock; 


    private void OnCollisionEnter(Collision collision)
    {
        if (!IsRock  && collision.gameObject.tag == "Floor")
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
