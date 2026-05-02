using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using HMYS.BUsiness.Interfaces;
using HMYS.BUsiness.Models;


namespace HMYS.BUsiness.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendSurveyLinkAsync(string toEmail, string hastaAdi, string token)
        {
            try
            {
                // Anket linki — ileride kendi domain'inle değiştir
                var anketLink = $"https://uygulamaniz.com/anket?token={token}";

                // E-posta içeriği (HTML)
                var htmlBody = $@"
                 <html>
                 <body style='font-family: Arial; background: #f5f5f5; padding: 20px;'>
                 <div style='max-width: 600px; margin: auto; background: white; 
                 border-radius: 12px; padding: 32px;'>
        
                  <h2 style='color: #16A34A;'>Hasta Memnuniyet Anketi</h2>
        
                  <p>Sayın <strong>{hastaAdi}</strong>,</p>
        
                   <p>Hastanemizden aldığınız hizmeti değerlendirmeniz için 
                     sizi kısa bir ankete davet ediyoruz.</p>
        
                    <p>Anket sadece <strong>2-3 dakika</strong> sürmektedir.</p>

                  <p>Ankete katılmak için aşağıdaki kodu uygulamaya giriniz:</p>

                     <div style='background: #f0fdf4; border: 2px dashed #16A34A; 
                    border-radius: 8px; padding: 20px; text-align: center; margin: 20px 0;'>
                       <p style='color: #64748b; font-size: 13px; margin: 0 0 8px 0;'>
                            Anket Kodunuz (Token)
                            </p>
                            <p style='color: #16A34A; font-size: 22px; font-weight: bold; 
                      letter-spacing: 2px; margin: 0; font-family: monospace;'>
                {token}
                      </p>
                       </div>
        
                           <p style='color: #888; font-size: 12px;'>
                          Bu kod 48 saat geçerlidir.<br/>
                           Kodu uygulamadaki token alanına yapıştırınız.
                       </p>
        
                        </div>
                         </body>
                           </html>";
                // SMTP ayarları
                var smtpClient = new SmtpClient(_settings.SmtpHost)
                {
                    Port = _settings.SmtpPort,
                    Credentials = new NetworkCredential(
                        _settings.SenderEmail,
                        _settings.SenderPassword
                    ),
                    EnableSsl = true,
                };

                // E-posta mesajı
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
                    Subject = "Hasta Memnuniyet Anketi — Görüşünüz Bizim İçin Değerli",
                    Body = htmlBody,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Hata logla (G12 - Audit Trail)
                Console.WriteLine($"E-posta gönderilemedi: {ex.Message}");
                return false;
            }
        }
    }
}
    
