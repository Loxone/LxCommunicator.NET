using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Loxone.Communicator {
	public static class Cryptography {
		/// <summary>
		/// Converts a byte array into a hex string
		/// </summary>
		/// <param name="input">The byte array that should be converted</param>
		/// <returns>The converted hex string</returns>
		public static string GetHexFromByteArray(byte[] input) {
			return BitConverter.ToString(input).Replace("-", "");
		}

		/// <summary>
		/// Gets a byte array from a hex string input
		/// </summary>
		/// <param name="hexInput">The hex string that should be converted</param>
		/// <returns>The converted byte array</returns>
		public static byte[] GetByteArrayFromHex(string hexInput) {
			byte[] data = new byte[hexInput.Length / 2];
			for (int index = 0; index < data.Length; index++) {
				string byteValue = hexInput.Substring(index * 2, 2);
				data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return data;
		}

		/// <summary>
		/// Encrypts a text using AES
		/// </summary>
		/// <param name="input">The text that should be encrypted</param>
		/// <param name="session">The session that should be used, provides the encryption key</param>
		/// <returns>The encrypted text</returns>
		public static string AesEncrypt(string input, Session session) {
			byte[] inputBytes = Encoding.UTF8.GetBytes(input);
			AesEngine engine = new AesEngine();
			CbcBlockCipher blockCipher = new CbcBlockCipher(engine);
			PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(blockCipher, new ZeroBytePadding());
			cipher.Init(true, new ParametersWithIV(new KeyParameter(GetByteArrayFromHex(session.AesKey)), GetByteArrayFromHex(session.AesIv)));
			byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
			int length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
			cipher.DoFinal(outputBytes, length);
			string encrypted = Convert.ToBase64String(outputBytes);
			return encrypted;
		}

		/// <summary>
		/// Decrypts a text using AES
		/// </summary>
		/// <param name="input">The text that should be decrypted</param>
		/// <param name="session">The session that should be used, provides the encryption key</param>
		/// <returns>The decrypted text</returns>
		public static string AesDecrypt(string input, Session session) {
			if (input == null) {
				return null;
			}
			byte[] inputBytes = Convert.FromBase64String(input);
			AesEngine engine = new AesEngine();
			CbcBlockCipher blockCipher = new CbcBlockCipher(engine);
			PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(blockCipher, new ZeroBytePadding());
			cipher.Init(false, new ParametersWithIV(new KeyParameter(GetByteArrayFromHex(session.AesKey)), GetByteArrayFromHex(session.AesIv)));
			byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
			int length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
			cipher.DoFinal(outputBytes, length);
			string decrypted = Encoding.UTF8.GetString(outputBytes);
			return decrypted;
		}

		/// <summary>
		/// Encrypts a text using RSA
		/// </summary>
		/// <param name="input">The text that should be encrypted</param>
		/// <param name="session">The session that should be used, provides the encryption key</param>
		/// <returns>The encrypted text</returns>
		public static async Task<string> EncryptRSA(string input, Session session) {
			byte[] encryptedData;
			string key = await session.GetMiniserverPublicKey();
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
				rsa.FromXmlString(key);
				encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.Pkcs1);
				return Convert.ToBase64String(encryptedData);
			}
		}
	}
}
