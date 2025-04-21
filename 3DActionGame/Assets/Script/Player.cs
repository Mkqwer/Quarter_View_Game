using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.Rendering.DebugUI.Table;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;       
    public GameObject[] Weapons;   
    public bool[] HasWeapons;
    public GameObject[] Grenades;
    public int HasGrenades;
    public Camera FollowCamera;

    public int Ammo;
    public int Coin;
    public int Health;
    
    public int MaxAmmo;
    public int MaxCoin;
    public int MaxHealth;
    public int MaxHasGrenades;

    float hAxis;
    float vAxis;
    float FireDelay;

    bool WalkDown;
    bool JumpDown;
    bool iDown;
    bool SwapDown1;
    bool SwapDown2;
    bool SwapDown3;

    bool FireDown;
    bool ReloadDown;

    bool IsReload;
    bool IsFireReady = true;
    bool IsBorder;
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
    Weapon EquipWeapon;
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
        Attack();
        Reload();
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
        FireDown = Input.GetButton("Fire1");
        ReloadDown = Input.GetButtonDown("Reload");
        SwapDown1 = Input.GetButtonDown("Swap1");
        SwapDown2 = Input.GetButtonDown("Swap2");
        SwapDown3 = Input.GetButtonDown("Swap3");
        
    }

    void Move()
    {
        MoveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (IsDodge)
            DodgeVec = MoveVec;

        if (IsSwap || IsReload || !IsFireReady)
            MoveVec = Vector3.zero;

        //충돌하는 방향은 무시
        if (IsSide && MoveVec == SideVec)
            MoveVec = Vector3.zero;

        if (!IsBorder)
            transform.position += MoveVec * PlayerSpeed * (WalkDown ? 0.3f : 1f) * Time.deltaTime;


      
        // bool 형태 조건 ? = true / : = false (삼항연산자)

        Anim.SetBool("IsRun", MoveVec != Vector3.zero);
        Anim.SetBool("IsWalk", WalkDown);
    }
    
    void Turn()
    {
        transform.LookAt(transform.position + MoveVec);
        if (FireDown)
        {
            Ray Ray = FollowCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit RayHit;
            if (Physics.Raycast(Ray, out RayHit, 100))
            {
                Vector3 nextVec = RayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
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

    void Attack()
    {
        if (EquipWeapon == null)
        { 
            return;
        }
        FireDelay += Time.deltaTime;
        IsFireReady = EquipWeapon.Rate < FireDelay;

        if (FireDown && IsFireReady && !IsDodge && !IsSwap && !IsReload && !IsJump)
        {
            EquipWeapon.Use();
            Anim.SetTrigger(EquipWeapon.type == Weapon.Type.Melee ? "DoSwing" : "DoShot");
            FireDelay = 0;
        }
    }

    void Reload()
    {
        if (EquipWeapon == null)
            return;

        if (EquipWeapon.type == Weapon.Type.Melee)
            return;

        if (Ammo == 0)
            return;


        if (ReloadDown && !IsJump && !IsDodge && !IsSwap && IsFireReady)
        {
            Debug.Log("Reload");
            Anim.SetTrigger("DoReload");
            IsReload = true;

             Invoke("ReloadOut", 2f);
            
        }
    }

    void ReloadOut()
    {
        int ReAmmo = Ammo < EquipWeapon.maxAmmo ? Ammo : EquipWeapon.maxAmmo;
        EquipWeapon.CurAmmo = ReAmmo;
        Ammo -= ReAmmo;
        IsReload = false;
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
        

        if ((SwapDown1 || SwapDown2 || SwapDown3) && !IsJump && !IsDodge && !IsReload)
        {
            if(EquipWeapon != null)
            {
                EquipWeapon.gameObject.SetActive(false);
            }

            EquipWaeponIndex = WeaponIndex;
            EquipWeapon = Weapons[WeaponIndex].GetComponent<Weapon>();
            EquipWeapon.gameObject.SetActive(true);

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

    void FreezeRotation()
    {
        Rigid.angularVelocity = Vector3.zero;
    }
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        IsBorder = Physics.Raycast(transform.position, MoveVec, 5, LayerMask.GetMask("Wall"));

    }
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
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

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.ItemType)
            {
                case Item.Type.Ammo:
                    Ammo += item.Value;
                    if (Ammo > MaxAmmo)
                        Ammo = MaxAmmo;
                    break;
                case Item.Type.Coin:
                    Coin += item.Value;
                    if (Coin > MaxCoin)
                        Coin = MaxCoin;
                    break;
                case Item.Type.Heart:
                    Health += item.Value;
                    if(Health > MaxHealth)
                       Health = MaxHealth;
                    break;
                case Item.Type.Grenade:
                    Grenades[HasGrenades].SetActive(true);
                    HasGrenades += item.Value;
                    if (HasGrenades > MaxHasGrenades)
                        HasGrenades = MaxHasGrenades;
                    break;
            }
            Destroy(other.gameObject);
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
