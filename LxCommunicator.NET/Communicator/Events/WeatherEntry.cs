using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Communicator.Events {
	public class WeatherEntry {
		public int TimeStamp { get; private set; }
		public int WeatherType { get; private set; }
		public int WindDirection { get; private set; }
		public int SolarRadiation { get; private set; }
		public int RelativeHumidity { get; private set; }
		public double Temperature { get; private set; }
		public double PerceivedTemperature { get; private set; }
		public double DewPoint { get; private set; }
		public double Precipitation { get; private set; }
		public double WindSpeed { get; private set; }
		public double BarometricPressure { get; private set; }

		/// <summary>
		/// Reads the next weatherEntry of a binaryReader
		/// </summary>
		/// <param name="reader">The reader that should be read of</param>
		/// <returns>The read weatherEntry</returns>
		public static WeatherEntry Parse(System.IO.BinaryReader reader) {
			WeatherEntry entry = new WeatherEntry() {
				TimeStamp = reader.ReadInt32(),
				WeatherType = reader.ReadInt32(),
				WindDirection = reader.ReadInt32(),
				SolarRadiation = reader.ReadInt32(),
				RelativeHumidity = reader.ReadInt32(),
				Temperature = reader.ReadDouble(),
				PerceivedTemperature = reader.ReadDouble(),
				DewPoint = reader.ReadDouble(),
				Precipitation = reader.ReadDouble(),
				WindSpeed = reader.ReadDouble(),
				BarometricPressure = reader.ReadDouble()
			};
			return entry;
		}
	}
}
