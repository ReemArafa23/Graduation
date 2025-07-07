using UnityEngine;

public class TooltipHighlighter : MonoBehaviour
{
    public Transform rayOrigin; // ControllerPointerPose
    public float rayLength = 5f;
    public LayerMask layerMask;
    public GameObject tooltipPrefab;

    private GameObject currentTooltip;
    private GameObject lastHitObject;

    void Update()
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != lastHitObject)
            {
                ClearTooltip();

                lastHitObject = hitObject;

                currentTooltip = Instantiate(tooltipPrefab, hit.point + Vector3.up * 0.05f, Quaternion.identity);
                currentTooltip.GetComponent<TextMesh>().text = hitObject.name;

                // Make it always face camera
                currentTooltip.transform.forward = Camera.main.transform.forward;
            }
        }
        else
        {
            ClearTooltip();
        }
    }

    void ClearTooltip()
    {
        if (currentTooltip != null)
        {
            Destroy(currentTooltip);
            currentTooltip = null;
        }

        lastHitObject = null;
    }
}
