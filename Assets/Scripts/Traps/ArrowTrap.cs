using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSound;

    private void Attack()
    {
        cooldownTimer = 0;
        arrows[FindInactiveArrow()].transform.position = firePoint.position;
        arrows[FindInactiveArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }
    void OnWillRenderObject()
    {
        
        if (Camera.current.CompareTag("MainCamera") && cooldownTimer >= attackCooldown)
        {
            Attack();
            SoundManager.instance.PlaySound(arrowSound);
        }
    }
    private int FindInactiveArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
    }
}