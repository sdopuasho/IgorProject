using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitcom_AccountingEquipment
{
    public class AuthorizationData
    {
        /// <summary>
        /// Блок проверки логина на необходимые символы
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        public bool LoginEnteringTextCheck(string Login)
        {
            if (Login.Length < 4 || Login.Length > 25)
                return false;
            if (!Login.Any(char.IsUpper))
                return false;
            if (!Login.Any(char.IsLower))
                return false;
            if (!Login.Any(char.IsDigit))
                return false;


            return true;
        }
        /// <summary>
        /// Блок проверки пароля на необходимые символы
        /// </summary>
        public bool PasswordEnteringTextCheck(string Password)
        {
            if (Password.Length < 0 || Password.Length > 25)
                return false;
            if (!Password.Any(char.IsUpper))
                return false;
            if (!Password.Any(char.IsLower))
                return false;
            if (!Password.Any(char.IsDigit))
                return false;


            return true;
        }
    }
}
