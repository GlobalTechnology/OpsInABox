using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace AgapeEncryption
{
	public class ADCEncrypt
	{
		public static string Decrypt(string val)
		{
            byte[] SharedKey = { 204, 3, 86, 175, 154, 132, 65, 229, 87, 43, 89, 195, 132, 41, 77, 61 };
            byte[] sharedvector = { 152, 198, 32, 65, 99, 111, 234, 132, 76, 48, 32, 78, 165, 84, 32, 5 };

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			byte[] toDecrypt = Convert.FromBase64String(val);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, tdes.CreateDecryptor(SharedKey, sharedvector), CryptoStreamMode.Write);
			cs.Write(toDecrypt, 0, toDecrypt.Length);
			cs.FlushFinalBlock();
			return Encoding.UTF8.GetString(ms.ToArray());
		}

		public static string Encrypt(string val)
		{
			byte[] SharedKey = { 204,3,86,175,154,132,65,229,87,43,89,195,132,41,77,61 };
			byte[] sharedvector = { 152,198,32,65,99,111,234,132,76,48,32,78,165,84,32,5 };

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			byte[] toEncrypt = Encoding.UTF8.GetBytes(val);
			MemoryStream ms = new MemoryStream();
			CryptoStream cs = new CryptoStream(ms, tdes.CreateEncryptor(SharedKey, sharedvector), CryptoStreamMode.Write);

			cs.Write(toEncrypt, 0, toEncrypt.Length);
			cs.FlushFinalBlock();
			return Convert.ToBase64String(ms.ToArray());
		}
	}
}
