namespace DynamicMealTextureReplacer
{
#pragma warning disable CA2211
	[DefOf]
	public static class DMTRDefOf
	{
		public static ThingDef DMTR_FillWithAllMeatDefs;
		public static ThingDef DMTR_FillWithAllInsectMeatDefs;
		public static ThingDef DMTR_FillWithAllEggDefs;

		static DMTRDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DMTRDefOf));
		}
	}
}
