using System;

namespace Analytics
{
	public enum EventRecordStatus
	{
		Failed,
		Recorded,
		UniqueCountExceeded,
		ParamsCountExceeded,
		LogCountExceeded,
		LoggingDelayed
	}
}
