using UnityEngine;

public class MenuInteraction : MonoBehaviour
{
    public GameObject brainModel;
    public GameObject heartModel;
    public GameObject lungModel;

    void Start()
    {
        HideAllModels(); // اخفي كل الموديلات في البداية
    }

    public void ShowBrain()
    {
        HideAllModels();
        brainModel.SetActive(true);
    }

    public void ShowHeart()
    {
        HideAllModels();
        heartModel.SetActive(true);
    }

    public void ShowLung()
    {
        HideAllModels();
        lungModel.SetActive(true);
    }

    void HideAllModels()
    {
        brainModel.SetActive(false);
        heartModel.SetActive(false);
        lungModel.SetActive(false);
    }
}
