using ASP.NET.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Quartz;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace ASP.NET.Quartz
{
    public class EmailSender(TestContext _context, IConfiguration _configuration) : IJob
    {
        public async Task Execute(IJobExecutionContext _jobContext)
        {
            var sendings = await _context.Sendings.ToListAsync();
            foreach (var sending in sendings)
            {
                try
                {
                    await SendEmailAsync(sending.Email, $"New post {sending.Title} in community {sending.Community}");

                    _context.Sendings.Remove(sending);
                }
                catch (Exception ex)
                {

                Console.WriteLine($"Failed to send one email ({sending.Email}). Retrying in 5 minutes");
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SendEmailAsync(string subscriberEmail, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(smtpSettings["SenderName"], smtpSettings["SenderEmail"])
            );
            message.To.Add(new MailboxAddress(subscriberEmail, subscriberEmail));
            message.Subject = "New post published";
            message.Body = new TextPart("html") { Text = body };

            var client = new SmtpClient();
            await client.ConnectAsync(
                smtpSettings["Server"],
                int.Parse(smtpSettings["Port"] ?? string.Empty),
                false
            );

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
