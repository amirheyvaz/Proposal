using Proposal.Core;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;


namespace Proposal.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Authentication")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("GetToken/{username}/{password}")]
        public string GetToken(string username, string password)
        {
            //string pass = AESEncryption.Decrypt(password);
            if (CheckUser(username, password))
            {
                return JwtManager.GenerateToken(username);
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        private bool CheckUser(string username, string password)
        {
            // should check in the database
            var StudentRepository = IocConfig.Container.GetInstance<IStudentRepository>();
            var ProfessorRepository = IocConfig.Container.GetInstance<IProfessorRepository>();
            if (StudentRepository.AuthenticateStudent(username, password) || ProfessorRepository.AuthenticateProfessor(username , password))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

    }

    public class AESEncryption
    {
        const string Key = "ABCDEFGHJKLMNOPQRSTUVWXYZABCDEF"; // must be 32 character
        const string IV = "ABCDEFGHIJKLMNOP"; // must be 16 character
        public static string Encrypt(string message)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = UTF8Encoding.UTF8.GetBytes(IV);
            aes.Key = UTF8Encoding.UTF8.GetBytes(Key);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(dest);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                byte[] cipherText = Convert.FromBase64String(encryptedText);
                byte[] aesIV = UTF8Encoding.UTF8.GetBytes(IV);
                byte[] aesKey = UTF8Encoding.UTF8.GetBytes(Key);
                ICryptoTransform decryptor = aes.CreateDecryptor(aesKey, aesIV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}
