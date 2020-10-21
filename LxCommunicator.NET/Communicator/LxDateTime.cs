using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loxone.Communicator {
	public static class LxDateTime {
		public static readonly DateTime Invalid = new DateTime(2009, 1, 1, 0, 0, 0).AddSeconds(-1);
		public static readonly DateTime Default = new DateTime(2009, 1, 1, 0, 0, 0);

		public static bool IsValidLxDateTime(this DateTime dateTime) {
			return dateTime >= Default;
		}

		public static DateTime Parse(double value) {
			return Default.AddSeconds(value);
		}

		public static double GetLxDateTime(this DateTime dateTime) {
			return dateTime.Subtract(dateTime).TotalSeconds;
		}
	}

	public class LxDateTimeConverter : JsonConverter<DateTime> {
		public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer) {
			if (reader.Value is long integer) {
				return LxDateTime.Parse(integer);
			}
			else if (reader.Value is double floatingPoint) {
				return LxDateTime.Parse(floatingPoint);
			}
			else if (reader.Value is DateTime dateTime) {
				return dateTime;
			}
			else if (reader.Value is string text) {
				return LxDateTime.Parse(Convert.ToDouble(text));
			}
			else {
				return LxDateTime.Invalid;
			}

		}

		public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer) {
			writer.WriteValue(value.GetLxDateTime());
		}
	}
}
