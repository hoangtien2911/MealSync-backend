﻿namespace MealSync.Application.Common.Utils;

public class TimeFrameUtils
{
    public static string GetTimeFrameString(int startTime, int endTime)
    {
        var startFrame = GetTimeHoursFormat(startTime);
        var endFame = GetTimeHoursFormat(endTime);
        return startFrame + "-" + endFame;
    }

    public static string GetTimeHoursFormat(int time)
    {
        var hour = time / 100;
        var minute = time % 100;
        return string.Format("{0}:{1}", hour.ToString().PadLeft(2,'0'), minute.ToString().PadRight(2, '0'));
    }

    public static int GetTimeHoursInRound(){
        DateTimeOffset utcTime = DateTimeOffset.UtcNow;
        DateTimeOffset timeInUtcPlus7 = utcTime.ToOffset(TimeSpan.FromHours(7));

        // Get the current hour and minute
        int hour = timeInUtcPlus7.Hour;
        int minute = timeInUtcPlus7.Minute;

        // Logic to round up to the next section (either xx:30 or next hour)
        if (minute > 0 && minute <= 30)
        {
            minute = 30;
        }
        else if (minute > 30)
        {
            hour += 1;
            minute = 0;
        }

        // Return the result as a number in hhmm format
        int roundedTime = hour * 100 + minute;
        return roundedTime;
    }

    public static DateTimeOffset GetCurrentTime()
    {
        DateTimeOffset utcTime = DateTimeOffset.UtcNow;
        return utcTime.ToOffset(TimeSpan.FromHours(7));
    }

    public static int ConvertEndTime(int endTime)
    {
        if (endTime == 2400)
        {
            return 0;
        }

        return endTime;
    }
}