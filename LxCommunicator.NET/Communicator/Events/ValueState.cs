using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Loxone.Communicator.Events {
	public class ValueState : EventState {

		/// <summary>
		/// The actual value of the State
		/// </summary>
		public double Value { get; private set; }

		/// <summary>
		/// Reads the next Valuestate from a binary reader
		/// </summary>
		/// <param name="reader">The BinaryReader the ValueState should be read from</param>
		/// <returns>The read ValueState</returns>
		public static ValueState Parse(System.IO.BinaryReader reader) {
			ValueState state = new ValueState();
			state.SetUuid(Communicator.Uuid.ParseUuid(reader.ReadBytes(16)));
			state.Value = reader.ReadDouble();
			return state;
		}

		/// <summary>
		/// Gets the value of the Valuestate
		/// </summary>
		/// <returns>Object containing the double value</returns>
		public override object GetValue() {
			return Value;
		}
	}
}
