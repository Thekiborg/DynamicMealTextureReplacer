using System.Linq;

namespace DynamicMealTextureReplacer
{
	public class ThingComp_Gizmo : ThingComp
	{
		ModExtension_DynamicMealTextureReplacer ModExtension => parent.def.GetModExtension<ModExtension_DynamicMealTextureReplacer>();

		public int CounterY { get; set; }
		public int CounterX { get; set; }

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Action command_Action = new()
			{
				defaultLabel = "Change graphic",
				action = () =>
				{
					CounterY = MealAtlasIngredientFilter.GetRow(ModExtension, parent.GetComp<CompIngredients>());
					
					if (CounterX < ModExtension.dimensionsMapping.ElementAt(ModExtension.dimensionsMapping.Count - 1 - CounterY).Value - 1)
					{
						CounterX++;
					}
					else
					{
						CounterX = 0;
					}

					parent.DirtyMapMesh(parent.Map);
				},
			};
			yield return command_Action;
		}
	}
}
