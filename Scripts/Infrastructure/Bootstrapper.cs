using UnityEngine;

namespace Infrastructure
{
    public static class Bootstrapper
    {
        private const string RootName = "GameRoot";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Execute() =>
            Object.Instantiate(Resources.Load(RootName));
    }
}
