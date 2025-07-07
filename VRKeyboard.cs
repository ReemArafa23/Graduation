using TMPro;
using UnityEngine;

public class VRKeyboard : MonoBehaviour
{
    public TMP_InputField targetInputField;
    private bool isUpperCase = true;

    public void KeyPress(string key)
    {
        if (targetInputField == null) return;

        string character = isUpperCase ? key.ToUpper() : key.ToLower();
        targetInputField.text += character;
    }

    public void Backspace()
    {
        if (targetInputField == null || string.IsNullOrEmpty(targetInputField.text)) return;
        targetInputField.text = targetInputField.text.Substring(0, targetInputField.text.Length - 1);
    }

    public void ClearInput()
    {
        if (targetInputField == null) return;
        targetInputField.text = "";
    }

    public void ToggleCase()
    {
        isUpperCase = !isUpperCase;
    }

    public void SubmitInput()
    {
        // Optional: Automatically call GeminiChatManager.SendToGemini() or hide keyboard
        Debug.Log("Submit: " + targetInputField.text);
    }

    public void HideKeyboard()
    {
        gameObject.SetActive(false);
    }
}
