using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Ammo, Coin, Grenade, Heart, Weapon
    }
    public Type ItemType;
    public int Value;


  
    void Update()
    {
        transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }
}
