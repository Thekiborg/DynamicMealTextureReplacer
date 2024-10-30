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


		public static void SplitAtlas(List<int> dimensionsMapping, Material matSingleFromDef, ThingDef thingDef, ModExtension_DynamicMealTextureReplacer modExtension)
		{
			widthPixels = modExtension.widthPixels;
			heightPixels = modExtension.heightPixels;
			Vector2 mainTextureScale = new(XSize, YSize);

			int totalRows = dimensionsMapping.Count;
			// Makes the jaggedArray have as many arrays as rows are in the atlas

			modExtension.UVCoordsForPrinting = new Vector2[totalRows][][];
			modExtension.MeshesForDrawing = new Mesh[totalRows][];

			for (int rowNum = 0; rowNum < totalRows; rowNum++)
			{
				int texturesPerRow = dimensionsMapping[totalRows - 1 - rowNum];
				// Sets the amount of textures per row from the last one in the list to the first one.
				// Goes opposite to the loop to match the top most item in the XML list to the top most row in the texture/array

				modExtension.UVCoordsForPrinting[rowNum] = new Vector2[texturesPerRow][];
				modExtension.MeshesForDrawing[rowNum] = new Mesh[texturesPerRow];

				for (int colNum = 0; colNum < texturesPerRow; colNum++)
				{
					Rect singleTextureRect = new()
					{
						x = CalculateXCoordinate(colNum),
						y = CalculateYCoordinate(rowNum),
						width = XSize,
						height = YSize,
					};

					Printer_Plane.GetUVs(singleTextureRect, out var uv1, out var uv2, out var uv3, out var uv4, false);
					modExtension.UVCoordsForPrinting[rowNum][colNum] = [uv1, uv2, uv3, uv4];

					PopulateMeshArray(colNum, rowNum, uv1, uv2, uv3, uv4, modExtension);
				}
			}
			
			if (modExtension.UVCoordsForPrinting.NullOrEmpty())
			{
				Log.Error($"Could not extract UVs for meal {thingDef}, this should never happen.");
			}
			if (modExtension.MeshesForDrawing.NullOrEmpty())
			{
				Log.Error($"Could not extract Meshes for meal {thingDef}, this should never happen.");
			}/*
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
			*/
		}

		private static void PopulateMeshArray(int X, int Y, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, ModExtension_DynamicMealTextureReplacer modExtension)
		{
			Mesh singleTextureMesh = MeshMakerPlanes.NewPlaneMesh(1f);
			singleTextureMesh.uv = [uv1, uv2, uv3, uv4];
			modExtension.MeshesForDrawing[Y][X] = singleTextureMesh;
		}

		private static float CalculateXCoordinate(int X)
		{
			return X * XSize + (XPadding * (X * 2 + 1));
			// X * size moves X full texture box sizes to the right

			// X * 2 + 1 calculates how many times the paddings needs to be added for the material to land in the texture box
			// xPadding * ↑ gets the actual padding size to add
		}

		private static float CalculateYCoordinate(int Y)
		{
			return Y * YSize + (YPadding * (Y * 2 + 1));
		}
	}
}
