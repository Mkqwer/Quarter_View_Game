using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private PlayerControls controls;

    [Header("공격 설정")]
    public float attackDistance = 3f;           // 공격 거리
    public int attackDamage = 10;               // 데미지
    public LayerMask enemyLayer;                // 적이 있는 레이어

    private Camera mainCam;

    private void Awake()
    {
        controls = new PlayerControls();

        // 마우스 좌클릭 (Attack)
        controls.Player.Attack.performed += _ => OnAttack();

        // 키보드 K (Skill)
        controls.Player.Skill.performed += _ => DoSkill();

        mainCam = Camera.main;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void OnAttack()
    {
        // 1. 마우스 커서의 위치를 가져옴 (스크린 → 월드)
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f; // 2D이므로 z는 제거

        // 2. 플레이어 → 마우스 방향으로 단위 벡터 계산
        Vector2 attackDirection = (mouseWorldPos - transform.position).normalized;

        // 3. Raycast로 적 감지
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackDistance, enemyLayer);

        Debug.DrawRay(transform.position, attackDirection * attackDistance, Color.red, 0.5f); // 디버그용

        if (hit.collider != null)
        {
            // 적이 맞았을 경우 데미지 처리
            var enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"{enemy.name}에게 {attackDamage} 데미지를 줬습니다!");
            }
        }
        else
        {
            Debug.Log("공격했지만 아무것도 맞지 않았습니다.");
        }
    }

    private void DoSkill()
    {
        Debug.Log("스킬 발동!");
    }
}
