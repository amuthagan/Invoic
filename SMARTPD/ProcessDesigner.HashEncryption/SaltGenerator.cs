using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.HashEncryption
{
    /// <summary>
    /// Salt Generator Class
    /// </summary>
    public static class SaltGenerator
    {
        /// <summary>
        /// m_crypto service provider and constat salt size
        /// </summary>
        private static RNGCryptoServiceProvider m_cryptoServiceProvider = null;
        private const int SALT_SIZE = 24;

        /// <summary>
        /// Initializes the <see cref="SaltGenerator"/> class.
        /// </summary>
        static SaltGenerator()
        {
            m_cryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Return the salt for SHA hashing.
        /// </summary>
        /// <returns></returns>
        public static string GetSaltString()
        {
            // Lets create a byte array to store the salt bytes
            byte[] saltBytes = new byte[SALT_SIZE];

            // lets generate the salt in the byte array
            m_cryptoServiceProvider.GetNonZeroBytes(saltBytes);

            // Let us get some string representation for this salt
            string saltString = Utility.GetString(saltBytes);

            // Now we have our salt string ready lets return it to the caller
            return saltString;
        }
    }
}
