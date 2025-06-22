using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 20;

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"[Enemy] {gameObject.name} 남은 체력: {hp}");

        if (hp <= 0)
            Destroy(gameObject);
    }
}
