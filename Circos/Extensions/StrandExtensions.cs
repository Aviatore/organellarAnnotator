using System;
using circos.Enums;

namespace circos.Extensions
{
    public static class StrandExtensions
    {
        public static string GetString(this Strand strand)
        {
            return strand switch
            {
                Strand.Minus => "Minus",
                Strand.Plus => "Plus",
                _ => throw new ArgumentOutOfRangeException(nameof(strand), "Wrong strand type")
            };
        }
    }
}