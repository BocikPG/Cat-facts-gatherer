
using System.Text.Json.Serialization;

namespace CatFacts;

public record struct CatFactsRequest
{
	[JsonPropertyName("fact")]
	public string Fact { get; set; }
	[JsonPropertyName("length")]
	public int Length { get; set; }
}