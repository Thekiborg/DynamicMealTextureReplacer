using System.Linq;

namespace DynamicMealTextureReplacer
{
	internal static class MealAtlasIngredientFilter
	{
		public static int GetRow(ModExtension_DynamicMealTextureReplacer modExtension, CompIngredients compIngredients)
		{
			for (int atlasYRowIndex = 0; atlasYRowIndex < modExtension.dimensionsMapping.Count; atlasYRowIndex--)
			{
				List<ThingDef> filterOnRow = modExtension.dimensionsMapping.ElementAt(modExtension.dimensionsMapping.Count - 1 - atlasYRowIndex).Key;
				// Goes opposite to the loop to match how the array is actually constructed;
				// Without going opposite, the top most ingredient on the List will match with the bottom most row in the array.
				// While going opposite, the top most ingredient matches the top most row in the array, matching the texture and the XML visually.

				List<ThingDef> ingredientsToCheck = compIngredients.ingredients.Take(modExtension.maxCheckedIngredients).ToList();

				if (MatchesFilter(filterOnRow, ingredientsToCheck))
				{
					return atlasYRowIndex;
				}
			}
			Log.Warning($"Could not match the ingredient filter on {compIngredients.parent}, throwing.");
			return 0;
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
