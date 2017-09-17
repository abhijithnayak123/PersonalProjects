using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Microsoft.Win32;

using TCF.Zeo.Common.DataProtection.Contract;

namespace TCF.Zeo.Common.DataProtection.Impl
{
	public sealed class DataProtectionService : IDataProtectionService
	{
		// use local machine or user to encrypt and decrypt the data
		public enum Store
		{
			Machine,
			User
		}

		// const values
		private class Consts 
		{
			// specify an entropy so other DPAPI applications can't see the data
			public readonly static byte[] EntropyData = ASCIIEncoding.ASCII.GetBytes("B0D125B7-967E-4f94-9305-A6F9AF56A19A");
		}

		// public methods

		// encrypt the data using DPAPI, returns a base64-encoded encrypted string
		public string Encrypt(string data, Store store)
		{
			// holds the result string
			string  result = "";

			// blobs used in the CryptProtectData call
			Win32.DATA_BLOB inBlob = new Win32.DATA_BLOB();
			Win32.DATA_BLOB entropyBlob = new Win32.DATA_BLOB();
			Win32.DATA_BLOB outBlob = new Win32.DATA_BLOB();

			try 
			{
				// setup flags passed to the CryptProtectData call
				int flags = Win32.CRYPTPROTECT_UI_FORBIDDEN | 
					(int)((store == Store.Machine) ? Win32.CRYPTPROTECT_LOCAL_MACHINE : 0);

				// setup input blobs, the data to be encrypted and entropy blob
				SetBlobData(ref inBlob, ASCIIEncoding.ASCII.GetBytes(data));
				SetBlobData(ref entropyBlob, Consts.EntropyData);

				// call the DPAPI function, returns true if successful and fills in the outBlob
				if (Win32.CryptProtectData(ref inBlob, "", ref entropyBlob, IntPtr.Zero, IntPtr.Zero, flags, ref outBlob)) 
				{
					byte[] resultBits = GetBlobData(ref outBlob);
					if (resultBits != null) 
						result = Convert.ToBase64String(resultBits);
				}
			}
			catch
			{
				// an error occurred, return an empty string
			}
			finally 
			{
				// clean up
				if (inBlob.pbData.ToInt32() != 0) 
					Marshal.FreeHGlobal(inBlob.pbData);

				if (entropyBlob.pbData.ToInt32() != 0) 
					Marshal.FreeHGlobal(entropyBlob.pbData);
			}

			return result;
		}

		// decrypt the data using DPAPI, data is a base64-encoded encrypted string
		public string Decrypt( string  data,  Store store) 
		{
			// holds the result string
			string result = "";

			// blobs used in the CryptUnprotectData call
			Win32.DATA_BLOB inBlob = new Win32.DATA_BLOB();
			Win32.DATA_BLOB entropyBlob = new Win32.DATA_BLOB();
			Win32.DATA_BLOB outBlob = new Win32.DATA_BLOB();

			try 
			{
				// setup flags passed to the CryptUnprotectData call
				int flags = Win32.CRYPTPROTECT_UI_FORBIDDEN |
					(int)((store == Store.Machine) ? Win32.CRYPTPROTECT_LOCAL_MACHINE : 0);

				// the CryptUnprotectData works with a byte array, convert string data
				byte[] bits = Convert.FromBase64String(data);

				// setup input blobs, the data to be decrypted and entropy blob
				SetBlobData(ref inBlob, bits);
				SetBlobData(ref entropyBlob, Consts.EntropyData);

				// call the DPAPI function, returns true if successful and fills in the outBlob
				if (Win32.CryptUnprotectData(ref inBlob, null, ref entropyBlob, IntPtr.Zero, IntPtr.Zero, flags, ref outBlob)) 
				{
					byte[] resultBits = GetBlobData(ref outBlob);
					if (resultBits != null) 
						result = ASCIIEncoding.ASCII.GetString(resultBits);
				}
			}
			catch 
			{
				// an error occurred, return an empty string
			}
			finally 
			{
				// clean up
				if (inBlob.pbData.ToInt32() != 0) 
					Marshal.FreeHGlobal(inBlob.pbData);
		
				if (entropyBlob.pbData.ToInt32() != 0) 
					Marshal.FreeHGlobal(entropyBlob.pbData);
			}

			return result;
		}


		// internal methods

		#region Data Protection API

		private class Win32 
		{
			public const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
			public const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;

			[StructLayout(LayoutKind.Sequential)]
				public struct DATA_BLOB
			{
				public int cbData;
				public IntPtr pbData;
			}

			[DllImport("crypt32", CharSet=CharSet.Auto)]
			public static extern bool CryptProtectData(ref DATA_BLOB pDataIn, string szDataDescr, ref DATA_BLOB pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

			[DllImport("crypt32", CharSet=CharSet.Auto)]
			public static extern bool CryptUnprotectData(ref DATA_BLOB pDataIn, StringBuilder szDataDescr, ref DATA_BLOB pOptionalEntropy, IntPtr pvReserved, IntPtr pPromptStruct, int dwFlags, ref DATA_BLOB pDataOut);

			[DllImport("kernel32")] 
			public static extern IntPtr LocalFree(IntPtr hMem);
		}

		#endregion

		// helper method that fills in a DATA_BLOB, copies 
		// data from managed to unmanaged memory
		private void SetBlobData(ref Win32.DATA_BLOB blob,  byte[] bits) 
		{
			blob.cbData = bits.Length;
			blob.pbData = Marshal.AllocHGlobal(bits.Length);
			Marshal.Copy(bits, 0, blob.pbData, bits.Length);
		}

