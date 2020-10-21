using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Communicator.Events {
	public class EventState {
		/// <summary>
		/// Uuid of the eventState
		/// </summary>
		[JsonProperty(Order = -2)]
		[JsonConverter(typeof(UuidConverter))]
		public Guid Uuid { get; private set; }

		/// <summary>
		/// Sets the uuid of the eventState
		/// </summary>
		/// <param name="uuid">The uuid</param>
		public void SetUuid(Guid uuid) {
			Uuid = uuid;
		}

		/// <summary>
		/// Get the value of the EventState
		/// </summary>
		/// <returns>An object containing the value</returns>
		public virtual object GetValue() {
			return null;
		}

		/// <summary>
		/// Serializes the eventState
		/// </summary>
		/// <returns>The serialized object as string</returns>
		public override string ToString() {
			return JsonConvert.SerializeObject(this);
		}
	}
}
