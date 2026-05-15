using UnityEngine;
using UnityEngine.EventSystems; // UI olaylarýný yakalamak için þart

public class MouseHoverPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Açýlýp Kapanacak Panel")]
    public GameObject infoPanel; // Inspector'dan açýlacak paneli sürükle

    void Start()
    {
        // Oyun baþladýðýnda panelin kapalý olduðundan emin olalým
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    // Mouse objenin üzerine GÝRDÝÐÝNDE çalýþýr
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            // Ýstersen burada panelin pozisyonunu mouse'un yanýna da taþýtabiliriz
            // infoPanel.transform.position = Input.mousePosition; 
        }
    }

    // Mouse objenin üzerinden ÇIKTIÐINDA çalýþýr
    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }
}