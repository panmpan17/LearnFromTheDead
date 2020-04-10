using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VaccineTube : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform RectTransform;

    public Rect Rect {
        get {
            Rect rect = RectTransform.rect;
            rect.center = RectTransform.TransformPoint(rect.center);
            rect.size = RectTransform.TransformVector(rect.size);
            return rect;
        }
    }

    [System.NonSerialized]
    public int Index;

    public enum TubeType { Red, Orange, Yellow, Blue, Green, Purple };
    public TubeType Type;

    private void Awake() {
        RectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // transform.position = VaccineTableUI.ins.PositionOfTube(Index);
        if (!VaccineTableUI.ins.FindSwitchTube(this)) {
            transform.position = VaccineTableUI.ins.PositionOfTube(Index);
        }
    }
}
