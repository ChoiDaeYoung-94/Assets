#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.EventSystems;

public class Managers : MonoBehaviour
{
    /// <summary>
    /// Singleton - 객체 오직 1
    /// Manager관련 script 모두 등록
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
            EditorGUILayout.HelpBox("초기 메니저 세팅", MessageType.Info);

            base.OnInspectorGUI();
        }
    }
#endif
}