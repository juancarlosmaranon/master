﻿using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Helpers;
using System.Net;
using System.Net.Mail;

namespace MvcCoreUtilidades.Controllers
{
    public class MailsController : Controller
    {
        private HelperPathProvider helperPath;
        private IConfiguration configuration;

        public MailsController(HelperPathProvider helperPath, IConfiguration configuration)
        {
            this.helperPath = helperPath;
            this.configuration = configuration;
        }

        public IActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail
            (string para, string asunto, string mensaje, IFormFile file)
        {
            MailMessage mail = new MailMessage();
            //NECESITAMOS INDICAR DESDE QUE CUENTA ESTAMOS ENVIANDO
            string user = this.configuration.GetValue<string>
                ("MailSettings:Credentials:User");
            mail.From = new MailAddress(user);
            //LOS DESTINATARIOS SON UNA COLECCION
            mail.To.Add(new MailAddress(para));
            mail.Subject = asunto;
            mail.Body = mensaje;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            //COMPROBAMOS SI EXISTE UN FILE
            if(file != null)
            {
                //LOS HACEMOS LUEGO
            }
            //VAMOS A CONFIGURAR LAS CREDENCIALES PARA ENVIAR EL CORREO
            string password = this.configuration.GetValue<string>
                ("MailSettings:Credentials:Password");
            string hostName = this.configuration.GetValue<string>
                ("MailSettings:Smtp:Host");
            int port = this.configuration.GetValue<int>
                ("MailSettings:Smtp:Port");
            bool enableSSL = this.configuration.GetValue<bool>
                ("MailSettings:Smtp:EnableSSL");
            bool defaultCredentials = this.configuration.GetValue<bool>
                ("MailSettings:Smtp:EnableSSL");
            //CREAMOS EL CLIENTE SMTP
            SmtpClient client = new SmtpClient();
            client.Host = hostName;
            client.Port = port;
            client.EnableSsl = enableSSL;
            client.UseDefaultCredentials = defaultCredentials;
            NetworkCredential credentials =
                new NetworkCredential(user, password);
            client.Credentials = credentials;
            await client.SendMailAsync(mail);
            ViewData["MENSAJE"] = "Email enviado correctamente";
            return View();
        }
    }
}