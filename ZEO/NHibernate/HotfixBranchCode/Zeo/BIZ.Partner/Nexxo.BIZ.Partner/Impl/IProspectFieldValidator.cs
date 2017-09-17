using System;

using MGI.Biz.Partner.Data;

using IdType = MGI.Core.Partner.Data.NexxoIdType;

namespace MGI.Biz.Partner.Impl
{
	public interface IProspectFieldValidator
	{
		void ValidateNames( Prospect prospect );

		void ValidateAddress( Prospect prospect );

		void ValidatePhone( Prospect prospect );

		void ValidateEmail( Prospect prospect );

		void ValidateDOB(Prospect prospect, int minimumAge);

		void ValidateSSN( Prospect prospect );

		void ValidateEmployment( Prospect prospect );

		void ValidateID( Identification id, IdType idType );

		//void ValidateCriteria( CustomerSearchCriteria criteria );
	}
}
