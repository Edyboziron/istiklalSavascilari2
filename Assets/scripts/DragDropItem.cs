using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Býrakýlacak Prefab")]
    public GameObject prefabRef;

    [Header("Maliyet Ayarý")]
    public int cost = 50;

    private bool canAfford = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GoldManager.Instance.GetCurrentGold() >= cost)
        {
            canAfford = true;
            if (BuildingManager.Instance != null)
                BuildingManager.Instance.StartDraggingFromUI(prefabRef);
        }
        else
        {
            canAfford = false;
            Debug.Log("Yetersiz Bakiye!");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canAfford) return;

        if (BuildingManager.Instance != null)
            BuildingManager.Instance.Dragging();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canAfford) return;

        if (BuildingManager.Instance != null)
        {
            // DEÐÝÞÝKLÝK BURADA:
            // StopDragging fonksiyonunun bize "true" (koyuldu) veya "false" (iptal) döndürmesini bekliyoruz.
            bool basari = BuildingManager.Instance.StopDragging();

            if (basari == true)
            {
                // Sadece bina baþarýyla konduysa para harca
                GoldManager.Instance.SpendGold(cost);
            }
            else
            {
                Debug.Log("Bina koyulamadý, para iade (kesilmedi).");
            }
        }
    }
}