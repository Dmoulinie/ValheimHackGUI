using UnityEngine;

namespace ValheimHackGUI
{
    public class Loader
    {

        public static void Init()
        {
            Loader.Load = new GameObject();
            Loader.Load.AddComponent<Hacks>();
            UnityEngine.Object.DontDestroyOnLoad(Loader.Load);
        }

        public static void Unload()
        {
            GameObject.Destroy(Load);
        }
        private static GameObject Load;
    }
}
