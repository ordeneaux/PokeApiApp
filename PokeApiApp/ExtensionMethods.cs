using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiApp
{
    public static class ExtensionMethods
    {
        public static string ToPrettyString(this HashSet<string> values)
        {
            return $"[{string.Join(", ", values)}]";
        }
    }
}
