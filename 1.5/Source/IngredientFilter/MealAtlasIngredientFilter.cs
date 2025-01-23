using System.Linq;

namespace DynamicMealTextureReplacer
{
	internal static class MealAtlasIngredientFilter
	{
		public static int GetRow(ModExtension_DynamicMealTextureReplacer modExtension, CompIngredients compIngredients)
		{
			int atlasYIndex = 0;
			List<ThingDef> ingredientsToCheck = compIngredients.ingredients.Take(modExtension.maxCheckedIngredients).ToList();

			foreach (ThingFilter filter in modExtension.dimensionsMapping.Keys)
			{
				// The top most ingredient matches the top most row in the array, matching the atlas and the XML visually.

				if (MatchesFilter(filter, ingredientsToCheck))
				{
					return atlasYIndex;
				}
				atlasYIndex++;
			}

			if (DebugSettings.godMode)
			{
				Log.Warning($"Could not match the ingredient filter on {compIngredients.parent}, returning fallback.");
			}
			return -1;
		}

		private static bool MatchesFilter(ThingFilter filter, List<ThingDef> ingredientsInMeal)
		{
			foreach (ThingDef ingredient in ingredientsInMeal)
			{
				if (!filter.Allows(ingredient))
				{
					return false;
				}
			}
			return true;
		}
	}
}
