using UnityEngine;

public class ImageCloser : MonoBehaviour
{
    [Header("Kapatýlacak Objeler")]
    // Buraya istediðin kadar Image/Obje sürükleyebilirsin
    public GameObject[] imagesToClose;

    // Bu fonksiyonu butona baðlayacaðýz
    public void CloseAllImages()
    {
        foreach (GameObject img in imagesToClose)
        {
            if (img != null)
            {
                img.SetActive(false); // Objeyi pasif yapar (gizler)
            }
        }
        Debug.Log("Seçili resimler kapatýldý.");
    }
}