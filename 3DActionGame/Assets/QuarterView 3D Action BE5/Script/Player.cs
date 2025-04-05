using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    float hAxis;
    float vAxis;
    bool WalkDown;

    bool IsSide; //�� �浹 ����
    Vector3 SideVec; //�� �浹 ���� ����

    Vector3 moveVec;

    Animator Anim;

    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
    }



    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        WalkDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * PlayerSpeed * (WalkDown ? 0.3f : 1f) * Time.deltaTime;
        // bool ���� ���� ? = true / : = false (���׿�����)

        Anim.SetBool("IsRun", moveVec != Vector3.zero);
        Anim.SetBool("IsWalk", WalkDown);

        transform.LookAt(transform.position + moveVec);

    }

}
