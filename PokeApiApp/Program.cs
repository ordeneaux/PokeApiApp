using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApiNet;
using log4net;
using log4net.Repository.Hierarchy;


namespace PokeApiApp
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            try
            {


                Dictionary<string, Uri> pokemonMap = new Dictionary<string, Uri>();
                var httpClient = new HttpClient();
                var pokeClient = new Client(httpClient);
                //var charmander = pokeClient.Pokemon_retrieveAsync("4").Result;
                var pokemonList = pokeClient.Pokemon_listAsync(int.MaxValue, 0, string.Empty).Result;
                foreach (var pokemon in pokemonList.Results.OrderBy(r => r.Name))
                {
                    pokemonMap[pokemon.Name] = pokemon.Url;
                }
                string id = GetValidPokemonIdFromConsole(pokemonMap);
                //NSwag generated client can't deserialize detail response correctly
                //var playerPokemonDetail = pokeClient.Pokemon_retrieveAsync(id).Result;
                PokeApiClient pokeApiClient = new PokeApiNet.PokeApiClient(new HttpClient());
                var inputPokemon = pokeApiClient.GetResourceAsync<PokeApiNet.Pokemon>(id).Result;
                List<TypeAnalysis> myReturnObject = new List<TypeAnalysis>();
                foreach (var type in inputPokemon.Types)
                {
                    var pokemonType = pokeApiClient.GetResourceAsync<PokeApiNet.Type>(type.Type.Name).Result;
                    var analysis = new TypeAnalysis()
                    {
                        Type = pokemonType
                    };

                    var doubleDamageTo = pokemonType.DamageRelations.DoubleDamageTo.Select(d => d.Name).ToList();
                    foreach (string typeName in doubleDamageTo)
                        analysis.StrongAgainst.Add(typeName);

                    var noDamangeFrom = pokemonType.DamageRelations.NoDamageFrom.Select(d => d.Name).ToList();
                    foreach (string typeName in noDamangeFrom)
                        analysis.StrongAgainst.Add(typeName);

                    var halfDamangeFrom = pokemonType.DamageRelations.HalfDamageFrom.Select(d => d.Name).ToList();
                    foreach (string typeName in halfDamangeFrom)
                        analysis.StrongAgainst.Add(typeName);

                    var noDamageTo = pokemonType.DamageRelations.NoDamageTo.Select(d => d.Name).ToList();
                    foreach (string typeName in noDamageTo)
                        analysis.WeakAgainst.Add(typeName);

                    var halfDamangeTo= pokemonType.DamageRelations.HalfDamageTo.Select(d => d.Name).ToList();
                    foreach (string typeName in halfDamangeTo)
                        analysis.WeakAgainst.Add(typeName);

                    var doubleDamageFrom = pokemonType.DamageRelations.DoubleDamageFrom.Select(d => d.Name).ToList();
                    foreach (string typeName in doubleDamageFrom)
                        analysis.WeakAgainst.Add(typeName);

                    myReturnObject.Add(analysis);
                }

                WriteSummary(myReturnObject);
            }
            catch (WebException ex)
            {
                Console.WriteLine("An error has occurred.  Check internet connection.");
                log.Error(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occurred.  See logs for details.");
                log.Error(ex);
            }
            finally
            {
                Console.WriteLine("Press Return or Enter to Exit.");
                Console.ReadLine();
            }
        }

        private static void WriteSummary(List<TypeAnalysis> myReturnObject)
        {
            if(myReturnObject.Count == 1) {
                Console.WriteLine("Type: " + myReturnObject[0].Type.Name);
                Console.WriteLine("Strong Against: " + myReturnObject[0].StrongAgainst.ToPrettyString());
                Console.WriteLine("Weak Against: " + myReturnObject[0].WeakAgainst.ToPrettyString());
            }
            else
            {
                int slotIndex = 1;
                foreach(TypeAnalysis analysis in myReturnObject)
                {
                    Console.WriteLine($"Slot {slotIndex} Type: " + analysis.Type.Name);
                    Console.WriteLine("Strong Against: " + analysis.StrongAgainst.ToPrettyString());
                    Console.WriteLine("Weak Against: " + analysis.WeakAgainst.ToPrettyString());
                    slotIndex++;
                }
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
