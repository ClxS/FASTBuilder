﻿using System.Globalization;

namespace FastBuilder.Communication.Events
{
	class ReportCounterEventArgs : BuildEventArgs
	{
		public static ReportCounterEventArgs Parse(string[] tokens)
		{
			var args = new ReportCounterEventArgs();
			BuildEventArgs.ParseBase(tokens, args);
			args.GroupName = tokens[EventArgStartIndex];
			args.CounterName = tokens[EventArgStartIndex + 1];
			args.UnitTag = tokens[EventArgStartIndex + 2];
			args.Value = float.Parse(tokens[EventArgStartIndex + 3], CultureInfo.InvariantCulture);
			return args;
		}

		public string GroupName { get; private set; }
		public string CounterName { get; private set; }
		public string UnitTag { get; private set; }
		public double Value { get; private set; }
	}
}