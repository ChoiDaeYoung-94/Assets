using UnityEngine;

public class DebugError
{
    public static void GetData(string where, string error)
    {
        Debug.Log("Error - " + where + "\nFailed to GetData : " + error);
    }
}
