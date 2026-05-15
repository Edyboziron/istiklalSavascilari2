using UnityEngine;
using System.Collections;

public class LookAtNegativeZEnemy : MonoBehaviour
{
    void Start()
    {
        // 1. Ýþlem: Objeyi -Z (Vector3.back) yönüne çevir
        // Vector3.back = (0, 0, -1) demektir.
        transform.rotation = Quaternion.LookRotation(Vector2.left);

        // 2. Ýþlem: 2 saniye sayacak Coroutine'i baþlat
        StartCoroutine(DisableScriptRoutine());
    }

    IEnumerator DisableScriptRoutine()
    {
        // 2 saniye bekle
        yield return new WaitForSeconds(2.0f);

        // Scripti kapat (Unity Inspector'daki tiki kaldýrýr)
        this.enabled = false;

        // Konsola bilgi verelim (Ýsteðe baðlý)
        Debug.Log(gameObject.name + " üzerindeki script kapatýldý.");
    }
}