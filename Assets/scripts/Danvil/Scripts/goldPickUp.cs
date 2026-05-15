using UnityEngine;
using System.Collections;

public class GoldPickup : MonoBehaviour
{
    [Header("Ayarlar")]
    public int goldAmountToAdd = 10;
    public float waitTime = 5f;

    void Start()
    {
        StartCoroutine(AutoCollectRoutine());
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 100f * Time.deltaTime);
    }

    IEnumerator AutoCollectRoutine()
    {
        yield return new WaitForSeconds(waitTime);

        // --- DEÐÝÞÝKLÝK BURADA ---
        // Artýk kendi içindeki deðiþkene deðil, Manager'a gönderiyoruz.
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(goldAmountToAdd);
            Debug.Log("Altýn toplandý! Miktar: " + goldAmountToAdd);
        }

        Destroy(gameObject);
    }
}