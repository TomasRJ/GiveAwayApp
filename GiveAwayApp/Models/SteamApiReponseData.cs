using System.Text.Json.Serialization;

namespace GiveAwayApp.Models
{
    public class SteamApiReponseData
    {
        [JsonPropertyName("data")]
        public SteamSpilData GetSteamSpilData { get; set; }
    }
    public class SteamSpilData
    {
        [JsonPropertyName("name")]
        public string SteamSpilTitel { get; set; }
        [JsonPropertyName("release_date")]
        public Udgivelsesdato SteamSpilUdgivelsesdato { get; set; }
        [JsonPropertyName("genres")]
        public SteamSpilGenre[] SteamSpilGenreList { get; set; }
        [JsonPropertyName("price_overview")]
        public SteamSpilPris SteamSpilPrisOversigt { get; set; }
    }

    public class Udgivelsesdato
    {
        [JsonPropertyName("date")]
        public string Dato { get; set; }
    }
    public class SteamSpilGenre
    {
        [JsonPropertyName("description")]
        public string GenreNavn { get; set; }
    }
    public class SteamSpilPris
    {
        [JsonPropertyName("final_formatted")]
        public string PrisMedValuta { get; set; }
    }
}
