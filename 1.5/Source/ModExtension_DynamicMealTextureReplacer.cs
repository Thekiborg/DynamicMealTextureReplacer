﻿namespace DynamicMealTextureReplacer
{
#pragma warning disable CA1051,CA1002
	public class ModExtension_DynamicMealTextureReplacer : DefModExtension
	{
		internal Vector2[][][] UVCoordsForPrinting;
		internal Mesh[][] MeshesForDrawing;

		public Dictionary<ThingFilter, int> dimensionsMapping;
		public int maxCheckedIngredients = 1;
		public float heightPixels;
		public float widthPixels;
	}
}
