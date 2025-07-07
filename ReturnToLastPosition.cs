using UnityEngine;
using Oculus.Interaction;

public class ReturnToGrabPose : MonoBehaviour
{
    private Grabbable grabbable;
    private Vector3 savedPosition;
    private Quaternion savedRotation;

    public Transform assemblePosition;   // لو مفكك يرجع هنا
    public Transform disassemblePosition; // لو متجمع يرجع هنا
    public bool isDisassembled = false;  // يحدد الوضع الحالي

    private bool isBeingGrabbed = false;

    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        if (grabbable == null)
        {
            Debug.LogError("Grabbable component not found!");
        }

        savedPosition = transform.position;
        savedRotation = transform.rotation;
    }

    void Update()
    {
        // الإمساك بدأ
        if (grabbable.SelectingPointsCount > 0 && !isBeingGrabbed)
        {
            isBeingGrabbed = true;
        }

        // تم الإفلات
        if (grabbable.SelectingPointsCount == 0 && isBeingGrabbed)
        {
            ReturnToCorrectPosition();
            isBeingGrabbed = false;
        }
    }

    void ReturnToCorrectPosition()
    {
        if (isDisassembled && disassemblePosition != null)
        {
            transform.position = disassemblePosition.position;
            transform.rotation = disassemblePosition.rotation;
        }
        else if (!isDisassembled && assemblePosition != null)
        {
            transform.position = assemblePosition.position;
            transform.rotation = assemblePosition.rotation;
        }
        else
        {
            // fallback
            transform.position = savedPosition;
            transform.rotation = savedRotation;
        }
    }

    // Call this externally when part gets assembled/disassembled
    public void UpdatePose(bool disassembled)
    {
        isDisassembled = disassembled;
    }

    public void UpdateSavedPose()
    {
        savedPosition = transform.position;
        savedRotation = transform.rotation;
    }
}
