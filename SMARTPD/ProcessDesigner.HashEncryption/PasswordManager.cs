using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.HashEncryption
{
    public class PasswordManager
    {
        SHAHashing m_shahashing = new SHAHashing();

        //public string GeneratePasswordHash(string plainTextPassword, out string salt)
        //{
        //    salt = SaltGenerator.GetSaltString();

        //    string finalString = plainTextPassword + salt;

        //    return m_SHAHashing.GetPasswordHashAndSalt(finalString);
        //}

        public byte[] GeneratePasswordHash(string plainTextPassword, out byte[] salt)
        {
            string pwdsalt = SaltGenerator.GetSaltString();
            salt = Utility.GetBytes(pwdsalt);
            string finalString = plainTextPassword + pwdsalt;

            return m_shahashing.GetPasswordHashAndSalt2(finalString);
        }

        public bool IsPasswordMatch(string password, string salt, string hash)
        {
            string finalString = password + salt;
            return hash == m_shahashing.GetPasswordHashAndSalt(finalString);
        }

        public bool IsPasswordMatch(string password, byte[] pwdsalt, byte[] pwdHash)
        {
            string pwd = Utility.GetString(pwdHash);
            string salt = Utility.GetString(pwdsalt);

            string finalString = password + salt;
            return pwd == m_shahashing.GetPasswordHashAndSalt(finalString);
        }

    }
}
