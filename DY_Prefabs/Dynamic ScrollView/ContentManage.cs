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
    [SerializeField, Tooltip("계산된 View의 총 Height")]
    float _contentHeight = 0;

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
    /// </summary>
    void SetContentHeight()
    {
        var lineCount = Math.Ceiling(_amount / _GLG_content.constraintCount);

        float contentSize = (float)lineCount * _GLG_content.cellSize.y;
        float contentSpacingSize = (float)(lineCount - 1) * _GLG_content.spacing.y;
        float GLGTopBotPadding = _GLG_content.padding.top + _GLG_content.padding.bottom;

        _contentHeight = contentSize + contentSpacingSize + GLGTopBotPadding;

        _RTR_content.sizeDelta = new Vector2(_RTR_content.sizeDelta.x, _contentHeight);
    }

    /// <summary>
    /// Item을 최소 라인수를 맞춰 생성 뒤 GLG, CSF를 비활성화
    /// </summary>
    void CreateTarget()
    {
        float height = _RTR_parentView.sizeDelta.y - _GLG_content.padding.top;
        int minLine = (int)Math.Truncate(height / (_GLG_content.cellSize.y + _GLG_content.spacing.y)) + 2;

        for (int i = -1; ++i < minLine * _GLG_content.constraintCount;)
            Instantiate(_go_item, _RTR_content).SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(_RTR_content);

        _GLG_content.enabled = false;
        _CSF_content.enabled = false;
    }

    /// <summary>
    /// ScrollRect -> On Value Changed에서 호출
    /// * 생성된 아이템 관리
    /// </summary>
    public void SetContentItems()
    {
        if (_RTR_content.anchoredPosition.y <= 0 || 
            _RTR_content.anchoredPosition.y >= _RTR_content.sizeDelta.y - _RTR_parentView.sizeDelta.y)
            return;
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
