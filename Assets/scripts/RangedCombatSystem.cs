using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RangedCombatSystem : MonoBehaviour
{
    [Header("UI Ayarları")]
    public HealthBar healthBar;

    [Header("Animasyon Ayarları")]
    public Animator animator;
    public string deathAnimParameter = "Die";
    public string victoryAnimParameter = "Victory";

    [Header("Karakter Özellikleri")]
    public float maxHealth = 100f;
    public float currentHealth;
    public bool isDead = false;

    // --- YENİ EKLENEN DEĞİŞKEN ---
    // SmoothDash scripti tarafından kontrol edilecek.
    public bool isDashing = false;

    [Header("Ölüm ve Yok Olma Ayarları")]
    public float destroyDelay = 4f;

    [Header("Saldırı Ayarları")]
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public float attackRange = 15f;
    public float projectileSpeed = 20f;

    [Header("Mermi Ayarları")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 3.5f;

    [Header("Hedef ve Mantık Ayarları")]
    public string enemyTag = "Enemy";
    public float targetSearchRange = 100f;
    public float searchFrequency = 0.2f;
    public bool invertVictoryLogic = false;

    [Header("Drop Ayarları")]
    public GameObject goldPrefab;
    public float dropOffset = 0.5f;

    private GameObject currentTarget;
    private float attackCooldown = 0f;
    private float searchTimer = 0f;
    private Rigidbody rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;

        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    void Update()
    {
        if (isDead) return;

        // --- GÜNCELLENEN KISIM: DASH KONTROLÜ ---
        // Eğer dash atılıyorsa, normal hareket mantığını çalıştırma.
        if (isDashing)
        {
            // Dash sırasındaki animasyon (Hızlı koşma efekti için hızı yüksek tutuyoruz)
            if (animator != null)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("Speed", 20f); // Dash hızına uygun yüksek bir değer
            }
            return; // Buradan sonrasını (hedef takibi vb.) çalıştırma
        }

        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0)
        {
            CheckEnvironmentAndSetState(false);
            searchTimer = searchFrequency;
        }

        attackCooldown -= Time.deltaTime;

        if (currentTarget == null)
        {
            StopMovementAnimation();
            return;
        }

        if (GetTargetHealth(currentTarget) <= 0)
        {
            currentTarget = null;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget > attackRange)
        {
            MoveTowardsTarget();
            if (animator != null)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("Speed", moveSpeed);
            }
        }
        else
        {
            StopMovementAnimation();
            LookAtTarget();

            if (attackCooldown <= 0f)
            {
                StartAttackAnimation();
                attackCooldown = 1f / attackSpeed;
            }
        }
    }

    public void CheckEnvironmentAndSetState(bool forceNewTarget = false)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        int enemyCount = enemies.Length;
        bool shouldBeInVictoryState = false;

        if (!invertVictoryLogic)
        {
            if (enemyCount == 0) shouldBeInVictoryState = true;
        }
        else
        {
            if (enemyCount > 0) shouldBeInVictoryState = true;
        }

        if (animator != null)
        {
            animator.SetBool(victoryAnimParameter, shouldBeInVictoryState);
        }

        if (shouldBeInVictoryState)
        {
            currentTarget = null;
        }
        else
        {
            if (forceNewTarget) currentTarget = null;

            if (currentTarget != null && GetTargetHealth(currentTarget) > 0) return;

            FindNearestEnemy(enemies);
        }
    }

    void FindNearestEnemy(GameObject[] enemies)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null || enemy == gameObject) continue;
            float h = GetTargetHealth(enemy);
            if (h == -1 || h <= 0) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= targetSearchRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        currentTarget = nearestEnemy;
    }

    // Bu metodu public yaptım ki SmoothDash erişebilsin
    public void StopMovementAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat("Speed", 0f);
        }
    }

    void MoveTowardsTarget()
    {
        if (currentTarget == null) return;
        LookAtTarget();
        Vector3 targetPosition = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void LookAtTarget()
    {
        if (currentTarget == null) return;
        Vector3 diff = currentTarget.transform.position - transform.position;
        diff.y = 0;
        if (diff.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(diff.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    void StartAttackAnimation()
    {
        if (animator != null) animator.SetTrigger("Attack");
        else Shoot();
    }

    public void Shoot()
    {
        if (currentTarget == null) return;

        if (projectilePrefab != null && firePoint != null)
        {
            GameObject bulletGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bulletGO.GetComponent<Rigidbody>();
            if (bulletRb != null) bulletRb.linearVelocity = firePoint.forward * projectileSpeed;

            Projectile bulletScript = bulletGO.GetComponent<Projectile>();
            if (bulletScript != null) bulletScript.Seek(currentTarget.transform, attackDamage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;
        currentHealth -= damageAmount;
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        if (animator != null) animator.SetTrigger(deathAnimParameter);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (goldPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, dropOffset, 0);
            Instantiate(goldPrefab, spawnPos, Quaternion.identity);
        }

        if (healthBar != null) healthBar.gameObject.SetActive(false);
        Destroy(gameObject, destroyDelay);
    }

    float GetTargetHealth(GameObject target)
    {
        if (target == null) return -1;
        CombatSystem melee = target.GetComponent<CombatSystem>();
        if (melee != null) return melee.currentHealth;
        RangedCombatSystem ranged = target.GetComponent<RangedCombatSystem>();
        if (ranged != null) return ranged.currentHealth;
        return -1;
    }
}