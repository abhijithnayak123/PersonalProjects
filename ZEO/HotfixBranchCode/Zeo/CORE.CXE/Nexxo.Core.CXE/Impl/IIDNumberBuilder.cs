﻿namespace MGI.Core.CXE.Impl
{
	public interface IIDNumberBuilder
	{
		long NextPAN();

		long GenerateNewId( string sequenceName, long first15 );
	}
}
