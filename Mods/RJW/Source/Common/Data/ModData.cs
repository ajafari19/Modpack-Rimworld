using HugsLib;
using HugsLib.Utils;
using System;

namespace rjw
{
	/// <summary>
	/// Rjw settings store
	/// </summary>
	public class SaveStorage : ModBase
	{
		public override string ModIdentifier => "RJW";

		public override Version GetVersion()
		{
			//--Log.Message("GetVersion() called");
			return base.GetVersion();
		}

		public static DataStore DataStore;//reference to savegame data, hopefully
		public static DesignatorsData DesignatorsData;//reference to savegame data, hopefully

		public override void WorldLoaded()
		{
			DataStore = UtilityWorldObjectManager.GetUtilityWorldObject<DataStore>();
			DesignatorsData = UtilityWorldObjectManager.GetUtilityWorldObject<DesignatorsData>();
		}
		protected override bool HarmonyAutoPatch { get => false; }//first.cs creates harmony and does some convoulted stuff with it

	}
}
