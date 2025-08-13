using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class enemyscript : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Transform target;
    public Transform[] patrolPoints; // Các điểm để enemy tuần tra
    private int currentPointIndex = 0;

    public float zoneRadius = 15f;
    public float runDistance = 10f;
    public float attackDistance = 5f;

    public int maxHealth = 100;
    private int currentHealth;

    public TextMeshProUGUI healthText;
    public GameObject healthCanvas; // Canvas chứa Text, gắn trên đầu enemy

    private Animator animator;
    private bool isDead = false;
    private bool isReturning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (patrolPoints.Length > 0)
        {
            navMeshAgent.SetDestination(patrolPoints[0].position);
        }
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        animator.SetFloat("distanceToPlayer", distanceToPlayer);

        float distanceToZoneCenter = Vector3.Distance(target.position, patrolPoints[0].position); // zone ở điểm đầu

        if (distanceToZoneCenter > zoneRadius)
        {
            Patrol(); // Player ra khỏi vùng → enemy tiếp tục tuần tra
            animator.SetBool("isAttacking", false);
            return;
        }

        if (distanceToPlayer <= attackDistance)
        {
            navMeshAgent.ResetPath();
            animator.SetBool("isAttacking", true);
        }
        else if (distanceToPlayer <= runDistance)
        {
            navMeshAgent.SetDestination(target.position);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            Patrol(); // Không thấy player → tiếp tục tuần tra
            animator.SetBool("isAttacking", false);
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.3f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            navMeshAgent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = currentHealth.ToString();
        healthCanvas.transform.LookAt(Camera.main.transform);
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        navMeshAgent.enabled = false;
        healthText.text = "0";
        Destroy(gameObject, 2f);
    }
}
