using UnityEngine;
using Oculus;

public class VRSnapTurnOrSmoothTurn : MonoBehaviour
{
    public float turnSpeed = 60f; // Degrees per second for smooth turn
    public bool useSnapTurn = false;
    public float snapAngle = 45f; // Angle per snap turn
    private bool readyToTurn = true;

    void Update()
    {
        float turnInput = 0f;

        // لو زر A (يمين) مضغوط
        if (OVRInput.Get(OVRInput.Button.One))
        {
            turnInput = 1f;
        }
        // لو زر B (يسار) مضغوط
        else if (OVRInput.Get(OVRInput.Button.Two))
        {
            turnInput = -1f;
        }

        if (useSnapTurn)
        {
            // SNAP TURN
            if (readyToTurn && turnInput != 0f)
            {
                float angle = turnInput > 0 ? snapAngle : -snapAngle;
                transform.Rotate(0, angle, 0);
                readyToTurn = false;
            }
            else if (turnInput == 0f)
            {
                // إعادة التفعيل لما المستخدم يسيب الزر
                readyToTurn = true;
            }
        }
        else
        {
            // SMOOTH TURN
            if (turnInput != 0f)
            {
                transform.Rotate(0, turnInput * turnSpeed * Time.deltaTime, 0);
            }
        }
    }
}
