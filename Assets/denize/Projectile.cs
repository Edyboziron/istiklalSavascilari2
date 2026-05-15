using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float damage;
    private float speed = 15f;

    [Header("Patlama Ayarlarý (Oynanýþ)")]
    [Tooltip("Hasarýn ne kadar geniþ bir alana yayýlacaðý")]
    public float explosionRadius = 5f;

    [Header("Görsel Ayarlar (Efekt)")]
    [Tooltip("Patlama efekti prefabý")]
    public GameObject explosionEffect;

    [Tooltip("Efektin oluþturulacaðý açý (X, Y, Z). Örneðin yere paralel yapmak için X=90 verebilirsin.")]
    public Vector3 effectRotation = Vector3.zero;

    [Tooltip("Efektin boyutu. 1 = Orijinal, 2 = Ýki kat büyük, 0.5 = Yarýsý.")]
    public float effectScale = 1f;

    public void Seek(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // 1. Patlama Efektini Oluþtur ve Ayarla
        if (explosionEffect != null)
        {
            // Inspector'dan girilen açýyý (Vector3) Quaternion'a çeviriyoruz
            Quaternion rotationToSpawn = Quaternion.Euler(effectRotation);

            // Efekti oluþturuyoruz
            GameObject effectIns = Instantiate(explosionEffect, transform.position, rotationToSpawn);

            // Boyutunu ayarlýyoruz (Mevcut boyutunu çarpan ile büyütüp/küçültüyoruz)
            effectIns.transform.localScale = effectIns.transform.localScale * effectScale;

            // 2 saniye sonra sil
            Destroy(effectIns, 2f);
        }

        // 2. Alan Hasarý (Area of Effect)
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            CombatSystem melee = nearbyObject.GetComponent<CombatSystem>();
            if (melee != null)
            {
                melee.TakeDamage(damage);
            }

            RangedCombatSystem ranged = nearbyObject.GetComponent<RangedCombatSystem>();
            if (ranged != null)
            {
                ranged.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}