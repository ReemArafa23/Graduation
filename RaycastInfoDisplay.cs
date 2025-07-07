using UnityEngine;
using UnityEngine.UI;

public class PartHighlighter : MonoBehaviour
{
    public Transform rayOrigin;                  // Assign RightHandAnchor/PointerPose
    public LayerMask highlightLayer;             // Set to "AnatomyPart" or similar
    public GameObject infoPanel;                 // UI Panel shown on hover
    public Text partNameText;                    // UI Text for part name
    public Text descriptionText;                 // UI Text for description
    public Material highlightMaterial;           // Highlight material
    public LineRenderer lineRenderer;            // Assign LineRenderer for laser pointer

    private GameObject lastHighlightedObject;
    private Material originalMaterial;
    private bool isHighlighted;

    private GameObject currentHitObject;
    private int stableHitFrames = 0;
    private const int stableThreshold = 2;

    void Update()
    {
        if (rayOrigin == null) return;

        // Raycast from controller
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // Draw the pointer laser
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, ray.origin);
            lineRenderer.SetPosition(1, ray.origin + ray.direction * 5f);
        }

        if (Physics.Raycast(ray, out hit, 1000f, highlightLayer))
        {
            GameObject hitObj = hit.collider.gameObject;

            // Flicker prevention: Only act when stable for a few frames
            if (hitObj == currentHitObject)
            {
                stableHitFrames++;
            }
            else
            {
                currentHitObject = hitObj;
                stableHitFrames = 0;
            }

            if (stableHitFrames >= stableThreshold && hitObj != lastHighlightedObject)
            {
                ClearLastHighlight();
                lastHighlightedObject = hitObj;

                // Highlight visual
                Renderer renderer = hitObj.GetComponent<Renderer>();
                if (renderer == null) renderer = hitObj.GetComponentInChildren<Renderer>();
                if (renderer == null) renderer = hitObj.GetComponentInParent<Renderer>();

                if (renderer != null)
                {
                    originalMaterial = renderer.material;
                    renderer.material = highlightMaterial;
                    isHighlighted = true;
                }

                // Metadata
                AnatomyPartInfo metadata = hitObj.GetComponent<AnatomyPartInfo>();
                if (metadata == null) metadata = hitObj.GetComponentInChildren<AnatomyPartInfo>();
                if (metadata == null) metadata = hitObj.GetComponentInParent<AnatomyPartInfo>();

                if (metadata != null)
                {
                    partNameText.text = metadata.partName;
                    descriptionText.text = metadata.description;
                }
                else
                {
                    partNameText.text = "Unknown Part";
                    descriptionText.text = "No description available.";
                }

                infoPanel.SetActive(true);

                // Haptic Feedback
                if (OVRInput.IsControllerConnected(OVRInput.Controller.RTouch))
                {
                    OVRInput.SetControllerVibration(0.3f, 0.6f, OVRInput.Controller.RTouch);
                }
            }
        }
        else
        {
            ClearLastHighlight();
            infoPanel.SetActive(false);
            currentHitObject = null;
            stableHitFrames = 0;
        }
    }

    void LateUpdate()
    {
        if (infoPanel.activeSelf && rayOrigin != null)
        {
            Vector3 offset = rayOrigin.forward * 1.5f;
            infoPanel.transform.position = rayOrigin.position + offset;
            infoPanel.transform.rotation = Quaternion.LookRotation(infoPanel.transform.position - rayOrigin.position);
        }
    }

    void ClearLastHighlight()
    {
        if (isHighlighted && lastHighlightedObject != null)
        {
            Renderer renderer = lastHighlightedObject.GetComponent<Renderer>();
            if (renderer == null) renderer = lastHighlightedObject.GetComponentInChildren<Renderer>();
            if (renderer == null) renderer = lastHighlightedObject.GetComponentInParent<Renderer>();

            if (renderer != null && originalMaterial != null)
            {
                renderer.material = originalMaterial;
            }

            isHighlighted = false;
            lastHighlightedObject = null;

            // Stop haptics
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
    }
}
