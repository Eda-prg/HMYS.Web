using MediatR;
using System.Collections.Generic;

namespace HMYS.Web.Features.AnketGonderim.Commands
{
    public class SoruCevapDto
    {
        public int QuestionID { get; set; }
        public int Puan { get; set; }
        public string MetinCevap { get; set; }
    }

    public class AnketCevaplaCommand : IRequest<bool>
    {
        public string Token { get; set; }
        public List<SoruCevapDto> Cevaplar { get; set; }
    }
}