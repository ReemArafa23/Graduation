using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VRKeyboardManager : MonoBehaviour
{
    public TMP_InputField targetInputField;
    public GeminiChatManager chatManager;
    public bool isUpperCase = true;

    private void Start()
    {
        AutoBindKeys();
    }

    public void AutoBindKeys()
    {
        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            string label = button.GetComponentInChildren<TMP_Text>().text;

            button.onClick.RemoveAllListeners();

            switch (label)
            {
                case "Delete":
                    button.onClick.AddListener(Backspace);
                    break;
                case "Clear":
                    button.onClick.AddListener(ClearInput);
                    break;
                case "Letter case":
                    button.onClick.AddListener(ToggleCase);
                    break;
                case "Enter":
                    button.onClick.AddListener(SubmitInput);
                    break;
                default:
                    string key = label; // Cache for closure
                    button.onClick.AddListener(() => KeyPress(key));
                    break;
            }
        }
    }

    public void KeyPress(string key)
    {
        if (targetInputField == null) return;
        string charToAdd = isUpperCase ? key.ToUpper() : key.ToLower();
        targetInputField.text += charToAdd;
    }

    public void Backspace()
    {
        if (string.IsNullOrEmpty(targetInputField.text)) return;
        targetInputField.text = targetInputField.text.Substring(0, targetInputField.text.Length - 1);
    }

    public void ClearInput()
    {
        targetInputField.text = "";
    }

    public void ToggleCase()
    {
        isUpperCase = !isUpperCase;
    }

    public void SubmitInput()
    {
        Debug.Log("Submitting: " + targetInputField.text);
        if (chatManager != null)
        {
            chatManager.SendToGemini();
        }

        gameObject.SetActive(false); // Optional: hide keyboard after submit
    }
}
