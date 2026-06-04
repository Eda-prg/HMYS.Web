using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.Business.Interfaces
{
    public interface IAnalyticsManager
    {
        double CalculateAverageScore(List<int> scores);
        double CalculateSatisfactionRate(double averageScore);
    }
}
