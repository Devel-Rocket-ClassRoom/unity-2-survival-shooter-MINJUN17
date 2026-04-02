using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private PlayerController playerController;
    private PlayerShooter playerShooter;
    private Animator animator;
    public Slider healthSlider;

    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerShooter = GetComponent<PlayerShooter>();
        animator = GetComponent<Animator>();
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerController != null) playerController.enabled = true;
        if(playerShooter != null) playerShooter.enabled = true;
        UpdateHealth();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!IsDead)
        {
            base.OnDamage(damage, hitPoint, hitNormal);
            UpdateHealth();
        }
    }

    public override void Die()
    {
        base.Die();
        animator.SetTrigger("Die");
        playerController.enabled = false;
        playerShooter.enabled = false;

    }

    public void UpdateHealth()
    {
        healthSlider.maxValue = startingHealth;

        healthSlider.value = Health;
    }
}
