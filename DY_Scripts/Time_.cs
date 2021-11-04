using System;
using System.Text;
using UnityEngine;

public class Time_ : MonoBehaviour
{
    // 다음 날 00:00 반환
    public static DateTime GetTomorrow()
    {
        DateTime temp_DT = DateTime.Now.AddDays(1);
        string temp_str = temp_DT.Year.ToString() + "/" + temp_DT.Month.ToString() + "/" + (temp_DT.Day).ToString() + " 00:00:00";

        return Convert.ToDateTime(temp_str);
    }

    // string or int 형식의 시간(초)을 string으로 반환
    public static string TimeToString(object time, bool plusZero = false, bool plusSecond = false)
    {
        double _time = int.Parse(time as string);
        StringBuilder temp = new StringBuilder();

        string hour = string.Empty;
        string minute = string.Empty;
        string second = string.Empty;

        if (_time >= 3600)
        {
            hour = ((int)_time / 3600).ToString();
            minute = (((int)_time % 3600) / 60).ToString();
            second = (((int)_time % 3600) % 60).ToString();
        }
        else if (_time < 3600 && _time >= 60)
        {
            minute = ((int)_time / 60).ToString();
            second = ((int)_time % 60).ToString();
        }
        else
            second = ((int)_time).ToString();

        if (plusZero)
        {
            if (hour.Length == 1) hour = "0" + hour;
            if (minute.Length == 1) minute = "0" + minute;
            if (second.Length == 1) second = "0" + second;
        }

        if (hour.Length != 0) temp.Append(hour + "시간 ");
        if (minute.Length != 0) temp.Append(minute + "분");

        if (plusSecond || (hour.Length == 0 && minute.Length == 0))
            temp.Append(" " + second + "초");

        return temp.ToString();
    }

    // 일일 퀘스트 갱신 체크
    public static bool IsDailyQuestUpdate(DateTime tomorrow, DateTime today)
    {
        TimeSpan span = tomorrow - today;
        if (span.TotalSeconds <= 0) return false;
        else return true;
    }

    // 일일 퀘스트 남은 시간 string 반환
    public static string update_Time(DateTime tomorrow, DateTime today, bool plusZero = false)
    {
        string temp = string.Empty;
        TimeSpan span = tomorrow - today;

        temp = TimeToString(span.TotalSeconds, plusZero: true, plusSecond: false);

        return temp;
    }

    // 일일 출석 보상 갱신 체크
    public static bool IsDailyRewardUpdate(DateTime nowday, DateTime lastday)
    {
        int spanDay = nowday.Day - lastday.Day;

        if (spanDay != 0) return true;
        else return false;
    }
}