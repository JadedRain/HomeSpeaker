using System;
using System.Runtime.Serialization;

namespace HomeSpeaker.Shared
{
	[Serializable]
	public class MissingConfigExceptionMaui : Exception
	{
		public MissingConfigExceptionMaui()
		{
		}

		public MissingConfigExceptionMaui(string? message) : base(message)
		{
		}

		public MissingConfigExceptionMaui(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected MissingConfigExceptionMaui(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}