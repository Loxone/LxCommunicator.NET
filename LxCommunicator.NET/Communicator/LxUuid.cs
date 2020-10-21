using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Communicator {
	public static class Uuid {
		/// <summary>
		/// Parses an uuid from a string
		/// </summary>
		/// <param name="uuid">the string containing the uuid</param>
		/// <returns>The parsed uuid</returns>
		public static Guid ParseUuid(string uuid) {
			return new Guid(uuid.Replace("-", ""));
		}

		/// <summary>
		/// Parses an uuid from a byte[]
		/// </summary>
		/// <param name="bytes">the byte[] containing the uuid</param>
		/// <returns>The parsed uuid</returns>
		public static Guid ParseUuid(byte[] bytes) {
			return new Guid(bytes);
		}

		/// <summary>
		/// Gets the uuid as string
		/// </summary>
		/// <param name="uuid">The uuid that should be converted</param>
		/// <returns>The converted uuid as string</returns>
		public static string GetUuidString(this Guid uuid) {
			string value = uuid.ToString("N");
			return value.Insert(16, "-").Insert(12, "-").Insert(8, "-");
		}
	}

	/// <summary>
	/// Converter used to read/write uuids from/to json text
	/// </summary>
	public class UuidConverter : JsonConverter<Guid> {
		public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue, JsonSerializer serializer) {
			return Uuid.ParseUuid(reader.ReadAsString());
		}

		public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer) {
			writer.WriteValue(value.GetUuidString());
		}
	}

}
