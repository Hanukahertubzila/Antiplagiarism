using System.Collections.Generic;
using System.Linq;

using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
    {
        var result = new List<ComparisonResult>();
        for (int i = 0; i < documents.Count; i++)
            for (int j = i + 1; j < documents.Count; j++)
                result.Add(new ComparisonResult(documents[i], documents[j], GetLevenshteinDistance(documents[i], documents[j])));
        return result;
    }

    private double GetLevenshteinDistance(DocumentTokens first, DocumentTokens second)
    {
        var previous = new double[second.Count + 1];
        var current = new double[second.Count + 1];
        for (int i = 0; i < previous.Length; i++)
            previous[i] = i;
        current[0] = 1;
        for (int i = 1; i < first.Count + 1; i++)
        {
            if (i != 1)
                SwapLines(current, previous, i);
            for (int j = 1; j < second.Count + 1; j++)
            {
                if (first[i - 1] == second[j - 1])
                    current[j] = previous[j - 1];
                else
                {
                    var options = new List<double>()
                    {
                        current[j - 1] + 1,
                        previous[j] + 1,
                        previous[j - 1] + TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1])
                    };
                    current[j] = options.Min();
                }
            }
        }
        return current[current.Length - 1];
    }

    private void SwapLines(double[] current, double[] previous, int n)
    {
        current.CopyTo(previous, 0);
        for (int k = 0; k < current.Length; k++)
            current[k] = 0;
        current[0] = n;
    }
}