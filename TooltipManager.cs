using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TooltipManager : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public GameObject tooltipPrefab;
    private GameObject currentTooltip;

    private GameObject lastHitObject;

    void Update()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject != lastHitObject)
            {
                if (currentTooltip != null)
                    Destroy(currentTooltip);

                lastHitObject = hitObject;

                // لو عايز تظهر التولتيب بس للحاجات اللي ليها tag أو component معين
                if (hitObject.CompareTag("AnatomyPart")) // أو اعمل check على سكربت
                {
                    currentTooltip = Instantiate(tooltipPrefab, hit.point + Vector3.up * 0.05f, Quaternion.identity);
                    currentTooltip.GetComponentInChildren<TextMeshPro>().text = hitObject.name;

                    // خليه يبص للكاميرا دايمًا
                    currentTooltip.AddComponent<Billboard>();
                }
            }
        }
        else
        {
            lastHitObject = null;
            if (currentTooltip != null)
                Destroy(currentTooltip);
        }
    }
}
