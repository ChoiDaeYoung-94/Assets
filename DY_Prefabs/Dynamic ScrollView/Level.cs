using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace DY
{
    public class Level : MonoBehaviour
    {
        [Header("--- 세팅 ---")]
        [SerializeField, Tooltip("추후 Content에 RectTransform을 넘겨주기 위함")]
        internal RectTransform _RTR_this = null;
        [SerializeField, Tooltip("TMP_Text - level표기 위함")]
        TMP_Text _TMP_level = null;

        internal void Init(int level)
        {
            _TMP_level.text = level.ToString();
        }
    }
}