using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loxone.Communicator.Events {
	public class TextState : EventState {

		/// <summary>
		/// The uuid of the icon
		/// </summary>
		[JsonConverter(typeof(UuidConverter))]
		public Guid UuidIcon { get; private set; }

		/// <summary>
		/// The actual event-text
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// Reads the next textState of a binaryReader
		/// </summary>
		/// <param name="reader">The reader that should be read of</param>
		/// <returns>The read textState</returns>
		public static TextState Parse(System.IO.BinaryReader reader) {
			TextState state = new TextState();
			state.SetUuid(Communicator.Uuid.ParseUuid(reader.ReadBytes(16)));
			state.UuidIcon = Communicator.Uuid.ParseUuid(reader.ReadBytes(16));
			uint length = reader.ReadUInt32();
			state.Text = "";
			uint offset = 0;
			do {
				state.Text += Encoding.UTF8.GetString(reader.ReadBytes(Convert.ToInt32(Math.Min(int.MaxValue, length - offset))));
				offset += int.MaxValue;
			} while (length > offset);
			while (reader.BaseStream.Position % 4 != 0) {
				reader.ReadByte();
			}
			return state;
		}

		/// <summary>
		/// Get the value of the textState
		/// </summary>
		/// <returns>An object containing the string value</returns>
		public override object GetValue() {
			return Text;
		}
	}
}
