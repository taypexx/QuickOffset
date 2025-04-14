using CompetitivePlay;
using Il2CppAssets.Scripts.Database;
using MelonLoader;
using UnityEngine;

namespace QuickOffset
{
    public class Main : MelonMod
    {
        public static bool isMenuScene = false;
        public override void OnInitializeMelon()
        {
            Save.Load();
            OffsetUI.Init();

            MelonLogger.Msg("QuickOffset initialized!");

            base.OnInitializeMelon();
        }

        public override void OnDeinitializeMelon()
        {
            Save.Config.Offsets = OffsetUI.offsets;
            Save.Save_();

            MelonLogger.Msg("Saved QuickOffset settings!");

            base.OnDeinitializeMelon();
        }

        public override void OnUpdate()
        {
            if (isMenuScene)
            {
                if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), Save.Config.OpenHotkey)))
                {
                    if (OffsetUI.listOpened)
                    {
                        OffsetUI.offsetList.ForceClose();
                    }
                    else
                    {
                        OffsetUI.OpenList();
                    }
                }
            }

            base.OnUpdate();
        }


        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            isMenuScene = sceneName == "UISystem_PC";

            if (isMenuScene)
            {
                OffsetUI.CreateUI();
            }

            base.OnSceneWasLoaded(buildIndex, sceneName);
        }
    }
}
