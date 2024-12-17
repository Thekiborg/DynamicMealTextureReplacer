namespace DynamicMealTextureReplacer
{
	public class Graphic_IngredientsVariant : Graphic_Single
	{
		private const int randomRangeMin = 0;
		private readonly Dictionary<Thing, (ModExtension_DynamicMealTextureReplacer ModExtension, CompIngredients CompIngredients)> modExtensionCompCache = [];

		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			if (thing is null) return;
			CacheCompAndModExtension(thing);

			int row = MealAtlasIngredientFilter.GetRow(modExtensionCompCache[thing].ModExtension, modExtensionCompCache[thing].CompIngredients);

			if (row > -1)
			{
				int col = GetRandomTextureOnRow(thing, row, modExtensionCompCache[thing].ModExtension);

				Printer_Plane.PrintPlane(layer,
					thing.TrueCenter(),
					thing.DrawSize,
					MatSingleFor(thing),
					extraRotation,
					uvs: modExtensionCompCache[thing].ModExtension.UVCoordsForPrinting[row][col]);
			}
			else
			{
				data.attachments[0].Graphic.Print(layer, thing, extraRotation);
			}

		}

		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (thing is null) return;

			CacheCompAndModExtension(thing);

			int row = MealAtlasIngredientFilter.GetRow(modExtensionCompCache[thing].ModExtension, modExtensionCompCache[thing].CompIngredients);

			if (row > -1)
			{
				int col = GetRandomTextureOnRow(thing, row, modExtensionCompCache[thing].ModExtension);

				Mesh mesh = modExtensionCompCache[thing].ModExtension.MeshesForDrawing[row][col];
				Quaternion quat = QuatFromRot(rot);
				if (extraRotation != 0f)
				{
					quat *= Quaternion.Euler(Vector3.up * extraRotation);
				}
				if (data != null && data.addTopAltitudeBias)
				{
					quat *= Quaternion.Euler(Vector3.left * 2f);
				}
				loc += DrawOffset(rot);
				Material mat = MatSingleFor(thing);
				DrawMeshInt(mesh, loc, quat, mat);
				ShadowGraphic?.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
			else
			{
				data.attachments[0].Graphic.Draw(loc, rot, thing, extraRotation);
			}

		}

		private static int GetRandomTextureOnRow(Thing thing, int row, ModExtension_DynamicMealTextureReplacer modExtension)
		{
			int randomRangeMax = modExtension.UVCoordsForPrinting[row].Length;
			Log.Message("row index: " + row + "col number: " + randomRangeMax);
			int seed = thing.thingIDNumber; //% modExtension.TextureVariants[row].Length;
			return Rand.RangeSeeded(randomRangeMin, randomRangeMax, seed);
		}

		private void CacheCompAndModExtension(Thing thing)
		{
			if (!modExtensionCompCache.ContainsKey(thing))
			{
				var modExtension = thing?.def.GetModExtension<ModExtension_DynamicMealTextureReplacer>();
				var compIngredients = thing.TryGetComp<CompIngredients>();
				modExtensionCompCache.Add(thing, new(modExtension, compIngredients));
			}
		}
	}
}
