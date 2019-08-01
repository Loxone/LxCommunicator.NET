using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Loxone.Communicator.Events {
	public class WeatherState : EventState{
		/// <summary>
		/// The date since the last update
		/// </summary>
		public uint LastUpdate { get; private set; }
		/// <summary>
		/// The number of entries
		/// </summary>
		public int NrEntries { get; private set; }

		/// <summary>
		/// The actual weatherEntries
		/// </summary>
		public List<WeatherEntry> Entries { get; private set; }

		/// <summary>
		/// Reads the next weatherState of a binaryReader
		/// </summary>
		/// <param name="reader">The reader that should be read of</param>
		/// <returns>The read weatherState</returns>
		public static WeatherState Parse(BinaryReader reader) {
			WeatherState state = new WeatherState();
			state.SetUuid(Communicator.Uuid.ParseUuid(reader.ReadBytes(16)));
			state.LastUpdate = reader.ReadUInt32();
			state.NrEntries = reader.ReadInt32();
			state.Entries = new List<WeatherEntry>(state.NrEntries);
			for (int i = 0; i < state.NrEntries; i++) {
				BinaryReader entryReader = new BinaryReader(new MemoryStream(reader.ReadBytes(24)));
				state.Entries.Add(WeatherEntry.Parse(entryReader));
			}
			return state;
		}

		/// <summary>
		/// Gets the value of the weatherState
		/// </summary>
		/// <returns>A list containing the weatherEntries</returns>
		public override object GetValue() {
			return Entries;
		}
	}
}
