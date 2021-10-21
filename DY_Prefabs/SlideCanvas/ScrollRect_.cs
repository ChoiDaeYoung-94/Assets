using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollRect_ : ScrollRect
{
    [Tooltip("�ڽ� ��ũ�Ѻ並 ��Ʈ�� �� ��� Horizontal�� �θ� ��ũ�Ѻ��� ���ҷ�")]
    bool _horizontal;

    ScrollView _parentScroll = null;
    ScrollRect _parentScrollRect = null;

    // �θ� ��ũ�Ѻ� Init�� ���� ����
    public void Init()
    {
        GameObject Scroll_H = GameObject.Find("SlideCanvas");
        _parentScroll = Scroll_H.GetComponent<ScrollView>();
        _parentScrollRect = Scroll_H.GetComponent<ScrollRect>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        /// <summary>
        /// x�� �� ũ�� ���� �̵��� �� ũ�ϱ� �θ� ��ũ�Ѻ� ��Ʈ��
        /// y�� �� ũ�� ���� �̵��� �� ũ�ϱ� �ڽ� ��ũ�Ѻ� ��Ʈ��
        /// </summary>
        _horizontal = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if (_horizontal)
        {
            _parentScroll.OnBeginDrag(eventData);
            _parentScrollRect.OnBeginDrag(eventData);
        }
        else base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (_horizontal)
        {
            _parentScroll.OnDrag(eventData);
            _parentScrollRect.OnDrag(eventData);
        }
        else base.OnDrag(eventData);

    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (_horizontal)
        {
            _parentScroll.OnEndDrag(eventData);
            _parentScrollRect.OnEndDrag(eventData);
        }
        else base.OnEndDrag(eventData);
    }
}
