namespace DynamicMealTextureReplacer
{
#pragma warning disable CA1051,CA1002
	public class ModExtension_DynamicMealTextureReplacer : DefModExtension
	{
		internal Material[][] TextureVariants;

		public Dictionary<List<ThingDef>, int> dimensionsMapping;
		public int maxCheckedIngredients = 1;
		public float heightPixels;
		public float widthPixels;
	}
}
