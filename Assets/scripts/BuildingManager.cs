using UnityEngine;
using UnityEngine.InputSystem; // New Input System gerekli
using UnityEngine.EventSystems; // UI'a týklamayý engellemek için

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [Header("Yerleþtirme Ayarlarý")]
    public float gridSize = 1.0f;
    public LayerMask groundLayer;   // "Zemin" Layer'ý
    public LayerMask buildingLayer; // "Building" Layer'ý

    [Header("Grid Görünümü")]
    public bool showGrid = true;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public Color gridColor = Color.black;

    // --- DEÐÝÞKENLER ---
    private GameObject _activeObject; // Þu an elimizdeki obje
    private bool _isNewObjectFromUI;  // UI'dan mý geldi?
    private GameObject _prefabRef;    // UI referansý
    private Vector3 _originalPosition; // Yerden aldýysak eski konumu

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Eðer sahnede aktif bir Pointer (Mouse veya Parmak) yoksa iþlem yapma
        if (Pointer.current == null) return;

        // 1. Týklama / Dokunma (Press) - Yerdeki objeyi seçme
        // wasPressedThisFrame: Mouse týklandýðýnda veya parmak ekrana deðdiðinde true olur.
        if (_activeObject == null && Pointer.current.press.wasPressedThisFrame)
        {
            // UI üzerine týklýyorsak oyun dünyasýndan bir þey seçmeyelim (Mobil için kritik)
            if (IsPointerOverUI()) return;

            TryGrabSceneObject();
        }

        // 2. Býrakma (Release) - Parmaðý veya Mouse'u býrakma
        // wasReleasedThisFrame: Mouse býrakýldýðýnda veya parmak ekrandan kalktýðýnda true olur.
        if (_activeObject != null && !_isNewObjectFromUI && Pointer.current.press.wasReleasedThisFrame)
        {
            StopDragging();
        }

        // 3. UI'dan yeni gelen obje için özel durum:
        // Mobilde butona bastýðýnda parmaðýný kaldýrmýþ olabilirsin. 
        // Tekrar dokunup yerleþtirmek için bir týklama daha bekleyebiliriz veya
        // (Opsiyonel) UI'dan gelen objeyi yerleþtirmek için de parmaðý kaldýrýnca býrakmasýný saðlayabilirsin:
        if (_activeObject != null && _isNewObjectFromUI && Pointer.current.press.wasReleasedThisFrame)
        {
            // Eðer "Sürükle býrak" mantýðý istiyorsan burayý açabilirsin.
            // Þimdilik UI'dan gelenler genelde "týkla - hayaleti gör - týkla yerleþtir" mantýðýyla çalýþýr.
            // Aþaðýdaki satýrý açarsan: Parmaðýný kaldýrdýðýn an binayý koyar.
            StopDragging();
        }

        // 4. Sürükleme (Her karede pozisyon güncelleme)
        Dragging();
    }

    // --- A) YERDEKÝ OBJEYÝ TUTMA ---
    private void TryGrabSceneObject()
    {
        // Mouse.current yerine Pointer.current kullanýyoruz
        Vector2 screenPos = Pointer.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, buildingLayer))
        {
            _activeObject = hit.collider.gameObject;
            _isNewObjectFromUI = false;

            // Hata durumunda döneceði yeri kaydet
            _originalPosition = _activeObject.transform.position;

            // Taþýma moduna al
            SetObjectMode(_activeObject, true);
        }
    }

    // --- B) UI'DAN YENÝ OBJE GETÝRME ---
    public void StartDraggingFromUI(GameObject prefab)
    {
        // Eðer zaten elimizde bir þey varsa önce onu temizleyelim
        if (_activeObject != null) Destroy(_activeObject);

        _prefabRef = prefab;
        _isNewObjectFromUI = true;

        _activeObject = Instantiate(prefab);
        SetObjectMode(_activeObject, true);
    }

    // --- C) TAÞIMA (DRAGGING) ---
    public void Dragging()
    {
        if (_activeObject == null) return;

        // Pointer pozisyonunu al (Mouse veya Dokunmatik)
        Vector2 screenPos = Pointer.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, groundLayer))
        {
            // Grid Hesaplama
            float x = Mathf.Round(hit.point.x / gridSize) * gridSize;
            float z = Mathf.Round(hit.point.z / gridSize) * gridSize;
            float y = hit.point.y + 0.5f; // Pivot noktasýna göre ayarla (0.5f genelde merkez için)

            // Lerp kullanarak yumuþak takip ettirebilirsin (Mobilde parmak altýný görmek için iyidir)
            // Þimdilik direkt atama yapýyoruz:
            _activeObject.transform.position = new Vector3(x, y, z);

            if (!_activeObject.activeSelf) _activeObject.SetActive(true);
        }
        else
        {
            // Zemin dýþýna çýktýysa gizle
            if (_activeObject.activeSelf) _activeObject.SetActive(false);
        }
    }

    // --- D) BIRAKMA VE KONTROL (STOP DRAGGING) ---
    public bool StopDragging()
    {
        if (_activeObject == null) return false;

        bool isOccupied = IsGridOccupied(_activeObject.transform.position);
        bool isOffGrid = !_activeObject.activeSelf;
        bool isValidPlacement = !isOccupied && !isOffGrid;

        // --- SENARYO 1: UI'DAN GELDÝ ---
        if (_isNewObjectFromUI)
        {
            if (isValidPlacement)
            {
                GameObject finalObj = Instantiate(_prefabRef, _activeObject.transform.position, Quaternion.identity);

                int layerID = LayerMask.NameToLayer("Building");
                if (layerID != -1) SetLayerRecursively(finalObj, layerID);

                Destroy(_activeObject);
                _activeObject = null;
                _prefabRef = null;
                return true;
            }
            else
            {
                Debug.Log("Geçersiz yer! Ýptal edildi.");
                Destroy(_activeObject);
                _activeObject = null;
                _prefabRef = null;
                return false;
            }
        }
        // --- SENARYO 2: YERDEN ALINDI ---
        else
        {
            if (isValidPlacement)
            {
                SetObjectMode(_activeObject, false);
                _activeObject = null;
                return true;
            }
            else
            {
                Debug.Log("Geçersiz hamle! Eski yerine dönüyor.");
                _activeObject.transform.position = _originalPosition;
                _activeObject.SetActive(true);
                SetObjectMode(_activeObject, false);
                _activeObject = null;
                return false;
            }
        }
    }

    // --- YARDIMCI: UI Týklama Kontrolü (Mobilde çok önemlidir) ---
    private bool IsPointerOverUI()
    {
        // EventSystem kullanýlarak parmaðýn bir buton üzerinde olup olmadýðýna bakar
        if (EventSystem.current == null) return false;

        // Mobilde parmak ID'si genelde pointerId ile alýnýr, PC'de -1 veya 0'dýr.
        // IsPointerOverGameObject() New Input System ile bazen parametre ister.
        // En basit evrensel yöntem:
        return EventSystem.current.IsPointerOverGameObject();
    }

    // --- DÝÐER YARDIMCI METOTLAR ---
    bool IsGridOccupied(Vector3 targetPosition)
    {
        Vector3 boxSize = new Vector3(gridSize * 0.9f, 1f, gridSize * 0.9f);
        Collider[] colliders = Physics.OverlapBox(targetPosition, boxSize / 2, Quaternion.identity, buildingLayer);

        foreach (var col in colliders)
        {
            if (col.gameObject != _activeObject) return true;
        }
        return false;
    }

    private void SetObjectMode(GameObject obj, bool isDragging)
    {
        foreach (var col in obj.GetComponentsInChildren<Collider>())
        {
            col.enabled = !isDragging;
        }

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = isDragging;
            rb.useGravity = !isDragging;
        }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;
        foreach (Transform child in obj.transform) SetLayerRecursively(child.gameObject, newLayer);
    }

    private void OnDrawGizmos()
    {
        if (!showGrid) return;
        Gizmos.color = gridColor;
        Vector3 center = transform.position;
        float yOffset = center.y + 0.1f;

        for (float x = -gridWidth * gridSize; x <= gridWidth * gridSize; x += gridSize)
        {
            Gizmos.DrawLine(new Vector3(x + center.x, yOffset, (-gridHeight * gridSize) + center.z),
                            new Vector3(x + center.x, yOffset, (gridHeight * gridSize) + center.z));
        }
        for (float z = -gridHeight * gridSize; z <= gridHeight * gridSize; z += gridSize)
        {
            Gizmos.DrawLine(new Vector3((-gridWidth * gridSize) + center.x, yOffset, z + center.z),
                            new Vector3((gridWidth * gridSize) + center.x, yOffset, z + center.z));
        }
    }
}