		// helper method that gets data from a DATA_BLOB, 
		// copies data from unmanaged memory to managed
		private byte[] GetBlobData(ref Win32.DATA_BLOB blob) 
		{
			// return an empty string if the blob is empty
			if (blob.pbData.ToInt32() == 0) 
				return null;

			// copy information from the blob
			byte[] data = new byte[blob.cbData];
			Marshal.Copy(blob.pbData, data, 0, blob.cbData);
			Win32.LocalFree(blob.pbData);

			return data;
		}

		public string Encrypt( string data, int slot = 0 )
		{
			// Check arguments.
			if ( string.IsNullOrEmpty( data ) )
				throw new ArgumentException( "invalid data" );
			if ( slot < 0 || slot > 1 )
				throw new ArgumentException( "slot must be 0 or 1" );

			// Read the DEK and IV from the registry
			string _DEK = null;
			string _IV = null;

			try
			{
				_DEK = Decrypt( (string)Registry.LocalMachine.OpenSubKey( "SOFTWARE" ).OpenSubKey( "NEXXO" ).GetValue( string.Format( "DEK{0}", slot ) ), Store.Machine );
			}
			catch ( Exception ex )
			{
				throw new SecurityException( string.Format( "HKLM.SOFTWRE.NEXXO.DEK{0} is missing or invalid: " + ex.Message, slot ) );
			}
			try
			{
				_IV = Decrypt( (string)Registry.LocalMachine.OpenSubKey( "SOFTWARE" ).OpenSubKey( "NEXXO" ).GetValue( string.Format( "IV{0}", slot ) ), Store.Machine );
			}
			catch ( Exception ex )
			{
				throw new SecurityException( string.Format( "HKLM.SOFTWRE.NEXXO.IV{0} is missing or invalid: " + ex.Message, slot ) );
			}

			if ( _DEK == null || _DEK.Length != 44 )
				throw new ArgumentNullException( string.Format( "Invalid DEK{0}", slot ) );
			if ( _IV == null || _IV.Length != 24 )
				throw new ArgumentNullException( string.Format( "Invalid IV{0}", slot ) );

			byte[] _Encrypted;

			// Create an AesManaged object with the specified key and IV. 
			using ( AesManaged _AES = new AesManaged() )
			{
				_AES.Key = Convert.FromBase64String( _DEK );
				_AES.IV = Convert.FromBase64String( _IV );

				// Create a decrytor to perform the stream transform.
				ICryptoTransform _Encryptor = _AES.CreateEncryptor( _AES.Key, _AES.IV );

				// Create the streams used for encryption. 
				using ( MemoryStream msEncrypt = new MemoryStream() )
				{
					using ( CryptoStream csEncrypt = new CryptoStream( msEncrypt, _Encryptor, CryptoStreamMode.Write ) )
					{
						using ( StreamWriter swEncrypt = new StreamWriter( csEncrypt ) )
						{

							//Write all data to the stream.
							swEncrypt.Write( data );
						}
						_Encrypted = msEncrypt.ToArray();
					}
				}
			}

			// Return the encrypted bytes from the memory stream. 
			return Convert.ToBase64String( _Encrypted );
		}

		public string Decrypt( string cipherText, int slot = 0 )
		{
			// Check arguments. 
			if ( string.IsNullOrEmpty( cipherText ) )
				throw new ArgumentNullException( "cipherText null or empty" );
			if ( slot < 0 || slot > 1 )
				throw new ArgumentException( "slot must be 0 or 1" );

			// Read the DEK and IV from the registry
			string _DEK = null;
			string _IV = null;

			try
			{
				_DEK = Decrypt( (string)Registry.LocalMachine.OpenSubKey( "SOFTWARE" ).OpenSubKey( "NEXXO" ).GetValue( string.Format( "DEK{0}", slot ) ), Store.Machine );
			}
			catch ( Exception ex )
			{
				throw new SecurityException( string.Format( "HKLM.SOFTWRE.NEXXO.DEK{0} is missing or invalid: " + ex.Message, slot ) );
			}
			try
			{
				_IV = Decrypt( (string)Registry.LocalMachine.OpenSubKey( "SOFTWARE" ).OpenSubKey( "NEXXO" ).GetValue( string.Format( "IV{0}", slot ) ), Store.Machine );
			}
			catch ( Exception ex )
			{
				throw new SecurityException( string.Format( "HKLM.SOFTWRE.NEXXO.IV{0} is missing or invalid: " + ex.Message, slot ) );
			}

			if ( _DEK == null || _DEK.Length != 44 )
				throw new ArgumentNullException( string.Format( "Invalid DEK{0}", slot ) );
			if ( _IV == null || _IV.Length != 24 )
				throw new ArgumentNullException( string.Format( "Invalid IV{0}", slot ) );

			// Create an AesManaged object with the specified key and IV. 
			using ( AesManaged _AES = new AesManaged() )
			{
				_AES.Key = Convert.FromBase64String( _DEK );
				_AES.IV = Convert.FromBase64String( _IV );

				// Create a decrytor to perform the stream transform.
				ICryptoTransform _AESDecryptor = _AES.CreateDecryptor( _AES.Key, _AES.IV );
				string _clearText = string.Empty;

				// Create the streams used for decryption. 
				using ( MemoryStream _msDecrypt = new MemoryStream( Convert.FromBase64String( cipherText ) ) )
				{
					using ( CryptoStream _csDecrypt = new CryptoStream( _msDecrypt, _AESDecryptor, CryptoStreamMode.Read ) )
					{
						using ( StreamReader _srDecrypt = new StreamReader( _csDecrypt ) )
						{
							// Read the decrypted bytes from the decrypting stream and place them in a string.
							_clearText = _srDecrypt.ReadToEnd();
						}
					}
				}

				return _clearText;
			}
		}
	}
}
