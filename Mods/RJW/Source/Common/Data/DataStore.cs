using System.Collections.Generic;
using HugsLib.Utils;
using Verse;

namespace rjw
{
	/// <summary>
	/// Hugslib Utility Data object storing the RJW info
	/// also implements extensions to Pawn method 
	/// is used as a static field in PawnData
	/// </summary>
	public class DataStore : UtilityWorldObject
	{
		public Dictionary<int, PawnData> PawnData = new Dictionary<int, PawnData> ();
		//public Dictionary<int, PartsData> PartsData = new Dictionary<int, PartsData> ();

		public override void ExposeData()
		{
			if (Scribe.mode==LoadSaveMode.Saving)
				PawnData.RemoveAll(item => item.Value==null || !item.Value.IsValid);
			base.ExposeData();
			Scribe_Collections.Look(ref PawnData, "Data", LookMode.Value, LookMode.Deep);
			//Scribe_Collections.Look(ref PartsData, "Data", LookMode.Value, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (PawnData == null) PawnData = new Dictionary<int, PawnData>();
				//if (PartsData == null) PartsData = new Dictionary<int, PartsData>();
			}
		}

		public PawnData GetPawnData(Pawn pawn)
		{
			PawnData res;
			//--Log.Message("Getting data for " + pawn);
			//--Log.Message("Pawn " + pawn + " id " + pawn.thingIDNumber);
			//--Log.Message("PawnData isn't null " + !(PawnData == null));
			var filled = PawnData.TryGetValue(pawn.thingIDNumber, out res);
			//--Log.Message("Output is not null" + PawnData.TryGetValue(pawn.thingIDNumber, out res));
			//--Log.Message("Out is not null " + (res != null));
			//--Log.Message("Out is valid " + (res != null && res.IsValid));
			if ((res==null) || (!res.IsValid))
			{
				if (filled)
				{
					//--Log.Message("Clearing incorrect data for " + pawn);
					PawnData.Remove(pawn.thingIDNumber);
				}
				//--Log.Message("PawnData missing, creating for " + pawn);
				res = new PawnData(pawn);
				PawnData.Add(pawn.thingIDNumber, res);
			}
			//--Log.Message("Finishing");
			//--Log.Message("PawnData is " + res.Comfort + " " + res.Service + " " + res.Breeding);
			return res;
		}

		void SetPawnData(Pawn pawn, PawnData data)
		{
			PawnData.Add(pawn.thingIDNumber, data);
		}

		//public PartsData GetPartsData(Thing thing)
		//{
		//	PartsData res;
		//	//--Log.Message("Getting data for " + thing);
		//	//--Log.Message("Pawn " + thing + " id " + thing.thingIDNumber);
		//	//--Log.Message("PartsData isn't null " + !(PartsData == null));
		//	var filled = PartsData.TryGetValue(thing.thingIDNumber, out res);
		//	//--Log.Message("Output is not null" + PartsData.TryGetValue(thing.thingIDNumber, out res));
		//	//--Log.Message("Out is not null " + (res != null));
		//	//--Log.Message("Out is valid " + (res != null && res.IsValid));
		//	if ((res==null) || (!res.IsValid))
		//	{
		//		if (filled)
		//		{
		//			//--Log.Message("Clearing incorrect data for " + thing);
		//			PartsData.Remove(thing.thingIDNumber);
		//		}
		//		//--Log.Message("PartsData missing, creating for " + thing);
		//		res = new PartsData(thing);
		//		PartsData.Add(thing.thingIDNumber, res);
		//	}
		//	//--Log.Message("Finishing");
		//	return res;
		//}

		//void SetPartsData(Thing thing, PartsData data)
		//{
		//	PartsData.Add(thing.thingIDNumber, data);
		//}
	}
}