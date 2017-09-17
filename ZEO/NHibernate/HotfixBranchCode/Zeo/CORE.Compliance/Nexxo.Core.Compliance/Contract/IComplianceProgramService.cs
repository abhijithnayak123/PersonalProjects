using MGI.Core.Compliance.Data;

namespace MGI.Core.Compliance.Contract
{
	public interface IComplianceProgramService
	{
		/// <summary>
		/// This method is to get the compliance program by name
		/// </summary>
		/// <param name="countryAbbr">This is compliance program name</param>		
		/// <returns>Compliance program details</returns>
		ComplianceProgram Get( string name );
	}
}
