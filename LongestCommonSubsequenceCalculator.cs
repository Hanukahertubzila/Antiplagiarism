﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var opt = CreateOptimizationTable(first, second);
        return RestoreAnswer(opt, first, second);
    }

    private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
    {
        var opt = new int[first.Count + 1, second.Count + 1];
        for (int i = 1; i < first.Count + 1; i++)
            for (int j = 1; j < second.Count + 1; j++)
            {
                if (first[i - 1] == second[j - 1])
                    opt[i, j] = opt[i - 1, j - 1] + 1;
                else
                {
                    var max = new List<int>() { opt[i - 1, j - 1], opt[i, j - 1], opt[i - 1, j] };
                    opt[i, j] = max.Max();
                }
            }
        return opt;
    }

    private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
    {
        var result = new List<string>();
        int i = opt.GetLength(0) - 1;
        int j = opt.GetLength(1) - 1;
        while (i > 0 && j > 0)
        {
            if (first[i - 1] == second[j - 1])
            {
                result.Add(first[i - 1]);
                i--;
                j--;
            }
            else
            {
                if (opt[i - 1, j] > opt[i, j - 1])
                    i--;
                else
                    j--;
            }
        }
        result.Reverse();
        return result;
    }
}