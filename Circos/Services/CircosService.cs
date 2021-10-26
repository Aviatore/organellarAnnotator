using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

        public async Task BlastReadAsync()
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

        public List<BlastOutput> GetBlastOutputs() => _blastOutputs;
    }
}