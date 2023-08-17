using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Sercurity
{
    public class PKCSKeyGenerator
    {
        public static int COUNT = 20;
        public static sbyte[] SALT = { -57, 115, 33, -116, 126, -56, -18, -103 };
        /// <summary>
        /// Key used in the encryption algorythm.
        /// </summary>

        private sbyte[] key = new sbyte[8];

        /// <summary>
        /// IV used in the encryption algorythm.
        /// </summary>
        private sbyte[] iv = new sbyte[8];

        /// <summary>
        /// DES Provider used in the encryption algorythm.
        /// </summary>
        private DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        /// <summary>
        /// Initializes a new instance of the PKCSKeyGenerator class.
        /// </summary>
        public PKCSKeyGenerator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PKCSKeyGenerator class.
        /// </summary>
        /// <param name="keystring">This is the same as the "password" of the PBEWithMD5AndDES method.</param>
        /// <param name="salt">This is the salt used to provide extra security to the algorythim.</param>
        /// <param name="iterationsMd5">Fill out iterationsMd5 later.</param>
        /// <param name="segments">Fill out segments later.</param>
        public PKCSKeyGenerator(string keystring, sbyte[] salt, int iterationsMd5, int segments)
        {
            this.Generate(keystring, salt, iterationsMd5, segments);
        }

        /// <summary>
        /// Gets the asymetric Key used in the encryption algorythm.  Note that this is read only and is an empty byte array.
        /// </summary>
        public sbyte[] Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Gets the initialization vector used in in the encryption algorythm.  Note that this is read only and is an empty byte array.
        /// </summary>
        public sbyte[] IV
        {
            get
            {
                return this.iv;
            }
        }

        /// <summary>
        /// Gets an ICryptoTransform interface for encryption
        /// </summary>
        public ICryptoTransform Encryptor
        {
            get
            {
                byte[] key = Array.ConvertAll(this.key, b => unchecked((byte)b));
                byte[] iv = Array.ConvertAll(this.iv, b => unchecked((byte)b));
                return this.des.CreateEncryptor(key, iv);
            }
        }

        /// <summary>
        /// Gets an ICryptoTransform interface for decryption
        /// </summary>
        public ICryptoTransform Decryptor
        {
            get
            {
                byte[] key = Array.ConvertAll(this.key, b => unchecked((byte)b));
                byte[] iv = Array.ConvertAll(this.iv, b => unchecked((byte)b));
                return des.CreateDecryptor(key, iv);
            }
        }

        /// <summary>
        /// Returns the ICryptoTransform interface used to perform the encryption.
        /// </summary>
        /// <param name="keystring">This is the same as the "password" of the PBEWithMD5AndDES method.</param>
        /// <param name="salt">This is the salt used to provide extra security to the algorythim.</param>
        /// <param name="iterationsMd5">Fill out iterationsMd5 later.</param>
        /// <param name="segments">Fill out segments later.</param>
        /// <returns>ICryptoTransform interface used to perform the encryption.</returns>
        public ICryptoTransform Generate(string keystring, sbyte[] salt, int iterationsMd5, int segments)
        {
            // MD5 bytes
            int hashLength = 16;

            // to store contatenated Mi hashed results
            sbyte[] keyMaterial = new sbyte[hashLength * segments];

            // --- get secret password bytes ----
            sbyte[] passwordBytes = Array.ConvertAll(Encoding.UTF8.GetBytes(keystring), b => unchecked((sbyte)b));
            // --- contatenate salt and pswd bytes into fixed data array ---
            sbyte[] data00 = new sbyte[passwordBytes.Length + salt.Length];

            // copy the pswd bytes
            Array.Copy(passwordBytes, data00, passwordBytes.Length);

            // concatenate the salt bytes
            Array.Copy(salt, 0, data00, passwordBytes.Length, salt.Length);

            // ---- do multi-hashing and contatenate results  D1, D2 ...  into keymaterial bytes ----
            MD5 md5 = new MD5CryptoServiceProvider();
            sbyte[] result = null;

            // fixed length initial hashtarget
            sbyte[] hashtarget = new sbyte[hashLength + data00.Length];

            for (int j = 0; j < segments; j++)
            {
                // ----  Now hash consecutively for iterationsMd5 times ------
                if (j == 0)
                {
                    // initialize
                    result = data00;
                }
                else
                {
                    Array.Copy(result, hashtarget, result.Length);
                    Array.Copy(data00, 0, hashtarget, result.Length, data00.Length);
                    result = hashtarget;
                }

                for (int i = 0; i < iterationsMd5; i++)
                {
                    byte[] r = Array.ConvertAll(result, b => unchecked((byte)b));
                    byte[] r1 = md5.ComputeHash(r);
                    result = Array.ConvertAll(r1, b => unchecked((sbyte)b));
                }

                // contatenate to keymaterial
                Array.Copy(result, 0, keyMaterial, j * hashLength, result.Length);
            }

            Array.Copy(keyMaterial, 0, this.key, 0, 8);
            Array.Copy(keyMaterial, 8, this.iv, 0, 8);

            return this.Encryptor;
        }

        private ICryptoTransform _decryptorCryptoTransform;
        public ICryptoTransform DecryptorCryptoTransform
        {
            get
            {
                return _decryptorCryptoTransform == null ? _decryptorCryptoTransform = Decryptor : _decryptorCryptoTransform;
            }
        }

        public string Decrypt(sbyte[] cipherBytes)
        {
            byte[] clearBytes = DecryptorCryptoTransform.TransformFinalBlock((byte[])(Array)cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(clearBytes);
        }
    }
}
