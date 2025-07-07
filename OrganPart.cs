using UnityEngine;

public class OrganPart : MonoBehaviour
{
    public string partName;
    [TextArea(3, 10)] public string description;
    public OrganPart[] subParts;
    public Transform focusPoint;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    public void Highlight(bool state)
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (state)
                rend.material.EnableKeyword("_EMISSION");
            else
                rend.material.DisableKeyword("_EMISSION");
        }
    }

    public void ResetTransform()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}