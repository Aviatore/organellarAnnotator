using System;
using System.IO;
using circos.Extensions;
using circos.Services;

namespace circos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var cs = new CircosService(Path.Combine(Directory.GetCurrentDirectory(), "Data/blast_out.txt"));
            cs.BlastReadAsync().Wait();

            foreach (var blastOutput in cs.GetBlastOutputs())
            {
                Console.Out.WriteLine($"{blastOutput.QueryId} ::: {blastOutput.SubjectId} ::: {blastOutput.Strand.GetString()}");
            }
        }
    }
}