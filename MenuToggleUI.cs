using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggleUI : MonoBehaviour
{
    [Header("Assign these in the Inspector")]
    public GameObject uiCanvas;                    // واجهة الـ UI اللي هنظهرها/نخفيها
    public Transform head;                         // الكاميرا (رأس اللاعب)
    public InputActionProperty menuButton;         // زر Menu من الكنترولر الشمال

    private bool isVisible = false;

    void Start()
    {
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false); // نخفيها في البداية
        }

        if (menuButton != null)
        {
            menuButton.action.Enable(); // نتأكد إن الزر شغال
        }
    }

    void OnEnable()
    {
        if (menuButton != null)
        {
            menuButton.action.performed += ToggleUI;
        }
    }

    void OnDisable()
    {
        if (menuButton != null)
        {
            menuButton.action.performed -= ToggleUI;
        }
    }

    private void ToggleUI(InputAction.CallbackContext context)
    {
        Debug.Log("✔ Menu button pressed!");

        isVisible = !isVisible;

        if (uiCanvas != null)
        {
            uiCanvas.SetActive(isVisible);
        }

        if (isVisible && uiCanvas != null)
        {
            Debug.Log("✔ Showing UI in front of head.");
            PositionCanvasInFrontOfHead();
        }
        else
        {
            Debug.Log("✖ Hiding UI.");
        }
    }

    private void PositionCanvasInFrontOfHead()
    {
        if (head == null || uiCanvas == null)
            return;

        // يظهر قدام العين تمامًا بالاتجاه اللي المستخدم باصص فيه
        Vector3 spawnPosition = head.position + head.forward * 1.5f;
        uiCanvas.transform.position = spawnPosition;

        // واجه الواجهة ناحية اللاعب
        uiCanvas.transform.rotation = Quaternion.LookRotation(head.forward);
    }
}
