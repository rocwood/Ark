using System;

namespace Ark
{
	/// <summary>
	/// Time in real world
	/// </summary>
	public static class RealTime
	{
		/// <summary>
		/// seconds since epochTime
		/// </summary>
		public static double now => (double)DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;

		/// <summary>
		/// milliseconds since epochTime
		/// </summary>
		public static long ticks => DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

		/// <summary>
		/// seconds since epochTime
		/// </summary>
		public static long unixTime => (long)(DateTime.UtcNow - epochTime).TotalSeconds;

		/// <summary>
		/// nanoseconds since epochTime
		/// </summary>
		public static long unixTimeNS => (DateTime.UtcNow - epochTime).Ticks * 100;

		/// <summary>
		/// epochTime: 0:00:00, January 1, 0001, UTC
		/// </summary>
		public static readonly DateTime epochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}
}
