using System.Linq;

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


		/// <summary>
		/// Splits the atlas material into 2 jagged arrays.<br></br>
		/// The first jagged array is made up of UVs, used by Print() on the GraphicClass.<br></br>
		/// The second jagged array is made up of meshes, used by Draw() on the GraphicClass.<br></br>
		/// Both store individual food items separated by rows. They're stored like this so each texture following certain ingredients are in the same row, making it easier to fetch what is going to end up getting drawn.
		/// </summary>
		/// <param name="dimensionsMapping">List inside the modExtension. Used to know the size of each row.</param>
		/// <param name="thingDef">The thingdef the modExtension is on.</param>
		/// <param name="modExtension">The modExtension itself, which is where the resulting arrays get stored at.</param>
		public static void SplitAtlas(List<int> dimensionsMapping, ThingDef thingDef, ModExtension_DynamicMealTextureReplacer modExtension)
		{
			widthPixels = modExtension.widthPixels;
			heightPixels = modExtension.heightPixels;

			int totalRows = dimensionsMapping.Count;
			// Makes the jaggedArray have as many arrays as rows are in the atlas

			modExtension.UVCoordsForPrinting = new Vector2[totalRows][][];
			modExtension.MeshesForDrawing = new Mesh[totalRows][];

			for (int rowNum = totalRows - 1; rowNum >= 0; rowNum--)
			{
				int texturesPerRow = dimensionsMapping[rowNum];
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

					// Used in Print()
					modExtension.UVCoordsForPrinting[rowNum][colNum] = [uv1, uv2, uv3, uv4];

					// Used in Draw()
					Mesh singleTextureMesh = MeshMakerPlanes.NewPlaneMesh(1f);
					singleTextureMesh.uv = [uv1, uv2, uv3, uv4];
					modExtension.MeshesForDrawing[rowNum][colNum] = singleTextureMesh;
				}
			}
			
			if (modExtension.UVCoordsForPrinting.NullOrEmpty())
			{
				Log.Error($"Could not extract UVs for meal {thingDef}, this should never happen.");
			}
			if (modExtension.MeshesForDrawing.NullOrEmpty())
			{
				Log.Error($"Could not extract Meshes for meal {thingDef}, this should never happen.");
			}
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
			return heightPixels - YSize - (Y * YSize + (YPadding * (Y * 2 + 1)));
		}
	}
}
