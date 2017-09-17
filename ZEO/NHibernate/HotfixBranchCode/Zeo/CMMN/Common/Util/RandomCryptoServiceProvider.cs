using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MGI.Common.Util
{

	/****************************Begin TA-50 Changes************************************************/
	//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
	//     Purpose: On Vera Code Scan, We found random.next would lead to security flaw to overcome we have inherited random and wrapped it with RNGCryptoServiceProvider
	
	// Inherit System.Random, Random is not sealed and its public methods are all virtual.
	public class RandomCryptoServiceProvider
	{

		private RNGCryptoServiceProvider _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
		private byte[] _uint32Buffer = new byte[4];

		public RandomCryptoServiceProvider() { }
		public RandomCryptoServiceProvider(int ignoredSeed) { }

		public int Next()
		{
			_rngCryptoServiceProvider.GetBytes(_uint32Buffer);
			//maximum value of Int32 is 2,147,483,647, hexadecimal 0x7FFFFFFF
			return BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF;
		}

		public int Next(int maxValue)
		{
			if (maxValue < 0)
				throw new ArgumentOutOfRangeException("maxValue");
			return Next(0, maxValue);
		}

		public int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
				throw new ArgumentOutOfRangeException("minValue");
			if (minValue == maxValue) return minValue;
			long diff = maxValue - minValue;
			while (true)
			{
				_rngCryptoServiceProvider.GetBytes(_uint32Buffer);
				UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);

				long max = (1 + (long)UInt32.MaxValue);
				long remainder = max % diff;
				if (rand < max - remainder)
				{
					return (int)(minValue + (rand % diff));
				}
			}
		}

		public double NextDouble()
		{
			_rngCryptoServiceProvider.GetBytes(_uint32Buffer);
			UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);
			return rand / (1.0 + UInt32.MaxValue);
		}

		public void NextBytes(byte[] buffer)
		{
			if (buffer == null) throw new ArgumentNullException("buffer");
			_rngCryptoServiceProvider.GetBytes(buffer);
		}
	}
}
