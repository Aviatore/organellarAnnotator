using System;
using System.Collections.Generic;

namespace circos.Models
{
    public class ChromCounter
    {
        private Dictionary<string, int> _circosIds;
        private int _chromNumber;

        public ChromCounter()
        {
            _chromNumber = 0;
            _circosIds = new Dictionary<string, int>();
        }

        public string AddChrom(string chromName)
        {
            if (!_circosIds.ContainsKey(chromName))
            {
                _chromNumber++;
                _circosIds.Add(chromName, _chromNumber);
            }

            return $"h{_circosIds[chromName]}";
        }

        public int GetChrom(string chromName) => _circosIds[chromName];
    }
}