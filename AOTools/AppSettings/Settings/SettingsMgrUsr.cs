﻿#region Using directives

using System.Collections.Generic;
using System.Runtime.Serialization;
using AOTools.AppSettings.RevitSettings;
using AOTools.AppSettings.Schema;
using AOTools.Settings;
using UtilityLibrary;
using Point = System.Drawing.Point;

#endregion

// itemname:	SettingsMgrUsr
// username:	jeffs
// created:		1/3/2018 8:04:40 PM

// user settings - incomplete

namespace AOTools.AppSettings.Settings
{
	public static class SettingsMgrUsr
	{
		public static readonly SettingsMgr<SettingsUsr> SmUsrSetg;

		public static readonly SettingsUsr SmUsr;

		static SettingsMgrUsr()
		{
			SmUsrSetg = new SettingsMgr<SettingsUsr>();
			SmUsr = SmUsrSetg.Settings;
			SmUsr.Header = new Header(SettingsUsr.USERSETTINGFILEVERSION);
		}
	}


	// sample Settings User
	[DataContract]
	public class SettingsUsr : SettingsPathFileUserBase
	{
		public int Count => UnitStylesList.Count;

		public const string USERSETTINGFILEVERSION = "1.0";

		[DataMember]
		public Point FormMeasurePointsLocation = new Point(0, 0);
		[DataMember]
		public bool MeasurePointsShowWorkplane = false;

		[DataMember]
		public List<SchemaDictionaryUsr> UnitStylesList = RevitSettingsUnitUsr.RsuUsr.RsuUsrSetg;
		
	}

}
