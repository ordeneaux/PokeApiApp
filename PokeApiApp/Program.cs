using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApiNet;
namespace PokeApiApp
{
    internal class Program
    {
        private static string BASEURL = "https://pokeapi.co/api/v2/";
        static void Main(string[] args)
        {
            //var client = new PokeApiClient();
            //var p = client.GetResourceAsync<Pokemon>(1);
            //p.ConfigureAwait(false);
            //var pokemons = client.GetAllNamedResourcesAsync<Pokemon>();
            //DataFetcher.
            //var client = new DataFetcher();
            Dictionary<string, Uri> map = new Dictionary<string, Uri>();
            var httpClient = new HttpClient();
            var pokeClient = new Client(httpClient);
            //var charmander = pokeClient.Pokemon_retrieveAsync("4").Result;
            var list = pokeClient.Pokemon_listAsync(int.MaxValue, 0, string.Empty).Result;
            foreach (var pokemon in list.Results.OrderBy(r => r.Name))
            {
                map[pokemon.Name] = pokemon.Url;
            }
            string id = GetValidPokemonIdFromConsole(map);
            //NSwag generated client can't deserialize detail response correctly
            //var playerPokemonDetail = pokeClient.Pokemon_retrieveAsync(id).Result;
            PokeApiClient pokeApiClient = new PokeApiNet.PokeApiClient(new HttpClient());
            var pokemonDetail = pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(id).Result;
            List<PokeApiNet.Type> typeDetailList = new List<PokeApiNet.Type>();
            foreach (var type in pokemonDetail.Types)
            {
                var pokemonType = pokeApiClient.GetResourceAsync<PokeApiNet.Type>(type.Type.Name).Result;
                typeDetailList.Add(pokemonType);
            }
        }
        private static string GetValidPokemonIdFromConsole(Dictionary<string, Uri> map)
        {
            while (true)
            {
                Console.WriteLine("Type a Pokemon name then Enter.");
                var inputName = Console.ReadLine().Trim();
                if (map.ContainsKey(inputName.ToLowerInvariant()))
                {
                    return IdStringFromUri(map[inputName.ToLowerInvariant()]);
                }
                else
                {
                    Console.WriteLine("Invalid name.");
                    Console.WriteLine("Valid names are:");
                    foreach (string name in map.Keys)
                    {
                        Console.WriteLine(name);
                    }
                    Console.WriteLine();
                }
            }
        }
        private static string IdStringFromUri(Uri uri)
        {
            var urlParts = uri.AbsolutePath.Split('/');
            return urlParts[4];
        }
    }
}
