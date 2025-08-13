using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public int damage = 20;
    private bool hasHit = false;
    private playerscript player;

    private void Start()
    {
        player = GetComponentInParent<playerscript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit || player == null || !player.IsAttacking()) return;

        enemyscript enemy = other.GetComponent<enemyscript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            hasHit = true;
        }
    }

    public void ResetHit()
    {
        hasHit = false;
    }
}
