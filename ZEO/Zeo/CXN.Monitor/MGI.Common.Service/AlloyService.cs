using System;
using System.ServiceProcess;
using MGI.Common.Archive.Data;
using MGI.Common.Archive.Impl;
using System.Configuration;
using P3Net.Data.Sql;
using System.Globalization;


namespace MGI.Common.Service
{
	public partial class AlloyService : ServiceBase
	{
		#region Private Members
		string ptnrConnectionString = null;

		//set default datetime value
		DateTime arcScheduledTime = DateTime.MinValue;
		DateTime cpMonitorFrequencyMinutes = DateTime.MinValue;
		DateTime imageArchiveFrequencyTime = DateTime.MinValue;
		#endregion

		public AlloyService()
		{
			InitializeComponent();
		}

		#region Service Events
		protected override void OnStart(string[] args)
		{
			//System.Diagnostics.Debugger.Launch();

			arcScheduledTime = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["ImageArchiveScheduledTime"], "HH:mm:ss", CultureInfo.InvariantCulture);

			double archiveFrequencyHours = 0.0;
			string archiveTimeFrequency = System.Configuration.ConfigurationManager.AppSettings["ImageArchiveFrequencyHours"]; //27:00:00
			// todo: check the string length before stubstring
			if (archiveTimeFrequency.Length >= 2)
			{
				if (Convert.ToInt32(archiveTimeFrequency.Substring(0, 2)) >= 24)
				{
					var timeString = archiveTimeFrequency.Split(':');
					// todo: check the string length of array throw approciate exception
					if (timeString.Length == 3)
					{
						var time = DateTime.MinValue;
						time = time.AddDays(Convert.ToInt32(timeString[0]) / 24);
						time = time.AddHours(Convert.ToInt32(timeString[0]) % 24);
						time = time.AddMinutes(Convert.ToInt32(timeString[1]));
						time = time.AddSeconds(Convert.ToInt32(timeString[2]));

						archiveFrequencyHours = (time - DateTime.MinValue).TotalMilliseconds;
					}
					else
					{
						
						throw new Exception("Incorrect format for the config key 'ImageArchiveFrequencyHours'. Please use HH:MM:SS format and start the service");
					}
				}
				else
				{
					imageArchiveFrequencyTime = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["ImageArchiveFrequencyHours"], "HH:mm:ss", CultureInfo.InvariantCulture);
					archiveFrequencyHours = (imageArchiveFrequencyTime.TimeOfDay - DateTime.MinValue.TimeOfDay).TotalMilliseconds;

				}
			}

			GenericTimer archiveTimer = new GenericTimer(archiveFrequencyHours, arcScheduledTime, Archive);
			archiveTimer.ServiceTimeStart();


			//cpMonitorFrequencyMinutes = DateTime.ParseExact(System.Configuration.ConfigurationManager.AppSettings["CpMonitorFrequencyMinutes"], "HH:mm:ss", CultureInfo.InvariantCulture);
			//double cpMonitorTimeSpan = (cpMonitorFrequencyMinutes.TimeOfDay - DateTime.MinValue.TimeOfDay).TotalMilliseconds;
			//GenericTimer cpMonitorTimer = new GenericTimer(cpMonitorTimeSpan, CpMonitor);
			//cpMonitorTimer.ServiceTimeStart();

		}

		protected override void OnStop()
		{
			// Clean up activities
		}
		#endregion

		#region Private Methods
		private void CpMonitor()
		{
			throw new NotImplementedException();
		}

		private void Archive()
		{
			if (ConfigurationManager.ConnectionStrings["PTNRDatabaseConnection"] != null)
			{
				ptnrConnectionString = ConfigurationManager.ConnectionStrings["PTNRDatabaseConnection"].ConnectionString;
			}
			else
			{
				return;
			}

			var imageDirectory = ConfigurationManager.AppSettings["ImageDirectory"] ?? Environment.CurrentDirectory;

			int archivePriorToDays = Convert.ToInt32(ConfigurationManager.AppSettings["ImageArchivePriorToDays"]);

			var sqlConnection = new SqlConnectionManager(ptnrConnectionString);

			AlloyImagesArchive archive = new AlloyImagesArchive();

			DataModel dataModelCheck = new DataModel()
			{
				OutputDirectory = imageDirectory,
				TransactionType = TransactionType.Check,
				ConnectionString = ptnrConnectionString,
				ArchivePriorToDays = archivePriorToDays
			};

			archive.BackupAlloyImages(dataModelCheck); // for check images Archieving

			DataModel dataModelMoneyOrder = new DataModel()
			{
				OutputDirectory = imageDirectory,
				TransactionType = TransactionType.MoneyOrder,
				ConnectionString = ptnrConnectionString,
				ArchivePriorToDays = archivePriorToDays
			};

			archive.BackupAlloyImages(dataModelMoneyOrder); // for MO images Archieving
		}
		#endregion
	}
}
