using ProyectoGrupo5.Models;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;

namespace ProyectoGrupo5.Service
{
    public class EmailServices 
    {
        private readonly IConfiguration config;
        public EmailServices(IConfiguration _config)
        {
            config = _config;
        }
        public void sendEmail(string Para,string NombreFactura)
        { 
            string Asunto="Pago Realizado"; 
            string Contenido = "Gracias Por Comprar En FrutaVerde.com";
            var rutaAdjunto = "Facturas/"+ NombreFactura + ".pdf";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(Para));
            email.Subject = Asunto;

            var builder = new BodyBuilder();
            builder.HtmlBody = Contenido;

            var attachment = builder.Attachments.Add(rutaAdjunto);

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(
                config.GetSection("Email:Host").Value,
                Convert.ToInt32(config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );

            smtp.Authenticate(config.GetSection("Email:UserName").Value, config.GetSection("Email:PassWord").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }



    }
}
