using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance;

    [Header("UI Ayarlarý")]
    public Text goldText;

    private int currentGold;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Varsayýlan kayýt okuma
        currentGold = PlayerPrefs.GetInt("PlayerGold", 250);
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        SaveGold();
        UpdateUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            SaveGold();
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("Yetersiz Bakiye!");
            return false;
        }
    }

    // --- YENÝ EKLENEN FONKSÝYON ---
    // Bu fonksiyonu butona baðlayacaksýn
    public void SetGoldTo100()
    {
        currentGold = 500; // Miktarý direkt 100 yap
        SaveGold();        // Kaydet
        UpdateUI();        // Ekrana yaz
        Debug.Log("Altýn miktarý 100 olarak ayarlandý.");
    }
    // -----------------------------

    public int GetCurrentGold()
    {
        return currentGold;
    }

    void SaveGold()
    {
        PlayerPrefs.SetInt("PlayerGold", currentGold);
        PlayerPrefs.Save();
    }

    void UpdateUI()
    {
        if (goldText != null)
            goldText.text = currentGold.ToString();
    }
}