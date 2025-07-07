using UnityEngine;
using Oculus.Interaction;

public class GrabReturnHandler : MonoBehaviour
{
    private Grabbable grabbable;
    private Vector3 assembledPosition;
    private Quaternion assembledRotation;
    private Vector3 disassembledPosition;
    private Quaternion disassembledRotation;
    private bool wasGrabbed = false;

    void Start()
    {
        grabbable = GetComponent<Grabbable>();

        // المكان الأصلي (قبل التفكيك)
        assembledPosition = transform.localPosition;
        assembledRotation = transform.localRotation;

        // حساب مكان التفكيك بناءً على الاتجاه والمسافة
        if (OrganDisassembler.Instance != null)
        {
            int index = OrganDisassembler.Instance.disassemblyParts.IndexOf(transform);
            if (index >= 0 && index < OrganDisassembler.Instance.moveDirections.Length)
            {
                Vector3 moveDir = OrganDisassembler.Instance.moveDirections[index].normalized * OrganDisassembler.Instance.moveDistance;
                disassembledPosition = assembledPosition + moveDir;
                disassembledRotation = assembledRotation;
            }
        }
    }

    void Update()
    {
        if (grabbable == null) return;

        // لو المستخدم بيمسك الجزء حاليًا
        if (grabbable.SelectingPointsCount > 0)
        {
            wasGrabbed = true;
        }
        // أول ما يسيبه
        else if (wasGrabbed)
        {
            bool isDisassembled = OrganDisassembler.Instance != null &&
                                  OrganDisassembler.Instance.IsPartDisassembled(transform);

            transform.localPosition = isDisassembled ? disassembledPosition : assembledPosition;
            transform.localRotation = isDisassembled ? disassembledRotation : assembledRotation;

            wasGrabbed = false;
        }
    }
}
