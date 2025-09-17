namespace EFilmStore.Models
{
    public class TmdbSearchResponse
    {
        public List<TmdbFilmResult> Results { get; set; }
    }

    public class TmdbFilmResult
    {
        public string Title { get; set; }
        public string PosterPath { get; set; }
    }
}
