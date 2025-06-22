using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private PlayerControls controls;

    [Header("���� ����")]
    public float attackDistance = 3f;           // ���� �Ÿ�
    public int attackDamage = 10;               // ������
    public LayerMask enemyLayer;                // ���� �ִ� ���̾�

    private Camera mainCam;

    private void Awake()
    {
        controls = new PlayerControls();

        // ���콺 ��Ŭ�� (Attack)
        controls.Player.Attack.performed += _ => OnAttack();

        // Ű���� K (Skill)
        controls.Player.Skill.performed += _ => DoSkill();

        mainCam = Camera.main;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void OnAttack()
    {
        // 1. ���콺 Ŀ���� ��ġ�� ������ (��ũ�� �� ����)
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0f; // 2D�̹Ƿ� z�� ����

        // 2. �÷��̾� �� ���콺 �������� ���� ���� ���
        Vector2 attackDirection = (mouseWorldPos - transform.position).normalized;

        // 3. Raycast�� �� ����
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackDistance, enemyLayer);

        Debug.DrawRay(transform.position, attackDirection * attackDistance, Color.red, 0.5f); // ����׿�

        if (hit.collider != null)
        {
            // ���� �¾��� ��� ������ ó��
            var enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"{enemy.name}���� {attackDamage} �������� ����ϴ�!");
            }
        }
        else
        {
            Debug.Log("���������� �ƹ��͵� ���� �ʾҽ��ϴ�.");
        }
    }

    private void DoSkill()
    {
        Debug.Log("��ų �ߵ�!");
    }
}
