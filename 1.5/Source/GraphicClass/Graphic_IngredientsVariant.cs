namespace DynamicMealTextureReplacer
{
	public class Graphic_IngredientsVariant : Graphic_Single
	{
		//public override Material MatSingle => MealAtlasWorker.extractedMealTextures[3] ?? base.MatSingle;
		internal ThingDef parent;

		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			ThingComp_Gizmo gizmo = thing.TryGetComp<ThingComp_Gizmo>();
			int CounterY = gizmo.CounterY;
			int CounterX = gizmo.CounterX;
			
			Material mat = parent.GetModExtension<ModExtension_DynamicMealTextureReplacer>()?.TextureVariants[CounterY][CounterX];
			Printer_Plane.PrintPlane(layer, thing.TrueCenter(), new Vector2(1f, 1f), mat, extraRotation);
		}
	}
}
