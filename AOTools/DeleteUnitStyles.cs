﻿#region Using directives

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static AOTools.Settings.ExtensibleStorageMgr;

#endregion

// itemname:	DeleteUnitStyles
// username:	jeffs
// created:		1/14/2018 7:19:43 PM


namespace AOTools
{
	[Transaction(TransactionMode.Manual)]
	class DeleteUnitStyles : IExternalCommand
	{
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			RSettings.DeleteCurrentSchema();

			return Result.Succeeded;
		}
	}
}
