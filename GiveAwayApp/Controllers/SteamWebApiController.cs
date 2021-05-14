using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GiveAwayApp.Models;

namespace GiveAwayApp.Controllers
{
    public class SteamWebApiController
    {
        public HttpClient SteamApiClient { get; }

        public SteamWebApiController(HttpClient client)
        {
            client.BaseAddress = new Uri("https://store.steampowered.com/");
            client.DefaultRequestHeaders.Add("Accept", "*/*");

            SteamApiClient = client;
        }

        public async Task<Spil> GetSteamInfo(int steamId)
        {
            var response = await SteamApiClient.GetAsync($"/api/appdetails?appids={steamId}");
            response.EnsureSuccessStatusCode();

            Stream responseStream = await response.Content.ReadAsStreamAsync();
            var initialSteamSpilData = await JsonSerializer.DeserializeAsync<Dictionary<string, SteamApiReponseData>>(responseStream);

            SteamSpilData steamSpilData = initialSteamSpilData[$"{steamId}"].GetSteamSpilData;

            string prisMedValuta = steamSpilData.SteamSpilPrisOversigt.PrisMedValuta;            
            Spil spil = new()
            {
                SteamId = steamId,
                Titel = steamSpilData.SteamSpilTitel,
                SpilCoverUrl = $"https://steamcdn-a.akamaihd.net/steam/apps/{steamId}/library_600x900.jpg",
                Udgivelsesdato = DateTime.Parse(steamSpilData.SteamSpilUdgivelsesdato.Dato),
                ValgtAntal = 0,
                Genre = SteamSpilGenreStringFactory(steamSpilData.SteamSpilGenreList),
                Pris = decimal.Parse(prisMedValuta.Remove(prisMedValuta.Length - 1))
            };

            return spil;
        }

        private static string SteamSpilGenreStringFactory(SteamSpilGenre[] genreList)
        {
            string output = "";
            bool first = true;
            if (genreList.Length > 1)
            {
                foreach (SteamSpilGenre genre in genreList)
                {
                    if (first)
                    {
                        output += genre.GenreNavn;
                        first = false;
                    }
                    else
                    {
                        output += ", " + genre.GenreNavn;
                    }
                }
            }
            else
            {
                output += genreList[0].GenreNavn;
            }
            return output;
        }
    }
}
