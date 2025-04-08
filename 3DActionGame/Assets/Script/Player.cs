using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;       
    public GameObject[] Weapons;   
    public bool[] HasWeapons;       

    float hAxis;
    float vAxis;

    bool WalkDown;
    bool JumpDown;
    bool iDown;
    bool SwapDown1;
    bool SwapDown2;
    bool SwapDown3;

    bool IsJump;
    bool IsDodge;
    bool IsSwap;

    bool IsSide; //벽 충돌 유무
    Vector3 SideVec; //벽 충돌 방향 저장
    Vector3 DodgeVec;
    Vector3 MoveVec;

    Rigidbody Rigid;
    Animator Anim;
    
    GameObject NearObject;
    GameObject EquipWeapon;
    int EquipWaeponIndex = -1;
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
        Swap();
        Interation();
    }
    
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        WalkDown = Input.GetButton("Walk");
        JumpDown = Input.GetButtonDown("Jump");
        iDown = Input.GetButtonDown("Interation");
        SwapDown1 = Input.GetButtonDown("Swap1");
        SwapDown2 = Input.GetButtonDown("Swap2");
        SwapDown3 = Input.GetButtonDown("Swap3");

    }

    void Move()
    {
        MoveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (IsDodge)
            MoveVec = DodgeVec;

        if (IsSwap)
            MoveVec = Vector3.zero;

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
        if (JumpDown && MoveVec == Vector3.zero && !IsJump && !IsDodge && !IsSwap) 
        {
            Rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            Anim.SetBool("IsJump", true);
            Anim.SetTrigger("DoJump");
            IsJump = true;
        }
    }
    void Dodge()
    {
        if (JumpDown && MoveVec != Vector3.zero && !IsJump && !IsDodge && !IsSwap)
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

    void Swap()
    {
        if (SwapDown1 && (!HasWeapons[0] || EquipWaeponIndex == 0)) 
            return;
        if (SwapDown2 && (!HasWeapons[1] || EquipWaeponIndex == 1)) 
            return;
        if (SwapDown3 && (!HasWeapons[2] || EquipWaeponIndex == 2)) 
            return;
  
        int WeaponIndex = -1;
       
        if (SwapDown1) WeaponIndex = 0;
        if (SwapDown2) WeaponIndex = 1;
        if (SwapDown3) WeaponIndex = 2;
        

        if ((SwapDown1 || SwapDown2 || SwapDown3) && !IsJump && !IsDodge)
        {
            if(EquipWeapon != null)
            {               
                EquipWeapon.SetActive(false);
            }

            EquipWaeponIndex = WeaponIndex;
            EquipWeapon = Weapons[WeaponIndex];
            EquipWeapon.SetActive(true);
            
            Anim.SetTrigger("DoSwap");

            IsSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        IsSwap = false;
    }
    void Interation()
    {
        if (iDown && NearObject != null && !IsJump && !IsDodge)
        {
            if (NearObject.tag == "Weapon")
            {
                Item item = NearObject.GetComponent<Item>();
                int WeaponIndex = item.Value;
                HasWeapons[WeaponIndex] = true;

                Destroy(NearObject);
            }
        }
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

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {            NearObject = other.gameObject;

        Debug.Log(NearObject.name);
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon") 
        { 
            NearObject = null;
        }

    }
}
