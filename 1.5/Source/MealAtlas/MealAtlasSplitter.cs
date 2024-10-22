using System.Linq;
using System.Text;

namespace DynamicMealTextureReplacer
{
	[StaticConstructorOnStartup]
	internal static class MealAtlasSplitter
	{
		// -- Atlas Information --
		static float widthPixels;
		static float heightPixels;
		const float textureSize = 128f;
		const float spaceBetweenTextures = 16f;

		// -- Atlas Sizes --
		static float XSize => textureSize / widthPixels;
		static float YSize => textureSize / heightPixels;
		static float XPadding => XSize / spaceBetweenTextures;
		static float YPadding => YSize / spaceBetweenTextures;


		public static Material[][] SplitAtlas(List<int> dimensionsMapping, Material matSingleFromDef, ThingDef thingDef)
		{
			var modExtension = thingDef.GetModExtension<ModExtension_DynamicMealTextureReplacer>();
			widthPixels = modExtension.widthPixels;
			heightPixels = modExtension.heightPixels;	

			int totalRows = dimensionsMapping.Count;
			// Makes the jaggedArray have as many arrays as rows are in the atlas

			Material[][] extractedTextures = new Material[totalRows][];
			Vector2 mainTextureScale = new(XSize, YSize);

			for (int Y = 0; Y < totalRows; Y++)
			{
				int texturesPerRow = dimensionsMapping[totalRows - 1 - Y];
				// Sets the amount of textures per row from the last one in the list to the first one.
				// Goes opposite to the loop to match the top most item in the XML list to the top most row in the texture/array

				extractedTextures[Y] = new Material[texturesPerRow];

				for (int X = 0; X < texturesPerRow; X++)
				{
					Material separatedMaterial = MaterialDuplicator.DuplicateMaterial(matSingleFromDef);
					separatedMaterial.name = $"{matSingleFromDef.name}_{Y}-{X}";

					separatedMaterial.mainTextureOffset = new(
						X * XSize + (XPadding * (X * 2 + 1)),
						// X * size moves X full texture box sizes to the right
						// X * 2 + 1 calculates how many times the paddings needs to be added for the material to land in the texture box
						// xPadding * ↑ gets the actual padding size to add
						Y * YSize + (YPadding * (Y * 2 + 1)));

					separatedMaterial.mainTextureScale = mainTextureScale;
					extractedTextures[Y][X] = separatedMaterial;
				}
			}

			if (extractedTextures.NullOrEmpty())
			{
				Log.Error($"Could not extract variants for meal {thingDef}, this should never happen.");
			}
			else
			{
				for (int Y = 0; Y < extractedTextures.Length; Y++)
				{
					var ingredientsFilter = modExtension.dimensionsMapping.ElementAt(extractedTextures.Length - 1 - Y).Key;
					var texturesPerRow = extractedTextures[Y].Length;

					StringBuilder stringBuilder = new();

					stringBuilder.Append($"New row at index {Y} with filter {{ ");

					foreach (var ingredient in ingredientsFilter)
					{
						stringBuilder.Append($"{ingredient} ");
					}
					stringBuilder.AppendLine($"}} with {texturesPerRow} variants:");
					stringBuilder.AppendLine();

					for (int X = 0; X < texturesPerRow; X++)
					{
						Material mat = extractedTextures[Y][X];
						stringBuilder.AppendLine($"■ Texture at ({Y},{X}) with name {mat.name} and offset {mat.mainTextureOffset}");
					}

					Log.Message(stringBuilder);
				}
			}

			return extractedTextures;
		}
	}
}
