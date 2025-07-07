using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using Meta.WitAi.TTS.Utilities;

public class GeminiChatManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField userInputField;
    public TMP_Text responseText;

    [Header("Meta TTS")]
    public TTSSpeaker speaker;

    [Header("Gemini API")]
    [Tooltip("Paste your Gemini API key here")]
    public string apiKey;

    public void SendToGemini()
    {
        if (!string.IsNullOrEmpty(userInputField.text))
        {
            StartCoroutine(SendRequest(userInputField.text));
        }
    }

    IEnumerator SendRequest(string userInput)
    {
        string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";
        string jsonBody = "{ \"contents\": [ { \"parts\": [ { \"text\": \"" + EscapeJson(userInput) + "\" } ] } ] }";

        UnityWebRequest request = new UnityWebRequest(endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResult = request.downloadHandler.text;
            string reply = ExtractText(jsonResult);

            responseText.text = reply;

            if (speaker != null)
            {
                speaker.Speak(reply);
            }
        }
        else
        {
            responseText.text = "Error: " + request.error;
        }
    }

    private string EscapeJson(string text)
    {
        return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    private string ExtractText(string json)
    {
        int index = json.IndexOf("\"text\":");
        if (index > 0)
        {
            int start = json.IndexOf("\"", index + 7) + 1;
            int end = json.IndexOf("\"", start);
            return json.Substring(start, end - start);
        }
        return "Unable to parse response.";
    }
}
