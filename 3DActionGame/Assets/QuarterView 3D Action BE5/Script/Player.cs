using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    float hAxis;
    float vAxis;
    bool WalkDown;

    bool IsSide; //벽 충돌 유무
    Vector3 SideVec; //벽 충돌 방향 저장

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
        // bool 형태 조건 ? = true / : = false (삼항연산자)

        Anim.SetBool("IsRun", moveVec != Vector3.zero);
        Anim.SetBool("IsWalk", WalkDown);

        transform.LookAt(transform.position + moveVec);

    }

}
