using UnityEngine;

public class DebugError
{
    public static void GetData(string where, string error)
    {
        Debug.Log("<color=red>Error</color> - " + where + "\nFailed to GetData : " + error);
    }

    public static void Parse(string where, string error)
    {
        Debug.Log("<color=red>Error</color> - " + where + "\nFailed to Parse : " + error);
    }

    public static void Contain(string where, string item)
    {
        Debug.Log("<color=red>Error</color> - " + where + "\nThere is no " + item);
    }
}