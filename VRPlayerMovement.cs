using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerMovement : MonoBehaviour
{
    public Transform head;                  // للاتجاه فقط
    public float speed = 1.5f;              // سرعة الحركة
    public float gravity = -9.81f;          // الجاذبية
    public float groundedOffset = -0.1f;    // لتثبيت اللاعب على الأرض
    private CharacterController cc;
    private float verticalVelocity = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // نتحقق إذا زر X أو Y مضغوط
        bool isXPressed = false;
        bool isYPressed = false;

        leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out isXPressed);   // زر X
        leftHand.TryGetFeatureValue(CommonUsages.secondaryButton, out isYPressed); // زر Y

        Vector3 move = Vector3.zero;

        if (isYPressed)
        {
            // يتحرك للأمام في اتجاه الرأس
            Vector3 headForward = new Vector3(head.forward.x, 0, head.forward.z).normalized;
            move += headForward;
        }

        if (isXPressed)
        {
            // يتحرك للخلف عكس اتجاه الرأس
            Vector3 headBackward = new Vector3(-head.forward.x, 0, -head.forward.z).normalized;
            move += headBackward;
        }

        // الجاذبية
        if (cc.isGrounded)
        {
            verticalVelocity = groundedOffset;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // الحركة النهائية
        Vector3 finalMove = move * speed + Vector3.up * verticalVelocity;
        cc.Move(finalMove * Time.deltaTime);
    }
}
