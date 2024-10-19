using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApiNet;

namespace PokeApiApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new PokeApiClient();
            var p = client.GetResourceAsync<Pokemon>(1);
            p.ConfigureAwait(false);
            var pokemons = client.GetAllNamedResourcesAsync<Pokemon>();
        }
    }
}
