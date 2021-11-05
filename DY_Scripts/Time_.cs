using System;
using System.Text;
using UnityEngine;

public class Time_ : MonoBehaviour
{
    // ���� �� 00:00 ��ȯ - Datetime
    public static DateTime GetTomorrow_Datetime()
    {
        DateTime temp_DT = DateTime.Now.AddDays(1);
        string temp_str = temp_DT.Year.ToString() + "/" + temp_DT.Month.ToString() + "/" + (temp_DT.Day).ToString() + " 00:00:00";

        return Convert.ToDateTime(temp_str);
    }

    // ���� �� (00:00)���� ���� �ð� ��ȯ -> double
    public static double GetTomorrow_TotalSeconds()
    {
        DateTime temp_DT = DateTime.Now.AddDays(1);
        string temp_str = temp_DT.Year.ToString() + "/" + temp_DT.Month.ToString() + "/" + (temp_DT.Day).ToString() + " 00:00:00";
        DateTime tmep_DT_tomorrow = Convert.ToDateTime(temp_str);

        TimeSpan span = tmep_DT_tomorrow - DateTime.Now;

        return span.TotalSeconds;
    }

    // string or int ������ �ð�(��)�� string���� ��ȯ
    public static string TimeToString(object time, bool plusZero = false, bool plusSecond = false, bool colon = false)
    {
        double _time = 0;
        if (!double.TryParse(time.ToString(), out _time))
            DebugError.Parse("Time_", time.ToString());

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

        if (colon)
        {
            if (hour.Length != 0) temp.Append(hour + ":");
            if (minute.Length != 0) temp.Append(minute + ":");

            if (plusSecond || (hour.Length == 0 && minute.Length == 0))
                temp.Append(second);
        }
        else
        {
            if (hour.Length != 0) temp.Append(hour + "�ð� ");
            if (minute.Length != 0) temp.Append(minute + "��");

            if (plusSecond || (hour.Length == 0 && minute.Length == 0))
                temp.Append(" " + second + "��");
        }

        return temp.ToString();
    }

    // ���� �⼮ ���� ���� üũ - ���� ������ �ִ� ������, ���� �� -> ������ ���� �ٲ� ��
    public static bool IsDailyRewardUpdate(DateTime nowday, DateTime oldDay)
    {
        TimeSpan span = nowday - oldDay;
        if (span.TotalSeconds >= 0) return true;
        else return false;
    }

    // ���� ����Ʈ ���� �ð� string ��ȯ
    public static string update_Time(DateTime tomorrow, DateTime today, bool plusZero = false)
    {
        string temp = string.Empty;
        TimeSpan span = tomorrow - today;

        temp = TimeToString(span.TotalSeconds, plusZero: true, plusSecond: false);

        return temp;
    }
}