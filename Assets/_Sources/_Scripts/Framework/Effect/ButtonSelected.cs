using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace Framework.Effect
{
public class ButtonSelected : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField] private Button button;

    private void PointerEnter()
    {
        transform.localScale = Vector3.one * 0.95f;
    }

    private void PointerExit()
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerEnter();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //_button.onClick.Invoke();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

#endif
}
}
