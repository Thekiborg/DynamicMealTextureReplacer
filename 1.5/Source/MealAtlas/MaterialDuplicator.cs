namespace DynamicMealTextureReplacer
{
	internal static class MaterialDuplicator
	{
		public static Material DuplicateMaterial(Material material)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to duplicate material outside the main thread");
				return null;
			}
			Material duplicatedMat = new(material);
			return duplicatedMat;
		}
	}
}
