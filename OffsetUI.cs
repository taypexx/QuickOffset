using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;
using PopupLib;
using PopupLib.UI.Windows;
using PopupLib.UI.Windows.Interfaces;
using UnityEngine.Events;
using CompetitivePlay;
using Il2CppAssets.Scripts.Database;
using System.Globalization;

namespace QuickOffset
{
    internal class OffsetUI
    {
        private static ForumWindow mainMenu;
        public static ForumWindow offsetList;
        private static ForumWindow removeOffset;
        private static InputWindow offsetInput;

        public static List<float> offsets;

        public static bool listOpened => offsetList.Activated;

        public static void Init()
        {
            offsets = Save.Config.Offsets;

            mainMenu = new ForumWindow();
            mainMenu.AutoReset = true;
            mainMenu.OnSelectionChanged += HandleMainMenu;

            mainMenu.ForumObjects.Add(new(
                new("-Offset List-"),
                new("")
            ));

            mainMenu.ForumObjects.Add(new(
                new("-Add Offset-"),
                new("")
            ));

            mainMenu.ForumObjects.Add(new(
                new("-Remove Offset-"),
                new("")
            ));

            mainMenu.ForumObjects.Add(new(
                new("-Exit-"),
                new("")
            ));

            offsetList = new ForumWindow();
            offsetList.AutoReset = true;
            offsetList.OnSelectionChanged += HandleList;

            removeOffset = new ForumWindow();
            removeOffset.AutoReset = true;
            removeOffset.OnSelectionChanged += HandleRemoveOffset;

            offsetInput = new InputWindow();
            offsetInput.AutoReset = true;
            offsetInput.OnCompletion += HandleOffsetInput;
        }

        public static void CreateUI()
        {
            var menuToggle = GameObject.Instantiate(
                GameObject.Find("UI/Standerd/PnlMenu/Panels/PnlOption/Toggles/BtnOffset"),
                GameObject.Find("UI/Standerd/PnlNavigation").transform
            );
            menuToggle.name = "MenuToggle";
            menuToggle.GetComponent<ButtonPointerEnter>().enabled = false;
            menuToggle.GetComponent<Animator>().enabled = false;
            menuToggle.transform.localScale = new(0.25f, 0.25f, 0.25f);
            menuToggle.transform.localPosition = new(750f, 505f, 0f);
            menuToggle.GetComponent<Image>().color = new(0.4f, 1f, 1f, 1f);

            GameObject.Find("UI/Standerd/PnlNavigation/MenuToggle/ImgSelected").SetActive(false);
            GameObject.Find("UI/Standerd/PnlNavigation/MenuToggle/TxtOffset").SetActive(false);

            //var menuToggleText = GameObject.Find("UI/Forward/MenuToggle/TxtOffset").GetComponent<Text>();
            //menuToggleText.text = "Quick Offset";
            //menuToggleText.color = new(0.75f, 0.53f, 1f, 1f);

            var menuToggleImg = GameObject.Find("UI/Standerd/PnlNavigation/MenuToggle/ImgOffset");
            //menuToggleImg.GetComponent<Image>().color = new(0.3f, 1f, 1f, 1f);
            menuToggleImg.transform.localPosition = new(0f, 0f, 0f);

            var menuToggleButton = menuToggle.GetComponent<Button>();
            menuToggleButton.onClick.RemoveAllListeners();

            menuToggleButton.onClick.AddListener((UnityAction)new System.Action(OpenMenu));
        }

        public static int FloatOffsetToInt(float offset)
        {
            return Mathf.FloorToInt(-offset*1000);
        }


        public static void HandleMainMenu(IListWindow window, int objectIndex)
        {
            switch (objectIndex)
            {
                case 0:
                    OpenList();
                    break;
                case 1:
                    OpenAddOffset();
                    break;
                case 2:
                    OpenRemoveOffset();
                    break;
                default:
                    mainMenu.ForceClose();
                    break;
            }
        }

        public static void HandleList(IListWindow window, int objectIndex)
        {
            if (objectIndex == 0)
            {
                OpenMenu();
                return;
            }

            var targOffset = offsets[objectIndex-1];

            DataHelper.offset = FloatOffsetToInt(targOffset);

            PopupLib.UI.PopupUtils.ShowInfo(new("Applied!"));
        }

        public static void HandleRemoveOffset(IListWindow window, int objectIndex)
        {
            if (objectIndex == 0)
            {
                OpenMenu();
                return;
            }

            List<float> newOffsets = new();

            for (int i = 0; i <= offsets.Count-1; i++)
            {
                if (i == objectIndex-1) { continue; }

                newOffsets.Add(offsets[i]);
            }

            offsets = newOffsets;

            PopupLib.UI.PopupUtils.ShowInfo(new("Removed!"));

            removeOffset.ForceClose();
            OpenRemoveOffset();
        }

        public static void HandleOffsetInput(PopupLib.UI.Windows.Abstract.BaseWindow window)
        {
            if (offsetInput.Result == "" || offsetInput.Result == null) {
                OpenMenu();
                return;
            }

            float newOffset;
            newOffset = float.Parse(offsetInput.Result, CultureInfo.InvariantCulture.NumberFormat);

            offsets.Add(newOffset);
            PopupLib.UI.PopupUtils.ShowInfo(new("Added!"));

            OpenMenu();
        }


        public static void OpenList()
        {
            removeOffset.ForceClose();
            mainMenu.ForceClose();
            offsetList.Show();

            offsetList.ForumObjects.Clear();

            offsetList.ForumObjects.Add(new(
                new("-Back-"),
                new("")
            ));

            foreach (var offset in offsets)
            {
                offsetList.ForumObjects.Add(new(
                    new(offset.ToString()),
                    new("")
                ));
            }
        }

        public static void OpenAddOffset()
        {
            mainMenu.ForceClose();
            offsetInput.Show();
        }

        public static void OpenRemoveOffset()
        {
            offsetList.ForceClose();
            mainMenu.ForceClose();
            removeOffset.Show();

            removeOffset.ForumObjects.Clear();

            removeOffset.ForumObjects.Add(new(
                new("-Back-"),
                new("")
            ));

            foreach (var offset in offsets)
            {
                removeOffset.ForumObjects.Add(new(
                    new(offset.ToString()),
                    new("")
                ));
            }
        }

        public static void OpenMenu()
        {
            removeOffset.ForceClose();
            offsetList.ForceClose();
            mainMenu.Show();
        }
    }
}
