using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokeApiNet;
using System;
using System.Net.Http;

namespace PokeApiUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            PokeApiClient pokeApiClient = new PokeApiNet.PokeApiClient(new HttpClient());
            var pikachu = pokeApiClient.GetResourceAsync<Pokemon>("25").Result;
            Assert.IsNotNull(pikachu);
        }
    }
}
