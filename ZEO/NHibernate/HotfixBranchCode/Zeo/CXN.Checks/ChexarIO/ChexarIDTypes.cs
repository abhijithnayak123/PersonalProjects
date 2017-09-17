using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChexarIO
{
	public enum ChexarIDTypes : int
	{
		Unknown = 0,
		USDriversLicense = 2,
		Passport = 3,
		StateID = 4,
		MilitaryId = 5,
		AlienIDOrGreenCard = 6,
		MatriculaConsular = 7,
		NationalIdOrCedulaDeIdentidad = 8,
		VoterRegistration = 9,
		DiplomaticId = 10,
		DiplomaticDriversLicense = 11,
		ForeignDriversLicense = 30,
		USVisa = 31
	}
}
