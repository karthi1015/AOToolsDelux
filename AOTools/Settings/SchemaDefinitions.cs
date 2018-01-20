﻿#region Using directives
using System;
using System.Collections.Generic;

using static AOTools.SBasicKey;
using static AOTools.SUnitKey;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

#endregion

// itemname:	SchemaDefinitions
// username:	jeffs
// created:		1/14/2018 4:28:23 PM


namespace AOTools
{
	// master schema
	//  field 0 = version
	//	field 1 = bool : UseOfficeUnitStyle
	//	field 2 = bool : AutoRestoreUnitStyle
	//	field 3 = int  : CurrentUnitStyle
	//	field 10+ = Entity : schema : a unit style

	// unit sub schema  (always UnitType = UT_Length
	//	field 0 = verson
	//	field 1 = string : unit style name
	//	field 2 = bool   : CanBeErased
	//	field 3 = int    : UnitSystem
	//	field 4 = int    : UnitType
	//	field 5 = double : accuracy
	//	field 6 = int    : display unit type
	//	fleid 7 = int    : unit symbol type
	//	field 8 = int    : fmt op: suppress spaces			// 0 = false; 1 = true; -1 = ignore
	//	field 9 = int    : fmt op: suppress leading zeros	// 0 = false; 1 = true; -1 = ignore
	//	field 10= int    : fmt op: suppress trailing zeros	// 0 = false; 1 = true; -1 = ignore
	//	field 11= int    : fmt op: use digit grouping		// 0 = false; 1 = true; -1 = ignore
	//	field 12= int    : fmt op: use plus prefix			// 0 = false; 1 = true; -1 = ignore

	class BasicSchema
	{
		public const string SCHEMA_NAME = "UnitStyleSettings";
		public const string SCHEMA_DESC = "unit style setings";

		public static readonly Guid SchemaGUID = new Guid("B1788BC0-381E-4F4F-BE0B-93A93B9470FF");

		public static readonly Dictionary<SBasicKey, FieldInfo> _schemaFields =
			new Dictionary<SBasicKey, FieldInfo>()
			{
				{
					(CURRENT),
					new FieldInfo(CURRENT, "CurrentUnitStyle",
						"number of the current style", 0)
				},

				{
					(COUNT),
					new FieldInfo(COUNT, "Count",
						"number of unit styles", 3)
				},

				{
					(USE_OFFICE),
					new FieldInfo(USE_OFFICE, "UseOfficeUnitStyle",
						"use the office standard style", true)
				},

				{
					(VERSION_BASIC),
					new FieldInfo(VERSION_BASIC, "version",
						"version", "1.0")
				},

				{
					(AUTO_RESTORE),
					new FieldInfo(AUTO_RESTORE,
						"AutoRestoreUnitStyle", "auto update to the selected unit style", true)
				}
			};

		internal static Dictionary<SBasicKey, FieldInfo> GetBasicSchemaFields()
		{
			return _schemaFields.Clone();
		}

		// the guid for each sub-schema and the 
		// field that holds the sub-schema - both must match
		// the guid here is missing the last (2) digits.
		// fill in for each sub-schema 
		// unit type is number is a filler
		public static readonly FieldInfo _subSchemaFieldInfo =
			new FieldInfo(UNDEFINED, "LocalUnitStyle{0:D2}",
				"subschema for the local unit style",
				new Entity(), UnitType.UT_Number, "B2788BC0-381E-4F4F-BE0B-93A93B9470{0:x2}");

		public static Guid GetSubSchemaGuid(int i)
		{
			return new Guid(string.Format(_subSchemaFieldInfo.Guid, i));
		}
	}


	class UnitSchema
	{
		public const string UNIT_SCHEMA_NAME = "UnitStyleSchema";
		public const string UNIT_SCHEMA_DESC = "unit style sub schema";

		public static readonly Dictionary<SUnitKey, FieldInfo> _unitSchemaFields =
			new Dictionary<SUnitKey, FieldInfo>()
			{
				{
					(VERSION_UNIT),
					new FieldInfo(VERSION_UNIT,
						"version", "version", "1.0")
				},

				{
					(STYLE_NAME),
					new FieldInfo(STYLE_NAME,
						"UnitStyleName", "name of this unit style", "unit style {0:D2}")
				},

				{
					(STYLE_DESC),
					new FieldInfo(STYLE_DESC,
						"UnitStyleDesc", "description for this unit style", "unit style description")
				},

				{
					(CAN_BE_ERASED),
					new FieldInfo(CAN_BE_ERASED,
						"CanBeErased", "can this unit style be erased", false)
				},

				{
					(UNIT_SYSTEM),
					new FieldInfo(UNIT_SYSTEM,
						"US", "unit system", (int) UnitSystem.Imperial)
				},

				{
					(UNIT_TYPE),
					new FieldInfo(UNIT_TYPE,
						"UnitType", "unit type", (int) UnitType.UT_Length)
				},

				{
					(ACCURACY),
					new FieldInfo(ACCURACY,
						"Accuracy", "accuracy", (1.0 / 12.0) / 16.0, UnitType.UT_Number)
				},

				{
					(DUT),
					new FieldInfo(DUT, "DUT",
						"display unit type", (int) DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES)
				},

				{
					(UST),
					new FieldInfo(UST,
						"UST", "unit symbol type", (int) UnitSymbolType.UST_NONE)
				},

				{
					(SUP_SPACE),
					new FieldInfo(SUP_SPACE,
						"SuppressSpaces", "suppress spaces", (int) FmtOpt.YES)
				},

				{
					(SUP_LEAD_ZERO),
					new FieldInfo(SUP_LEAD_ZERO,
						"SuppressLeadZero", "suppress leading zero", (int) FmtOpt.NO)
				},

				{
					(SUP_TRAIL_ZERO),
					new FieldInfo(SUP_TRAIL_ZERO,
						"SuppressTrailZero", "suppress trailing zero", (int) FmtOpt.IGNORE)
				},

				{
					(USE_DIG_GRP),
					new FieldInfo(USE_DIG_GRP,
						"DigitGrouping", "digit grouping", (int) FmtOpt.YES)
				},

				{
					(USE_PLUS_PREFIX),
					new FieldInfo(USE_PLUS_PREFIX,
						"PlusPrefix", "plus prefix", (int) FmtOpt.NO)
				}
			};

		internal static Dictionary<SUnitKey, FieldInfo>[] GetUnitSchemaFields(int count)
		{
			Dictionary<SUnitKey, FieldInfo>[] unitSchemaFields =
				new Dictionary<SUnitKey, FieldInfo>[count];

			// create the UnitSchema's
			// personlize the sub schema's
			for (int i = 0; i < count; i++)
			{
				unitSchemaFields[i] = _unitSchemaFields.Clone();
				unitSchemaFields[i][STYLE_NAME].Value =
					string.Format(_unitSchemaFields[STYLE_NAME].Value, i);

			}

			return unitSchemaFields;
		}

		internal static string GetSubSchemaName(int i)
		{
			return string.Format(BasicSchema._subSchemaFieldInfo.Name, i);
		}
	}
}
