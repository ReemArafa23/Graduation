using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PartInteractionManager : MonoBehaviour
{
    private XRRayInteractor rayInteractor;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI descriptionText;
    private Transform focusPoint;
    private Material highlightMaterial;
    private Material originalMaterial;

    private Transform currentPart;
    private bool isHeld = false;
    private XRBaseInteractor currentInteractor;

    [SerializeField] private float focusDistance = 0.6f;

    void Start()
    {
        // Auto-assign ray interactor
        rayInteractor = FindObjectOfType<XRRayInteractor>();

        // Auto-assign UI texts
        nameText = GameObject.Find("Text (1)")?.GetComponent<TextMeshProUGUI>();
        descriptionText = GameObject.Find("Text (2)")?.GetComponent<TextMeshProUGUI>();

        // Auto-assign focus point
        focusPoint = GameObject.Find("CerebrumFP")?.transform;

        // Load highlight material
        highlightMaterial = Resources.Load<Material>("HighlightMat");
    }

    void Update()
    {
        if (isHeld && currentPart != null && currentInteractor != null)
        {
            // Position held part in front of interactor
            currentPart.position = currentInteractor.transform.position + currentInteractor.transform.forward * focusDistance;
            currentPart.rotation *= Quaternion.Euler(0, Input.GetKey(KeyCode.X) ? 1f : 0f, 0); // Rotate with X key
        }
        else
        {
            RaycastHit hit;
            if (rayInteractor != null && rayInteractor.TryGetCurrent3DRaycastHit(out hit))
            {
                Transform target = hit.transform;
                if (target != currentPart)
                {
                    ResetHighlight();

                    currentPart = target;

                    if (highlightMaterial != null)
                    {
                        Renderer renderer = target.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            originalMaterial = renderer.material;
                            renderer.material = highlightMaterial;
                        }
                    }

                    nameText.text = target.name;
                    descriptionText.text = $"Details about {target.name} go here.";
                }
            }
        }
    }

    public void GrabPart(XRBaseInteractor interactor)
    {
        isHeld = true;
        currentInteractor = interactor;
    }

    public void ReleasePart()
    {
        isHeld = false;

        if (currentPart != null)
        {
            currentPart.position = focusPoint != null ? focusPoint.position : currentPart.position;
            currentPart.rotation = Quaternion.identity;
        }

        currentInteractor = null;
    }

    private void ResetHighlight()
    {
        if (currentPart != null && originalMaterial != null)
        {
            Renderer renderer = currentPart.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material = originalMaterial;
        }
    }
}
