using UnityEngine;

public class playerscript : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 5f;
    public float jumpForce = 8f;
    public float gravity = -20f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public Animator animator;
public WeaponTrigger weaponTrigger;
    private bool isAttacking = false;
    private bool comboQueued = false;
    private bool canCombo = false;

    private Vector3 velocity;
    private bool isDashing = false;
    private float dashTime;

    void Update()
    {
        // Tấn công bằng chuột trái → Attack1
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                animator.SetTrigger("attack1");
                isAttacking = true;
                comboQueued = false;
                weaponTrigger.ResetHit();
            }
        }

        // Combo bằng chuột phải → Attack1 rồi Attack2
        if (Input.GetMouseButtonDown(1))
        {
            if (!isAttacking)
            {
                animator.SetTrigger("attack1");
                isAttacking = true;
                comboQueued = true;
                weaponTrigger.ResetHit();
            }
        }

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (isAttacking)
        {
            if (state.IsName("Attack1"))
            {
                if (state.normalizedTime >= 0.7f && !canCombo)
                {
                    canCombo = true;
                }

                if (canCombo && comboQueued)
                {
                    animator.SetTrigger("attack2");
                    comboQueued = false;
                    canCombo = false;
                    weaponTrigger.ResetHit();
                }

                if (state.normalizedTime >= 1f && !comboQueued)
                {
                    ResetAttack();
                }
            }
            else if (state.IsName("Attack2"))
            {
                if (state.normalizedTime >= 1f)
                {
                    ResetAttack();
                }
            }
        }

        // Nhảy (Jump)
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded && !isDashing)
        {
            velocity.y = jumpForce;
            animator.SetTrigger("jump");
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.F) && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
            velocity = transform.forward * dashSpeed;
            animator.SetTrigger("dash");
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x, 0, z);

        if (!isDashing)
        {
            characterController.Move(move * speed * Time.fixedDeltaTime);
            if (move != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(move);
            }
        }

        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.fixedDeltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (isDashing)
        {
            dashTime -= Time.fixedDeltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
                velocity = Vector3.zero;
            }
        }

        characterController.Move(velocity * Time.fixedDeltaTime);
        animator.SetFloat("run", move.magnitude);
    }

    void ResetAttack()
    {
        isAttacking = false;
        comboQueued = false;
        canCombo = false;
    }

    // Cho kiếm kiểm tra có đang attack không
    public bool IsAttacking()
    {
        return isAttacking;
    }
}
