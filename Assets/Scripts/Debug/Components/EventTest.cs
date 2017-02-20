using UnityEngine;
using UnityEngine.EventSystems;

public class EventTest : EventTrigger
{
    static GameObject obj;
    static string dbg = "<null>";
    int counter = 0;

    void Awake()
    {
        if (obj == null)
            obj = gameObject;
    }
    public override void OnBeginDrag(PointerEventData data)
    {
        dbg = gameObject.name+": OnBeginDrag called.";
    }

    public override void OnCancel(BaseEventData data)
    {
        dbg = gameObject.name+": OnCancel called.";
    }

    public override void OnDeselect(BaseEventData data)
    {
        dbg = gameObject.name+": OnDeselect called.";
    }

    public override void OnDrag(PointerEventData data)
    {
        //dbg = gameObject.name+": OnDrag called.";
    }

    public override void OnDrop(PointerEventData data)
    {
        dbg = gameObject.name+": OnDrop called.";
    }

    public override void OnEndDrag(PointerEventData data)
    {
        //dbg = gameObject.name+": OnEndDrag called.";
    }

    public override void OnInitializePotentialDrag(PointerEventData data)
    {
        //dbg = gameObject.name+": OnInitializePotentialDrag called.";
    }

    public override void OnMove(AxisEventData data)
    {
        dbg = gameObject.name+": OnMove called.";
    }

    public override void OnPointerClick(PointerEventData data)
    {
        //dbg = gameObject.name+": OnPointerClick called.";
    }

    public override void OnPointerDown(PointerEventData data)
    {
        dbg = gameObject.name+": OnPointerDown called.";
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        dbg = gameObject.name+": OnPointerEnter called. "+counter;
        counter++;
    }

    public override void OnPointerExit(PointerEventData data)
    {
        dbg = gameObject.name+": OnPointerExit called.";
        counter = 0;
    }

    public override void OnPointerUp(PointerEventData data)
    {
        dbg = gameObject.name+": OnPointerUp called.";
    }

    public override void OnScroll(PointerEventData data)
    {
        dbg = gameObject.name+": OnScroll called.";
    }

    public override void OnSelect(BaseEventData data)
    {
        dbg = gameObject.name+": OnSelect called.";
    }

    public override void OnSubmit(BaseEventData data)
    {
        dbg = gameObject.name+": OnSubmit called.";
    }

    public override void OnUpdateSelected(BaseEventData data)
    {
        dbg = gameObject.name+": OnUpdateSelected called.";
    }

    void OnGUI()
    {
        if (obj == gameObject)
        {
            GUI.Label(new Rect(10, 80, 300, 250), dbg);

        }
    }
}