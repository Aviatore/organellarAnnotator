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
                    var fileNameHighlights = $"{strand.ToString().ToLower()}_{geneType.ToString().ToLower()}_highlights";
                    var fileNameGeneNames = $"{strand.ToString().ToLower()}_{geneType.ToString().ToLower()}_names";
                    var fileConnectors = $"{strand.ToString().ToLower()}_{geneType.ToString().ToLower()}_connectors";
                    
                    await using var swHighlights = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Data", fileNameHighlights));
                    await using var swNames = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Data", fileNameGeneNames));
                    await using var swConnectors = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Data", fileConnectors));
                    
                    var filteredBlastOutput = _blastOutputs.Where(p => p.Strand == strand && p.GeneType == geneType);
                    foreach (var blastOutput in filteredBlastOutput)
                    {
                        await swHighlights.WriteLineAsync(
                            $"{blastOutput.CircosId}\t{blastOutput.QueryStart}\t{blastOutput.QueryStop}");
                        await swNames.WriteLineAsync(
                            $"{blastOutput.CircosId}\t{blastOutput.QueryStart}\t{blastOutput.QueryStop}\t{blastOutput.SubjectId}");
                    }
                }
            }
        }

        public void GetConnectors(StreamWriter sw, BlastOutput blastOutput)
        {
            
        }

        public List<BlastOutput> GetBlastOutputs() => _blastOutputs;
    }
}