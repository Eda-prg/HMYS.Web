using System;
using System.Collections.Generic;
using System.Linq;
using HMYS.Business.Interfaces;

namespace HMYS.Business
{
    public class AnalyticsManager : IAnalyticsManager
    {
        public double CalculateAverageScore(List<int> scores)
        {
            if (scores == null || scores.Count == 0) return 0;
            return Math.Round(scores.Average(), 2);
        }

        public double CalculateSatisfactionRate(double averageScore)
        {
            return (averageScore / 5) * 100;
        }
    }
}