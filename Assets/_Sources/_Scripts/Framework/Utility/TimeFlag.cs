using UnityEngine;


public class TimeFlag
{
    private static float flag = 0;

    public static void SaveTime()
    {
        flag = Time.time;
    }

    public static bool CheckTimeRange(float range)
    {
        if (flag == 0)
        {
            SaveTime();
            return false;
        }
        return Time.time - flag >= range;
        
    }
}
