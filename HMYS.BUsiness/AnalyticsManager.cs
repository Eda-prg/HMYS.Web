using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public class AnalyticsManager
    {
        public double CalculateAverageScore(List<int> scores)
        {
            if (scores == null || scores.Count == 0) return 0;
            return Math.Round(scores.Average(), 2);
        }

        // Memnuniyet yüzdesini hesaplar (5 üzerinden)
        public double CalculateSatisfactionRate(double averageScore)
        {
            return (averageScore / 5) * 100;
        }
    }
}
