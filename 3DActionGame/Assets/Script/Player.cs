using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    float hAxis;
    float vAxis;

    bool WalkDown;
    bool JumpDown;
    bool IsJump;
    bool IsDodge;

    bool IsSide; //벽 충돌 유무
    Vector3 SideVec; //벽 충돌 방향 저장
    Vector3 DodgeVec;
    Vector3 MoveVec;

    Rigidbody Rigid;
    Animator Anim;

    void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }
    
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        WalkDown = Input.GetButton("Walk");
        JumpDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        MoveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (IsDodge)
            MoveVec = DodgeVec;

        //충돌하는 방향은 무시
        if (IsSide && MoveVec == SideVec)
            MoveVec = Vector3.zero;


        transform.position += MoveVec * PlayerSpeed * (WalkDown ? 0.3f : 1f) * Time.deltaTime;
        // bool 형태 조건 ? = true / : = false (삼항연산자)

        Anim.SetBool("IsRun", MoveVec != Vector3.zero);
        Anim.SetBool("IsWalk", WalkDown);
    }
    
    void Turn()
    {
        transform.LookAt(transform.position + MoveVec);
    }

   
    void Jump()
    {
        if (JumpDown && MoveVec == Vector3.zero && !IsJump && !IsDodge) 
        {
            Rigid.AddForce(Vector3.up * 25, ForceMode.Impulse);
            Anim.SetBool("IsJump", true);
            Anim.SetTrigger("DoJump");
            IsJump = true;
        }
    }
    void Dodge()
    {
        if (JumpDown && MoveVec != Vector3.zero && !IsJump && !IsDodge)
        {
            DodgeVec = MoveVec;
            PlayerSpeed *= 2;
            Anim.SetTrigger("DoDodge");
            IsDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        PlayerSpeed *= 0.5f;
        IsDodge = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Anim.SetBool("IsJump", false);
            IsJump = false;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            IsSide = true;
            SideVec = MoveVec;
        }
    }

    //벽 충돌 Out 체크
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            IsSide = false;
            SideVec = Vector3.zero;
        }
    }

}
