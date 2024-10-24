using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiApp
{
    public class TypeAnalysis
    {
        public PokeApiNet.Type Type;
        public HashSet<string> StrongAgainst = new HashSet<string>(); 
        public HashSet<string> WeakAgainst = new HashSet<string>();
    }
}
