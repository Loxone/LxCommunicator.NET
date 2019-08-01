using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Loxone.Communicator.Events {
	public class DaytimerState : EventState{
		/// <summary>
		/// Default value
		/// </summary>
		public double DefaultValue { get; private set; }
		/// <summary>
		/// Number of entries
		/// </summary>
		public int NrEntries { get; private set; }
		/// <summary>
		/// The actual Daytimer entries
		/// </summary>
		public List<DaytimerEntry> Entries { get; private set; }

		/// <summary>
		/// Reads the next daytimerState of a binaryReader
		/// </summary>
		/// <param name="reader">The reader that should be read of</param>
		/// <returns>The read daytimerState</returns>
		public static DaytimerState Parse(BinaryReader reader) {
			DaytimerState state = new DaytimerState();
			state.SetUuid(Communicator.Uuid.ParseUuid(reader.ReadBytes(16)));
			state.DefaultValue = reader.ReadDouble();
			state.NrEntries = reader.ReadInt32();
			state.Entries = new List<DaytimerEntry>(state.NrEntries);
			for (int i = 0; i < state.NrEntries; i++) {
				BinaryReader entryReader = new BinaryReader(new MemoryStream(reader.ReadBytes(24)));
				state.Entries.Add(DaytimerEntry.Parse(entryReader));
			}
			return state;
		}

		/// <summary>
		/// Gets the value of the daytimerState
		/// </summary>
		/// <returns>A List containing the entries</returns>
		public override object GetValue() {
			return Entries;
		}
	}
}
