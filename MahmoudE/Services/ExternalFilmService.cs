using System.Text.Json;
using EFilmStore.DTOs;

namespace EFilmStore.Services
{
    public class ExternalFilmService
    {
        private readonly HttpClient _http;

        public ExternalFilmService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ExternalFilmDto>> SearchFilmsAsync(string searchTerm)
        {
            var q = Uri.EscapeDataString(searchTerm ?? string.Empty);
            var url = $"https://itunes.apple.com/search?term={q}&media=movie&limit=20";

            using var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch films: {resp.StatusCode}");

            var json = await resp.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ItunesSearchResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data?.Results == null || data.Results.Count == 0)
                return new List<ExternalFilmDto>();

            return data.Results.Select(r => new ExternalFilmDto
            {
                Title = r.TrackName ?? "Ingen titel",
                // iTunes ger inte alltid regissör; vi använder artistName som en approximiering
                Director = string.IsNullOrWhiteSpace(r.ArtistName) ? "Okänd" : r.ArtistName,
                PosterUrl = string.IsNullOrEmpty(r.ArtworkUrl100)
                    ? null
                    : r.ArtworkUrl100.Replace("100x100bb", "500x500bb")
            }).ToList();
        }


        private class ItunesSearchResponse
        {
            public int ResultCount { get; set; }
            public List<ItunesMovie> Results { get; set; } = new();
        }

        private class ItunesMovie
        {
            public string TrackName { get; set; }
            public string ArtistName { get; set; }
            public string ArtworkUrl100 { get; set; }
        }
    }
}
