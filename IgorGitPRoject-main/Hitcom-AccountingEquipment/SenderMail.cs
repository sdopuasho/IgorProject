using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace Hitcom_AccountingEquipment
{
    
    public class SenderMail
    {
        public static int IntId { get; set; }
        public static string PositionName { get; set; }
        public static Visibility DelVisibility { get; set; }

        string All = "qwertyuiopasdfghjklzxcvbnm1234567890", result;
        /// <summary>
        /// Блок генерации пароля для почты
        /// </summary>
        /// <returns></returns>
        public string SenderCode()
        {
            result = "";
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                result += All[rnd.Next(All.Length)];
            }
            return result;
        }
        /// <summary>
        /// Блок создания сообщения на почту 
        /// Он принимает в себя пароль сгенерированный ранее в другом окне и почту из бд
        /// После чего создается тело сообщения
        /// SMTP клиент с указанием хоста, порта и других параметров, в том числе и данных для доступа к почте
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool senderMAil(string email, string code)
        {
            if (!string.IsNullOrEmpty(email))
            {
                string htmlString = @"<html>
                      <body>
                      <p></p>
                      <p> Ваш код для авторизации: " + code + @"</p></body>
                      <p>С уважением,<br>-Hitcom</br></p>
                      </body>
                      </html>
                     ";
                SmtpClient client = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = "hitcomlog@gmail.com",
                        Password = "qwenqwen1"
                    }

                };
                MailAddress fromEmail = new MailAddress("hitcomlog@gmail.com");
                MailAddress toemail = new MailAddress(email);
                MailMessage Message = new MailMessage()
                {
                    From = fromEmail,
                    Subject = "Код для авторизации",
                    IsBodyHtml = true,
                    Body = htmlString
                };
                Message.To.Add(toemail);
                try
                {
                    client.Send(Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("wrong" + ex.InnerException.Message, "error");
                }
                return true;
            }
            else
            {
                return false;
            }
        
        }
    }
}
