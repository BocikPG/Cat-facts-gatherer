using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatFacts;

public static class CatFactsEndpoints
{
	private static WebApplication _app;

	public static void AddCatFactsEndpoints(this WebApplication app)
	{
		_app = app;

		app.MapGet("/getcatfacts", GetCatFacts);

	}

	public static async Task<Results<Ok<CatFactsRequest>, ProblemHttpResult, NoContent>> GetCatFacts()
	{
		using (var client = new HttpClient())
		{
			client.BaseAddress = new Uri("https://catfact.ninja/");

			HttpResponseMessage response = new HttpResponseMessage();
			response = await client.GetAsync("fact");

			if (!response.IsSuccessStatusCode)
			{
				return TypedResults.NoContent();
			}

			string responseMessage = await response.Content.ReadAsStringAsync();
			CatFactsRequest facts = JsonSerializer.Deserialize<CatFactsRequest>(responseMessage);

			_app.Logger.LogInformation(facts.Fact);

			return TypedResults.Ok(facts);

		}
	}
}