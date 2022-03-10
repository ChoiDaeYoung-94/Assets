#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentManage : MonoBehaviour
{
    [Header("--- 세팅 ---")]
    [SerializeField, Tooltip("GO - 사용 될 content item")]
    GameObject _go_item = null;
    [SerializeField, Tooltip("필요한 item의 amount - 높이 계산에 필요")]
    float _amount = 0;
    [SerializeField, Tooltip("사용 될 ScrollView의 부모 패널 - [ 최대 사이즈(sizeDelta.y)를 알기 위함 ]")]
    RectTransform _RTR_parentView = null;
    [SerializeField, Tooltip("RectTransform - content")]
    RectTransform _RTR_content = null;
    [SerializeField, Tooltip("GridLayoutGroup - Init시 여러 계산에 필요")]
    GridLayoutGroup _GLG_content = null;
    [SerializeField, Tooltip("ContentSizeFitter - 아이템 생성 후 enabled false")]
    ContentSizeFitter _CSF_content = null;

    [Header("--- 참고용 ---")]
    [SerializeField, Tooltip("더해줄 최소 생성 라인 수 [고정]")]
    int _minPlusLine = 3;
    [SerializeField, Tooltip("최소 생성 itemList")]
    LinkedList<RectTransform> _LL_items = new LinkedList<RectTransform>();
    [SerializeField, Tooltip("비활성 itemList")]
    LinkedList<RectTransform> _LL_enabledItems = new LinkedList<RectTransform>();
    [SerializeField, Tooltip("Content의 PosY 변화 계산 위함")]
    float _curPosY = 0;
    [SerializeField, Tooltip("계산된 View의 총 Height")]
    float _contentHeight = 0;
    [SerializeField, Tooltip("첫 라인 Height")]
    float _startAnchorY = 0;
    [SerializeField, Tooltip("마지막 라인 Height")]
    float _endAnchorY = 0;
    [SerializeField, Tooltip("라인간 Height 간격")]
    float _intervalHeight = 0;
    [SerializeField, Tooltip("constraintCount의 따른 anchoredPositionX 배치")]
    List<float> _list_anchorX = new List<float>();
    [SerializeField, Tooltip("마지막 item Width_Index")]
    float _endAnchorX_index = 0;
    [SerializeField, Tooltip("마지막 item index")]
    int _endIndex = 0;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetContentHeight();

        CreateTarget();
    }

    #region Functions
    /// <summary>
    /// Item, spacing, padding을 고려한 Content의 총 Height 계산
    /// 그외 필요한 부분 계산
    /// </summary>
    void SetContentHeight()
    {
        var lineCount = Math.Ceiling(_amount / _GLG_content.constraintCount);

        float contentSize = (float)lineCount * _GLG_content.cellSize.y;
        float contentSpacingSize = (float)(lineCount - 1) * _GLG_content.spacing.y;
        float GLGTopBotPadding = _GLG_content.padding.top + _GLG_content.padding.bottom;

        _contentHeight = contentSize + contentSpacingSize + GLGTopBotPadding;

        _RTR_content.sizeDelta = new Vector2(_RTR_content.sizeDelta.x, _contentHeight);

        // cellSizeY + spacingY => 각 item의 간격
        _intervalHeight = _GLG_content.cellSize.y + _GLG_content.spacing.y;
    }

    /// <summary>
    /// Item을 최소 라인수를 맞춰 생성
    /// 생성 후 필요한 값들 계산
    /// GLG, CSF를 비활성화
    /// </summary>
    void CreateTarget()
    {
        float height = _RTR_parentView.sizeDelta.y - _GLG_content.padding.top;

        int minLine = (int)Math.Truncate(height / (_GLG_content.cellSize.y + _GLG_content.spacing.y)) + _minPlusLine;

        int x = -1;
        for (int i = -1; ++i < minLine * _GLG_content.constraintCount;)
        {
            // 최소 생성해야하는 양에 맞춰 생성 후 list에도 보관
            GameObject item = Instantiate(_go_item, _RTR_content);
            item.SetActive(true);

            RectTransform rtrItem = item.GetComponent<RectTransform>();
            _LL_items.AddLast(rtrItem);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_RTR_content);

            // 첫 줄의 item의 y 값 받음
            if (i == 0)
                _startAnchorY = rtrItem.anchoredPosition.y;

            // 한 줄에서 나올 수 있는 x 값 받음
            if (++x < _GLG_content.constraintCount)
                _list_anchorX.Add(rtrItem.anchoredPosition.x);

            // 최소로 생성하는 아이템의 양이 총 생성해야하는 양 보다 클 수 있음을 대비
            // 최종목적은 마지막 item의 anchoredPos + index
            if (i + 1 >= _amount || i + 1 >= minLine * _GLG_content.constraintCount)
            {
                _endAnchorX_index = rtrItem.anchoredPosition.x;
                _endAnchorY = rtrItem.anchoredPosition.y;
                _endIndex = i + 1;
                break;
            }
        }

        // 받은 마지막 anchoredPos를 이용해 마지막 x의 index 구함
        for (int i = -1; ++i < _list_anchorX.Count;)
            if (_list_anchorX[i] == _endAnchorX_index)
                _endAnchorX_index = i;

        // 첫 세팅을 위해 사용한 GLG, CSF 비활성화
        _GLG_content.enabled = false;
        _CSF_content.enabled = false;
    }

    /// <summary>
    /// ScrollRect -> On Value Changed에서 호출
    /// * 생성된 아이템 관리
    /// </summary>
    public void SetContentItems()
    {
        // 가장 위, 아래 부분 스크롤 시 제어 X
        if (_RTR_content.anchoredPosition.y <= 0 ||
            _RTR_content.anchoredPosition.y >= _RTR_content.sizeDelta.y - _RTR_parentView.sizeDelta.y)
            return;

        if (_RTR_content.anchoredPosition.y > _curPosY)
        {
            Debug.Log("스크롤 내리는 중");
            _curPosY = _RTR_content.anchoredPosition.y;
            ContentManageUpLine();
        }
        else
        {
            Debug.Log("스크롤 올리는 중");
            _curPosY = _RTR_content.anchoredPosition.y;
            ContentManageDownLine();
        }
    }

    void ContentManageUpLine()
    {
        // 현재 라인이 item의 머리 끝에 닿았을 때 그 위에 아이템이 존재할 경우 비활성화 하기 위함
        // => 머리 끝 부분의 PosY 구하기 위함
        double index_up = Math.Truncate((_RTR_content.anchoredPosition.y - _GLG_content.padding.top) / _intervalHeight);

        if (index_up > 0)
        {
            Debug.Log(_LL_items.Count + " ---- " + _LL_enabledItems.Count);

            // 위 부분을 다 _LL_enabledItems(비활성 리스트)에 추가 후 비활성화
            foreach (RectTransform item in _LL_items)
            {
                if (item.anchoredPosition.y - _GLG_content.cellSize.y > -_RTR_content.anchoredPosition.y)
                {
                    _LL_enabledItems.AddLast(item);
                    item.gameObject.SetActive(false);
                }
                else
                    break;
            }
            Debug.Log(_LL_items.Count + " ---- " + _LL_enabledItems.Count);

            // 비활성화 한 item들을 _LL_items에서 지운 뒤 위치 조절 후 활성화
            if (_LL_enabledItems != null && _LL_enabledItems.Count > 0)
            {
                foreach (RectTransform item in _LL_enabledItems)
                    _LL_items.Remove(item);

                foreach (RectTransform item in _LL_enabledItems)
                {
                    if (_endAnchorX_index + 1 >= _list_anchorX.Count)
                    {
                        _endAnchorX_index = 0;
                        _endAnchorY -= _intervalHeight;
                    }
                    else
                        ++_endAnchorX_index;

                    if (_endIndex + 1 <= _amount)
                    {
                        ++_endIndex;
                        _LL_items.AddLast(item);
                        item.anchoredPosition = new Vector2(_list_anchorX[(int)_endAnchorX_index], _endAnchorY);
                        item.gameObject.SetActive(true);
                    }
                    else
                        break;
                }

                foreach (RectTransform item in _LL_items)
                    _LL_enabledItems.Remove(item);
            }
        }
    }

    void ContentManageDownLine()
    {

    }
    #endregion

#if UNITY_EDITOR
    [CustomEditor(typeof(ContentManage))]
    public class customEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("dynamic scrollview\n" +
                "사용 할 content item과 Content의 Grid Layout Group을 세팅 뒤 사용", MessageType.Info);

            base.OnInspectorGUI();
        }
    }
#endif
}