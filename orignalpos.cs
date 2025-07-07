using UnityEngine;

public class OriginalPosition : MonoBehaviour
{
    [HideInInspector]
    public Vector3 originalLocalPosition;

    private void Awake()
    {
        originalLocalPosition = transform.localPosition;
    }
}
