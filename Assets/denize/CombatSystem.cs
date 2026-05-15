using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
// Eðer karakterde Animator yoksa otomatik eklemesin ama hata vermemesi için kontrol edeceðiz.
public class CombatSystem : MonoBehaviour
{
    [Header("UI Ayarlarý")]
    public HealthBar healthBar;

    [Header("Animasyon Ayarlarý")]
    public Animator animator; // Editörden sürükleyebilirsin veya Start'ta otomatik bulur
    // Animatördeki parametre isimleri (Hata yapmamak için deðiþken olarak tutuyoruz)
    private string animParamSpeed = "Speed";
    private string animParamAttack = "Attack";
    private string animParamDie = "Die";

    [Header("Karakter Özellikleri")]
    public string characterName;
    public float maxHealth = 100f;
    public float currentHealth;

    // Güvenli ölüm kontrolü
    public bool isDead = false;

    [Header("Saldýrý Ayarlarý")]
    public float attackDamage = 10f;
    public float attackSpeed = 1f;
    public float attackRange = 2f;

    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 3.5f;

    [Header("Hedef Bulma")]
    public string enemyTag = "Enemy";
    public float targetSearchRange = 100f;

    [Header("Drop Ayarlarý")]
    public GameObject goldPrefab;
    public float dropOffset = 0.5f;

    private GameObject currentTarget;
    private float attackCooldown = 0f;
    private Rigidbody rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();

        // Eðer editörden atanmadýysa otomatik bul
        if (animator == null) animator = GetComponent<Animator>();

        rb.freezeRotation = true;
        rb.useGravity = true;

        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);

        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    void Update()
    {
        if (isDead) return;

        attackCooldown -= Time.deltaTime;

        // Hedef yoksa veya öldüyse bekleme moduna geç (Hýzý sýfýrla)
        if (currentTarget == null || GetTargetHealth(currentTarget) <= 0)
        {
            if (currentTarget != null) // Hedef ölmüþse referansý temizle
            {
                currentTarget = null;
                UpdateTarget();
            }

            // Hareket etmediðimiz için animatöre hýzý 0 gönderiyoruz
            UpdateAnimationSpeed(0f);
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget > attackRange)
        {
            // Hedefe doðru hareket ediyoruz
            MoveTowardsTarget();

            // Hareketi animatöre bildir (MoveSpeed deðerini gönderiyoruz)
            UpdateAnimationSpeed(moveSpeed);
        }
        else
        {
            // Saldýrý menzilindeyiz, duruyoruz
            UpdateAnimationSpeed(0f);

            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;
            }
        }
    }

    // --- ANÝMASYON GÜNCELLEME FONKSÝYONU ---
    void UpdateAnimationSpeed(float targetSpeed)
    {
        if (animator != null)
        {
            // Mevcut hýz deðerini alýp hedef hýza yumuþak bir geçiþ yapýyoruz (Damp)
            // Böylece animasyonlar tak diye kesilmez, yumuþak geçer.
            float currentAnimSpeed = animator.GetFloat(animParamSpeed);
            float smoothSpeed = Mathf.Lerp(currentAnimSpeed, targetSpeed, Time.deltaTime * 10f);

            // Ondalýklý sayýyý Animatöre gönderiyoruz
            animator.SetFloat(animParamSpeed, smoothSpeed);
        }
    }

    void MoveTowardsTarget()
    {
        if (currentTarget == null) return;

        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        Vector3 targetPosition = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null || enemy == gameObject) continue;

            float enemyHealth = GetTargetHealth(enemy);

            if (enemyHealth == -1 || enemyHealth <= 0) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance && distanceToEnemy <= targetSearchRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        currentTarget = nearestEnemy;
    }

    void Attack()
    {
        if (currentTarget == null) return;

        Vector3 lookPos = currentTarget.transform.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        // --- SALDIRI ANÝMASYONU ---
        if (animator != null)
        {
            animator.SetTrigger(animParamAttack);
        }

        Debug.Log(characterName + " vurdu -> " + currentTarget.name);

        // Not: Gerçek bir projede hasarý animasyonun tam vurduðu anda (Animation Event ile) vermek daha iyidir.
        // Þimdilik kodun akýþýný bozmamak için direkt hasar veriyoruz.
        DealDamageToTarget(currentTarget, attackDamage);
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);

        // Hasar alma animasyonu eklenebilir (Opsiyonel)
        // if(animator != null) animator.SetTrigger("Hit");

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        Debug.Log(characterName + " öldü.");

        // --- ÖLÜM ANÝMASYONU ---
        if (animator != null)
        {
            animator.SetTrigger(animParamDie);
        }

        // --- DROP (ALTIN) ---
        if (goldPrefab != null)
        {
            Vector3 spawnPos = transform.position + new Vector3(0, dropOffset, 0);
            Instantiate(goldPrefab, spawnPos, Quaternion.identity);
            Debug.Log("Altýn düþtü!");
        }
        else
        {
            Debug.LogWarning("Altýn Prefab'ý atanmamýþ!");
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (healthBar != null) healthBar.gameObject.SetActive(false);

        // Animasyonun oynatýlmasý için yok etme süresini biraz uzattýk (0.2f -> 4.0f)
        Destroy(gameObject, 4.0f);
    }

    // --- YARDIMCI FONKSÝYONLAR ---

    float GetTargetHealth(GameObject target)
    {
        if (target == null) return -1;

        CombatSystem melee = target.GetComponent<CombatSystem>();
        if (melee != null) return melee.currentHealth;

        // RangedCombatSystem kodunu görmediðim için hata vermemesi adýna try-catch veya null check ile býrakýyorum.
        // Eðer o script projenizde varsa burasý çalýþacaktýr.
        var ranged = target.GetComponent("RangedCombatSystem");
        if (ranged != null)
        {
            // Reflection veya dynamic kullanmadan projedeki diðer koda eriþmek için
            // O kodun burada tanýmlý olmasý gerekir. Þimdilik burayý yorum satýrý mantýðýnda býrakýyorum.
            // Kendi projenizde RangedCombatSystem varsa aþaðýdaki gibi açabilirsiniz:
            /*
            RangedCombatSystem r = target.GetComponent<RangedCombatSystem>();
            return r.currentHealth;
            */
            return 10; // Placeholder
        }

        return -1;
    }

    void DealDamageToTarget(GameObject target, float damage)
    {
        if (target == null) return;

        CombatSystem melee = target.GetComponent<CombatSystem>();
        if (melee != null)
        {
            melee.TakeDamage(damage);
            return;
        }

        // RangedCombatSystem entegrasyonu (Projenizde varsa)
        /*
        RangedCombatSystem ranged = target.GetComponent<RangedCombatSystem>();
        if (ranged != null)
        {
            ranged.TakeDamage(damage);
        }
        */
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, targetSearchRange);
    }
}