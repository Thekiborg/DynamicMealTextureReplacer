﻿using System.Linq;

namespace DynamicMealTextureReplacer
{
	internal static class MealAtlasIngredientFilter
	{
		public static int GetRow(ModExtension_DynamicMealTextureReplacer modExtension, CompIngredients compIngredients)
		{
			int atlasYIndex = 0;
			List<ThingDef> ingredientsToCheck = compIngredients.ingredients.Take(modExtension.maxCheckedIngredients).ToList();

			foreach (List<ThingDef> filterOnRow in modExtension.dimensionsMapping.Keys)
			{
				// The top most ingredient matches the top most row in the array, matching the atlas and the XML visually.

				if (MatchesFilter(filterOnRow, ingredientsToCheck))
				{
					return atlasYIndex;
				}
				atlasYIndex++;
			}
			Log.Warning($"Could not match the ingredient filter on {compIngredients.parent}, returning fallback.");
			return -1;
		}

		private static bool MatchesFilter(List<ThingDef> ingredientFilter, List<ThingDef> ingredientsInMeal)
		{
			for (int i = 0; i < ingredientFilter.Count; i++)
			{
				if (!ingredientsInMeal.Contains(ingredientFilter[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
