using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

[ServiceContract]
public interface ITaskScheduler
{

	[OperationContract]
	/// Returns the dates of a repeating task given a start date, pattern, and count.
	DateTime[] GetOccurrences(DateTime startDate, string pattern, int count);

	[OperationContract]
	/// Returns the first occurence on or after today.
	DateTime GetNextOccurrence(DateTime startDate, string pattern);
}

