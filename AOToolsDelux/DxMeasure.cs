﻿#region + Using Directives
using System.Windows.Forms;
using AOTools.Utility;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using View = Autodesk.Revit.DB.View;

using static AOTools.Utility.Util;


#endregion

// projname: AOTools
// itemname: DxMeasure
// username: jeffs
// created:  12/1/2018 6:51:01 PM

namespace AOTools
{

	[Transaction(TransactionMode.Manual)]
	public class DxMeasure : IExternalCommand
	{
		private static FormDxMeasure _form;
		private static UIDocument _uiDoc;
		internal static Document _doc;

		private static bool _showWorkplane = false;

		public static bool ShowWorkplane { get => _showWorkplane; set => _showWorkplane = value; }

		public Result Execute(
			ExternalCommandData commandData,
			ref string message, ElementSet elements)
		{
			UIApplication uiApp = commandData.Application;
			_uiDoc = uiApp.ActiveUIDocument;
			_doc = _uiDoc.Document;

			
			// this cleaned up the text display problem
			//			Application.SetCompatibleTextRenderingDefault(false);

			using (TransactionGroup tg = new TransactionGroup(_doc, "AO delux measure"))
			{
				tg.Start();
				Process();
				tg.RollBack();
			}

			return Result.Succeeded;
		}

		internal static bool Process(UIDocument uiDoc,
			Document doc)
		{
			_uiDoc = uiDoc;
			_doc = doc;

			return Process();
		}

		internal static bool Process()
		{
			_form = new FormDxMeasure();

			View av = _doc.ActiveView;

			Util.VType vtype = GetViewType(av);

			if (vtype.VTCat == Util.VTtypeCat.OTHER)
			{
				return false;
			}

			// get the current sketch / work plane
			Plane p = av.SketchPlane?.GetPlane();

			if (p == null && (vtype.VTCat == Util.VTtypeCat.D2_WITHPLANE ||
				vtype.VTCat == Util.VTtypeCat.D3_WITHPLANE))
			{
				using (Transaction t = new Transaction(_doc, "measure points"))
				{
					t.Start();
					Plane plane = Plane.CreateByNormalAndOrigin(
						_doc.ActiveView.ViewDirection,
						new XYZ(0, 0, 0));

					SketchPlane sp = SketchPlane.Create(_doc, plane);

					av.SketchPlane = sp;

					t.Commit();
				}
			}

			MeasurePts(av, vtype);

			return true;
		}

		private static bool MeasurePts(View av, Util.VType vtype)
		{
			bool again = true;

			XYZ normal = XYZ.BasisZ;
			XYZ workingOrigin = XYZ.Zero;
			XYZ actualOrigin = XYZ.Zero;

			Plane p = av.SketchPlane?.GetPlane();
			string planeName = av.SketchPlane?.Name ?? "No Name";

			if (p != null)
			{
				normal = p.Normal;
				actualOrigin = p.Origin;

				if (vtype.VTSub != Util.VTypeSub.D3_VIEW)
				{
					workingOrigin = p.Origin;
				}
			}

//			ShowHideWorkplane(p, av);

			PointMeasurements? pm = GetPts(workingOrigin);

			while (again)
			{
				_form.UpdatePoints(pm, vtype, planeName);

				DialogResult result = _form.ShowDialog();

//				ShowHideWorkplane(p, av);

				switch (result)
				{
					case DialogResult.OK:
						pm = GetPts(workingOrigin);
						break;
					case DialogResult.Cancel:
						// must process the whole list of TransactionGroups
						// held by the stack
						again = false;
						break;
				}
			}
			return true;
		}

		private static PointMeasurements? GetPts(XYZ workingOrigin)
		{
			_form.tbxMessage.ResetText();

			XYZ startPoint;
			XYZ endPoint;

			try
			{
				View avStart = _doc.ActiveView;

				startPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				if (startPoint == null) return null;

				View avEnd = _doc.ActiveView;

				// cannot change views between points
				if (avStart.Id.IntegerValue != avEnd.Id.IntegerValue)
				{
					return null;
				}

				endPoint = _uiDoc.Selection.PickPoint(snaps, "Select Point");
				if (endPoint == null) return null;
			}
			catch
			{
				return null;
			}
			return new PointMeasurements(startPoint, endPoint, workingOrigin);
		}

		public static void ShowHideWorkplane()
		{
			View av = _doc.ActiveView;
			Plane p = av.SketchPlane?.GetPlane();

			if (p == null )
			{
				if (_form != null)
				{
					_form.Message = "No work plane to show";
				}
			}
			else
			{
				ShowHideWorkplane(p, av);
			}
		}

		private static void ShowHideWorkplane(Plane p, View av)
		{
			if (p == null) { return; }

			try
			{
				using (Transaction t = new Transaction(_doc, "measure points"))
				{
					t.Start();

					if (ShowWorkplane)
					{
						av.ShowActiveWorkPlane();
					}
					else
					{
						av.HideActiveWorkPlane();
					}

					t.Commit();
				}
			}
			catch
			{
			}
		}
	}
}
