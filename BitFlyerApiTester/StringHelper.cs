using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitFlyerApiTester
{
    public static class StringHelper
    {
        public static string ToHexString(this byte[] data)
            => data.Aggregate(new StringBuilder(data.Length * 2), (s, b) => s.AppendHex(b >> 4).AppendHex(b & 15)).ToString();

        private static StringBuilder AppendHex(this StringBuilder sb, int hb)
            => sb.Append((char)(hb < 10 ? '0' + hb : ('a' + hb - 10)));
    }
}
