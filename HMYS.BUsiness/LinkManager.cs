using HMYS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public class LinkManager
    {
        public string GenerateSecureToken()
        {
            return Guid.NewGuid().ToString();
        }
        public DateTime GetExpirationDate()
        {
            return DateTime.Now.AddHours(48);
        }
        public string CreateSurveyUrl(string baseUrl, string token)
        {
            return $"{baseUrl}/anket/katil?t={token}";
        }
        public string CreateSurveyToken()
        {
            return Guid.NewGuid().ToString(); // "550e8400-e29b-..." gibi bir kod üretir
        }
        public bool IsTokenValid(string token, List<Survey> activeSurveys)
        {
            var survey = activeSurveys.FirstOrDefault(s => s.GuidToken == token);
            return survey != null && !survey.IsUsed && survey.ExpireDate > DateTime.Now;
        }
    }
}
