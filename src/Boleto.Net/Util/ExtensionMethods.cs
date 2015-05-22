using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoletoNet.Util
{
    public static class ExtensionMethods
    {
        public static string Limit(this string texto, int limit)
        {
            return texto.Length > limit ? texto.Substring(0, limit) : texto;
        }
    }
}
