using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            Dictionary<string,Uri> map = new Dictionary<string,Uri>();
            using (var httpClient = new HttpClient())
            {
                var pokeClient = new Client(httpClient);
                //var charmander = pokeClient.Pokemon_retrieveAsync("4").Result;
                var list = pokeClient.Pokemon_listAsync(int.MaxValue,0,string.Empty).Result;
                foreach (var pokemon in list.Results.OrderBy(r => r.Name))
                {
                    map[pokemon.Name] = pokemon.Url;
                }
            }
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine("Type a Pokemon name then Enter.");
                var inputName = Console.ReadLine().Trim();
                if (map.ContainsKey(inputName.ToLowerInvariant()))
                {
                    validInput = true;
                }
                else
                {
                    Console.WriteLine("Invalid name.");
                    Console.WriteLine("Valid names are:");
                    foreach(string name in map.Keys)
                    {
                        Console.WriteLine(name);
                    }
                    Console.WriteLine();
                }
            }

        }
    }
}
