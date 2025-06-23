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


    //ī�� ȿ�� ������ 

    /*
    // ����: ü�� ���� ī��
    Card healCard = new Card(
        "�� ī��",
        "�÷��̾��� ü���� 20 ȸ���մϴ�.",
        (player) => player.Heal(20)
    );


    */

    public static Card jumpCard = new Card(
        "���� ī��",
        "�÷��̾��� ������ 2��",
        (player) => player.jumpForce *= 2);


    

}