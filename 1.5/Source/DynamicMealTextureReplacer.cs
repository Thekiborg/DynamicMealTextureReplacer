global using System;
global using System.Collections.Generic;
global using System.Reflection;
global using RimWorld;
global using Verse;
global using UnityEngine;
using System.Linq;

namespace DynamicMealTextureReplacer
{
	[StaticConstructorOnStartup]
	internal static class DynamicMealTextureReplacer
	{
		const string InsectMeatTexPath = "Things/Item/Resource/MeatFoodRaw/Meat_Insect";


		static readonly List<ThingDef> AllMeatThingDefs = DefDatabase<ThingDef>.AllDefsListForReading
													.Where(meatDef =>
															meatDef.ingestible?.foodType == FoodTypeFlags.Meat
															&& meatDef.graphicData?.texPath != InsectMeatTexPath
															).ToList();


		static readonly List<ThingDef> AllEggThingDefs = DefDatabase<ThingDef>.AllDefsListForReading
											.Where(eggDef =>
													!eggDef.thingCategories.NullOrEmpty()
													&& eggDef.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized)
													).ToList();


		static readonly List<ThingDef> AllInsectMeatThingDefs = DefDatabase<ThingDef>.AllDefsListForReading
									.Where(meatDef =>
											meatDef.ingestible?.foodType == FoodTypeFlags.Meat
											&& meatDef.graphicData?.texPath == InsectMeatTexPath
											).ToList();


		static DynamicMealTextureReplacer()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				var modExt = thingDef.GetModExtension<ModExtension_DynamicMealTextureReplacer>();

				if (modExt is null)
				{
					continue;
				}

				AddMeatThingDefsToDictionary(modExt.dimensionsMapping);
				AddInsectMeatThingDefsToDictionary(modExt.dimensionsMapping);
				AddEggsThingDefsToDictionary(modExt.dimensionsMapping);

				MealAtlasSplitter.SplitAtlas(
					[.. modExt.dimensionsMapping.Values],
					thingDef,
					modExt);
			}
		}


		/// <summary>
		/// Adds all the meat thingdefs loaded in the game to any key list that has the dummy def DMTR_FillWithAllMeatDefs
		/// </summary>
		/// <param name="atlasMapping"></param>
		private static void AddMeatThingDefsToDictionary(Dictionary<List<ThingDef>, int> atlasMapping)
		{
			foreach (var ingredientsList in atlasMapping.Keys.Where(list => list.Contains(DMTRDefOf.DMTR_FillWithAllMeatDefs)))
			{
				ingredientsList.AddRange(AllMeatThingDefs);
			}
		}


		/// <summary>
		/// Adds all the insect meat thingsdefs loaded in the game to any key list that has the dummy def DMTR_FillWithAllInsectMeatDefs
		/// </summary>
		/// <param name="atlasMapping"></param>
		private static void AddInsectMeatThingDefsToDictionary(Dictionary<List<ThingDef>, int> atlasMapping)
		{
			foreach (var ingredientsList in atlasMapping.Keys.Where(list => list.Contains(DMTRDefOf.DMTR_FillWithAllInsectMeatDefs)))
			{
				ingredientsList.AddRange(AllInsectMeatThingDefs);
			}
		}


		/// <summary>
		/// Adds all the unfertilized egg thingsdefs loaded in the game to any key list that has the dummy def DMTR_FillWithAllInsectMeatDefs
		/// </summary>
		/// <param name="atlasMapping"></param>
		private static void AddEggsThingDefsToDictionary(Dictionary<List<ThingDef>, int> atlasMapping)
		{
			foreach (var ingredientsList in atlasMapping.Keys.Where(list => list.Contains(DMTRDefOf.DMTR_FillWithAllEggDefs)))
			{
				ingredientsList.AddRange(AllEggThingDefs);
			}
		}
	}
}
