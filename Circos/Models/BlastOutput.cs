using System;
using System.Collections.Generic;
using System.Linq;
using circos.Enums;

namespace circos.Models
{
    public class BlastOutput
    {
        public string QueryId { get; }
        public string CircosId { get; }
        public int QueryStart { get; set; }
        public int QueryStop { get; set; }
        public string SubjectId { get; }
        public int SubjectStart { get; }
        public int SubjectStop { get; }
        public int IdenticalMatchesCount { get; }
        public int AlignmentLength { get; }
        public Strand Strand { get; }
        public GeneType GeneType { get; }
        public int Middle { get; set; }

        public BlastOutput(string[] input, ChromCounter circosIds)
        {
            if (input.Length != 10)
            {
                throw new ArgumentOutOfRangeException(nameof(input), "The input line does not contain 10 elements");
            }

            QueryId = input[0];

            CircosId = circosIds.AddChrom(QueryId);

            QueryStart = int.TryParse(input[1], out int queryStart)
                ? queryStart
                : throw new ArgumentException($"The value: {input[1]} is not a number");
            QueryStop = int.TryParse(input[2], out int queryStop)
                ? queryStop
                : throw new ArgumentException($"The value: {input[2]} is not a number");
            SubjectId = input[3];
            SubjectStart = int.TryParse(input[4], out int subjectStart)
                ? subjectStart
                : throw new ArgumentException($"The value: {input[4]} is not a number");
            SubjectStop = int.TryParse(input[5], out int subjectStop)
                ? subjectStop
                : throw new ArgumentException($"The value: {input[5]} is not a number");
            IdenticalMatchesCount = int.TryParse(input[6], out int identicalMatchesCount)
                ? identicalMatchesCount
                : throw new ArgumentException($"The value: {input[6]} is not a number");
            AlignmentLength = int.TryParse(input[7], out int alignmentLength)
                ? alignmentLength
                : throw new ArgumentException($"The value: {input[7]} is not a number");
            Strand = input[9] switch
            {
                "plus" => Strand.Plus,
                "minus" => Strand.Minus,
                _ => throw new ArgumentOutOfRangeException(nameof(input), "The last column is not a strand name")
            };

            if (SubjectId.Contains("rrn"))
            {
                GeneType = GeneType.Rrna;
            }
            else if (SubjectId.Contains("trn"))
            {
                GeneType = GeneType.Trna;
            }
            else
            {
                GeneType = GeneType.Protein;
            }
            
            Middle = ((QueryStop - QueryStart) / 2) + QueryStart;
        }
    }
}