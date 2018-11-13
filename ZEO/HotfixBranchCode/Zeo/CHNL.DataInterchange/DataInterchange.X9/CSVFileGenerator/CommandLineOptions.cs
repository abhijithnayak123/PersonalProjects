using CommandLine;

namespace Reporting.X9
{
	class CommandLineOptions
	{
		[Option('m', "mode", Required = true,
			HelpText = "Mode (required) values can 1, 2 or 3")]
		public int RunMode { get; set; }

		[Option('r', "rundate", Required = false,
			HelpText = "Run date for the X9 file")]
		public string RunDate { get; set; }

		[Option('i', "ignorepreviousrun", Required = false,
			HelpText = "Ignore previous run?")]
		public bool IgnorePreviousRun { get; set; }

		[Option('p', "partner", Required = false,
			HelpText = "Partner for which to generate X9 files")]
		public string Partner { get; set; }
	}
}
