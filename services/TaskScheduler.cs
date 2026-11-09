using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.UI.WebControls;

public class TaskScheduler : ITaskScheduler
{
    public DateTime[] GetOccurrences(DateTime startDate, string pattern, int count)
    {
        //reject invalid inputs and make a cap to keep the responses small
        if (count <= 0 || string.IsNullOrWhiteSpace(pattern)) return new DateTime[0];
        if (count > 52) count = 52;

        DateTime[] dates = new DateTime[count];
        for (int i = 0; i < count; i++)
        {
            //step forward from the start date based on the pattern
            switch (pattern.Trim().ToLower())
            {
                case "daily":
                    dates[i] = startDate.AddDays(i);
                    break;

                case "weekly":
                    dates[i] = startDate.AddDays(i * 7);
                    break;

                case "monthly":
                    dates[i] = startDate.AddMonths(i);
                    break;
                
                default:
                    return new DateTime[0];
            }
        }
        return dates;
    }

    public DateTime GetNextOccurrence(DateTime startDate, string pattern)
    {
        DateTime today = DateTime.Today;
        if (startDate.Date >= today) return startDate.Date;

        //walk forward until we reach today or later
        for (int i = 1; i <= 1000; i++)
        {
            DateTime next;
            switch (pattern.Trim().ToLower())
            {
                case "daily":
                    next = startDate.AddDays(i);
                    break;

                case "weekly":
                    next = startDate.AddDays(i * 7);
                    break;

                case "monthly":
                    next = startDate.AddMonths(i);
                    break;

                default:
                    return startDate;
            }
            if (next.Date >= today) return next.Date;
        }
        return startDate;
    }
}
