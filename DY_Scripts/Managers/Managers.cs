#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.EventSystems;

public class Managers : MonoBehaviour
{
    /// <summary>
    /// Singleton - ��ü ���� 1
    /// Manager���� script ��� ���
    /// </summary>
    static Managers instance;
    public static Managers Instance { get { return instance; } }

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("Settings");
            if (go == null)
            {
                go = new GameObject { name = "Settings" };
                go.AddComponent<EventSystem>();
                go.AddComponent<StandaloneInputModule>();
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            instance = go.GetComponent<Managers>();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Managers))]
    public class customEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("�ʱ� �޴��� ����", MessageType.Info);

            base.OnInspectorGUI();
        }
    }
#endif
}