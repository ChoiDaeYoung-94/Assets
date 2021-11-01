using UnityEditor;

using System;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    /// <summary>
    /// �ʱ�ȭ �ؾ��ϴ� ��ũ��Ʈ���� �̸��� �״�� ����
    /// -> ���� ���� ������ �ʱ�ȭ ����
    /// </summary>
    enum Scripts
    {
        
    }

    [Tooltip("�ʱ�ȭ �ؾ� �� ��ũ��Ʈ�� ���� ���ӿ�����Ʈ")]
    [SerializeField] GameObject[] _go_initialze = null;

    private void Awake()
    {
        foreach (Scripts script in Enum.GetValues(typeof(Scripts)))
        {
            foreach (GameObject item in _go_initialze)
            {                
                if (item.GetComponent(script.ToString()) != null)
                {
                    // �����ؾ� �� �޼���
                    item.GetComponent(script.ToString()).SendMessage("");
                    break;
                }                
            }
        }
    }

    [CustomEditor(typeof(Level_Lobby))]
    public class customEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("�ʱ�ȭ ������ ������ ��� ���", MessageType.Info);

            base.OnInspectorGUI();
        }
    }
}
