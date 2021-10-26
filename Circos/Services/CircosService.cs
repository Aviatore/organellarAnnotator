using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using circos.Enums;
using circos.Models;

namespace circos.Services
{
    public class CircosService
    {
        private List<BlastOutput> _blastOutputs;
        private string[] _blastOutputFilePaths;
        private ChromCounter _chromCounter;

        public CircosService(params string[] blastOutputFilePaths)
        {
            _blastOutputs = new List<BlastOutput>();
            _chromCounter = new ChromCounter();
            _blastOutputFilePaths = blastOutputFilePaths;

            foreach (var filePath in blastOutputFilePaths)
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File {filePath} does not exists");
                }
            }
        }

        public async Task BlastOutputReadAsync()
        {
            foreach (var filePath in _blastOutputFilePaths)
            {
                using var file = new StreamReader(filePath);

                var line = (await file.ReadLineAsync())?.Trim().Split('\t');
                while (line is not null)
                {
                    var blastOutput = new BlastOutput(line, _chromCounter);
                    _blastOutputs.Add(blastOutput);
                    
                    line = (await file.ReadLineAsync())?.Trim().Split('\t');
                }
            }
        }

        public async Task GetHighlights()
        {
            foreach (var geneType in Enum.GetValues(typeof(GeneType)).Cast<GeneType>())
            {
                foreach (var strand in Enum.GetValues(typeof(Strand)).Cast<Strand>())
                {
                    var fileName = $"{strand.ToString().ToLower()}_{geneType.ToString().ToLower()}_highlights";
                    using var sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName));
                    var filteredBlastOutput = _blastOutputs.Where(p => p.Strand == strand && p.GeneType == geneType);
                    foreach (var blastOutput in filteredBlastOutput)
                    {
                        await sw.WriteLineAsync(
                            $"{blastOutput.CircosId}\t{blastOutput.QueryStart}\t{blastOutput.QueryStop}");
                    }
                }
            }
        }

        public List<BlastOutput> GetBlastOutputs() => _blastOutputs;
    }
}