namespace MGI.Cxn.Check.Contract
{
	public interface ICheckProcessorFactory
	{
		ICheckProcessor GetProcessor(int processorID);
	}
}
