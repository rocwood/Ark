using System;

namespace Ark
{
	/// <summary>
	/// Accumulative time interface
	/// </summary>
	public interface ITimeAccumulator
	{
		double accumTime { get; }
		double deltaTime { get; }

		bool Update();
		void Reset();
	}

	/// <summary>
	/// Default time accumulator base on RealTime
	/// </summary>
	public class RealTimeAccumulator : ITimeAccumulator
	{
		public double accumTime => _accumTime;
		public double deltaTime => _deltaTime;

		public bool Update()
		{
			var now = Now();

			var ticks = now - _lastTicks;
			if (ticks < 0)
				ticks = 0;

			_deltaTicks = ticks;
			_accumTicks += ticks;
			_lastTicks = now;

			_deltaTime = (double)_deltaTicks / TimeSpan.TicksPerSecond;
			_accumTime = (double)_accumTicks / TimeSpan.TicksPerSecond;

			return true;
		}

		public void Reset()
		{
			_lastTicks = Now();
			_accumTicks = 0;
			_deltaTicks = 0;

			_accumTime = 0;
			_deltaTime = 0;
		}

		private static long Now() => DateTime.UtcNow.Ticks;

		private long _lastTicks = Now();
		private long _accumTicks = 0;
		private long _deltaTicks = 0;

		private double _accumTime = 0;
		private double _deltaTime = 0;
	}

	/// <summary>
	/// Fixed-step time accumulator
	/// </summary>
	public class FixedTimeAccumulator: ITimeAccumulator
	{
		public const double DefaultDeltaTime = 0.05;

		public double accumTime => _accumTime;
		public double deltaTime { get => _deltaTime; set => _deltaTime = value; }

		private ITimeAccumulator _baseAccumulator = null;

		public FixedTimeAccumulator(ITimeAccumulator baseAccumulator)
		{
			_baseAccumulator = baseAccumulator;
		}

		/// <summary>
		/// baseAccumular.Update should be called before
		/// </summary>
		public bool Update()
		{
			if (_baseAccumulator == null || _deltaTime <= 0)
				return false;

			double deltaTime = _baseAccumulator.accumTime - _accumTime;
			if (deltaTime < _deltaTime)
				return false;

			_accumTime += _deltaTime;

			return true;
		}

		public void Reset()
		{
			_accumTime = 0;
		}

		private double _accumTime = 0;
		private double _deltaTime = DefaultDeltaTime;
	}
}
