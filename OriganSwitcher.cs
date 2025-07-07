using UnityEngine;

public class OrganSwitcher : MonoBehaviour
{
    public GameObject brainModel;
    public GameObject heartModel;
    public GameObject lungModel;
    public GameObject menuCanvas; // اضف هنا الCanvas بتاع المنيو

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void UpdateMenuPosition()
    {
        if (menuCanvas != null)
        {
            Vector3 forwardPosition = cameraTransform.position + cameraTransform.forward * 1.2f;
            menuCanvas.transform.position = forwardPosition;
            menuCanvas.transform.rotation = Quaternion.LookRotation(cameraTransform.forward);
        }
    }

    public void ShowBrain()
    {
        brainModel.SetActive(true);
        heartModel.SetActive(false);
        lungModel.SetActive(false);

        EnableDisassembler(brainModel, true);
        EnableDisassembler(heartModel, false);
        EnableDisassembler(lungModel, false);

        UpdateMenuPosition();
    }

    public void ShowHeart()
    {
        brainModel.SetActive(false);
        heartModel.SetActive(true);
        lungModel.SetActive(false);

        EnableDisassembler(brainModel, false);
        EnableDisassembler(heartModel, true);
        EnableDisassembler(lungModel, false);

        UpdateMenuPosition();
    }

    public void ShowLung()
    {
        brainModel.SetActive(false);
        heartModel.SetActive(false);
        lungModel.SetActive(true);

        EnableDisassembler(brainModel, false);
        EnableDisassembler(heartModel, false);
        EnableDisassembler(lungModel, true);

        UpdateMenuPosition();
    }

    private void EnableDisassembler(GameObject model, bool enable)
    {
        var disassembler = model.GetComponent<OrganDisassembler>();
        if (disassembler != null)
        {
            disassembler.enabled = enable;
        }
    }
}
