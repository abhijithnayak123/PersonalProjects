using System;

namespace MGI.Common.DataProtection.Contract
{
	public interface IDataProtectionService
	{
		/// <summary>
		/// Encrypts a string
		/// </summary>
		/// <param name="data">string to be encrypted</param>
		/// <param name="slot">'slot' used for encryption (0 = current, 1 = future)</param>
		/// <returns>encrypted string</returns>
		string Encrypt(string data, int slot);

		/// <summary>
		/// Decrypts a cipher text encrypted using IDataProtectionService.Encrypt
		/// </summary>
		/// <param name="cipherText">the encrypted string</param>
		/// <param name="slot">'slot' used for encryption (0 = current, 1 = future)</param>
		/// <returns>decrypted string</returns>
		string Decrypt(string cipherText, int slot);
	}
}
