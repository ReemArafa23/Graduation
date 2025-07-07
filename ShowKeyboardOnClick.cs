using UnityEngine;
using UnityEngine.EventSystems;

public class ShowKeyboardOnClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject virtualKeyboard;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (virtualKeyboard != null)
        {
            virtualKeyboard.SetActive(true);
        }
    }
}
