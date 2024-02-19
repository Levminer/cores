using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace lib;
public class JSON {
	public JsonSerializerOptions SerializerOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		Converters = { new InfinityToZeroConverter() }
	};
}

internal class InfinityToZeroConverter : JsonConverter<double> {
	public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		return reader.GetDouble();
	}

	public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options) {
		// If the value is infinity, write 0 instead
		if (double.IsInfinity(value)) {
			writer.WriteNumberValue(0);
		} else {
			writer.WriteNumberValue(value);
		}
	}
}
