global using System;
global using System.Collections.Generic;
global using System.Reflection;
global using RimWorld;
global using Verse;
global using UnityEngine;

namespace DynamicMealTextureReplacer
{
	[StaticConstructorOnStartup]
	internal static class DynamicMealTextureReplacer
	{
		static DynamicMealTextureReplacer()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
			{
				var modExt = thingDef.GetModExtension<ModExtension_DynamicMealTextureReplacer>();

				if (modExt is null)
				{
					continue;
				}

				MealAtlasSplitter.SplitAtlas(
					[.. modExt.dimensionsMapping.Values],
					thingDef,
					modExt);
			}
		}
	}
}
