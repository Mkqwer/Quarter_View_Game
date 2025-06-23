using UnityEngine;
using UnityEngine.Experimental.AI;


public class Card : MonoBehaviour
{
    public static Card instance;
    public string Name;
    public string Description;
    public System.Action<PlayerController> ApplyEffect;

    private void Awake()
    {
        instance = this;
    }
    public Card(string name, string description, System.Action<PlayerController> effect)
    {
        Name = name;
        Description = description;
        ApplyEffect = effect;
    }

    public void Use(PlayerController player)
    {
        ApplyEffect?.Invoke(player);
    }


    //카드 효과 개별적 

    /*
    // 예시: 체력 증가 카드
    Card healCard = new Card(
        "힐 카드",
        "플레이어의 체력을 20 회복합니다.",
        (player) => player.Heal(20)
    );


    */

    public static Card jumpCard = new Card(
        "점프 카드",
        "플레이어의 점프력 2배",
        (player) => player.jumpForce *= 2);


    

}