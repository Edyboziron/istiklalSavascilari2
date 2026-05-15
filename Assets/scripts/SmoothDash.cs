using UnityEngine;
using System.Collections;

public class SmoothDash : MonoBehaviour
{
    [Header("Ayarlar")]
    public float beklemeSuresi = 1.0f;
    public float mesafe = 10.0f;
    public float hiz = 20.0f;

    void Start()
    {
        StartCoroutine(BekleVeGit());
    }

    IEnumerator BekleVeGit()
    {
        // 1. Bekleme
        yield return new WaitForSeconds(beklemeSuresi);

        RangedCombatSystem combat = GetComponent<RangedCombatSystem>();

        // --- DÜZELTME 1: Combat sistemine Dash attýðýmýzý bildiriyoruz ---
        if (combat != null)
        {
            combat.isDashing = true;
        }

        // 2. Hedefi belirle
        Vector3 hedefNokta = transform.position + (Vector3.right * mesafe);

        // 3. Git
        while (Vector3.Distance(transform.position, hedefNokta) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, hedefNokta, hiz * Time.deltaTime);
            yield return null;
        }

        // 4. Konumu sabitle
        transform.position = hedefNokta;

        // --- DÜZELTME 2: Dash bitti, kontrolü geri veriyoruz ---
        if (combat != null)
        {
            combat.isDashing = false; // Dash bitti, artýk normal hareket edebilir.

            // ÖNEMLÝ: Hýzýn "takýlý kalmasýný" engellemek için animasyonu sýfýrlýyoruz.
            combat.StopMovementAnimation();

            // Yeni hedef ara
            combat.CheckEnvironmentAndSetState(true);

            Debug.Log("Dash tamamlandý, animasyon sýfýrlandý ve hedef yeniden hesaplandý.");
        }
    }
}