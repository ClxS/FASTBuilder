﻿using System.Globalization;

namespace FastBuilder.Communication.Events
{
	internal class ReportProgressEventArgs : BuildEventArgs
	{
		public static ReportProgressEventArgs Parse(string[] tokens)
		{
			var args = new ReportProgressEventArgs();
			BuildEventArgs.ParseBase(tokens, args);
			args.Progress = float.Parse(tokens[EventArgStartIndex], CultureInfo.InvariantCulture);
			return args;
		}

		public double Progress { get; private set; }
	}
}
