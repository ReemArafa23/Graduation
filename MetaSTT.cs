using UnityEngine;
using TMPro;
using Meta.WitAi.Dictation;
using Oculus.Voice.Dictation;

public class MetaSTT : MonoBehaviour
{
    public AppDictationExperience dictation;
    public TMP_InputField userInputField;

    public void StartListening()
    {
        dictation.Activate();
    }

    public void OnTranscription(string text)
    {
        userInputField.text = text;
    }
}
