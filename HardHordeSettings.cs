using BepInEx;
using HarmonyLib;
using OC2HardHorde.Extension;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace OC2HardHorde
{
    public static class HardHordeSettings
    {
        public static bool enabled = false;
        private static FrontendOptionsMenu gameSettingsMenu = null;
        private static FrontendOptionsMenu modSettingsMenu = null;
        public static ToggleOption hardHordeOption = null;

        private static void AddHardHordeSettingsUI()
        {
            if (modSettingsMenu == null || hardHordeOption != null) return;
            GameObject hardHordeOptionObj = GameObject.Instantiate(gameSettingsMenu.transform.Find("SettingsBody/ContentPC/Viewport/Content/Windowed").gameObject);
            hardHordeOptionObj.name = "HardHorde";
            hardHordeOptionObj.transform.SetParent(modSettingsMenu.transform.Find("SettingsBody/ContentPC/Viewport/Content"), false);
            T17Text text = hardHordeOptionObj.transform.Find("Title").GetComponent<T17Text>();
            if (Localization.GetLanguage() == SupportedLanguages.Chinese)
            {
                text.text = "困难敌群";
                text.m_LocalizationTag = "\"困难敌群\"";
            }
            else
            {
                text.text = "HardHorde";
                text.m_LocalizationTag = "\"HardHorde\"";
            }
            hardHordeOption = hardHordeOptionObj.GetComponent<ToggleOption>();
            hardHordeOption.set_m_Option(null);
            hardHordeOption.set_m_OptionType((OptionsData.OptionType)(-1));
            modSettingsMenu.set_m_SyncOptions(modSettingsMenu.get_m_SyncOptions().AddToArray(hardHordeOption));
            hardHordeOptionObj.GetComponent<T17Toggle>().isOn = enabled;
        }

        private static void AddModSettingsUI()
        {
            GameObject root = GameObject.Find("FrontendRootMenu");
            if (root == null) return;
            FrontendRootMenu frontendRootMenu = root.GetComponent<FrontendRootMenu>();
            gameSettingsMenu = root.transform.Find("FixedAspectRootCanvas/ScreenSpaceCanvas/GameOptions").GetComponent<FrontendOptionsMenu>();
            modSettingsMenu = root.transform.Find("FixedAspectRootCanvas/ScreenSpaceCanvas/ModOptions")?.GetComponent<FrontendOptionsMenu>();
            if (modSettingsMenu != null) return;

            Transform settingsTab = root.transform.Find("WorldSpaceCanvas/Safezone_Rootmenu/RootTab/SettingsOptions");
            float dh = settingsTab.Find("GameSettings").GetComponent<RectTransform>().rect.height;
            RectTransform rect = settingsTab.GetComponent<RectTransform>();
            float h = rect.rect.height;
            rect.offsetMin += new Vector2(0, -dh);
            float py = rect.pivot.y;
            rect.pivot = new Vector2(rect.pivot.x, (h * py + dh) / (h + dh));
            T17Button modSettingsButton = GameObject.Instantiate(settingsTab.Find("GameSettings").gameObject).GetComponent<T17Button>();
            modSettingsButton.gameObject.name = "ModSettings";
            modSettingsButton.transform.SetParent(settingsTab, false);
            modSettingsButton.transform.SetSiblingIndex(3);
            settingsTab.GetChild(4).localPosition += new Vector3(0, -dh, 0);

            T17Button credits = settingsTab.Find("Credits").GetComponent<T17Button>();
            Navigation navigation1 = credits.navigation;
            navigation1.selectOnDown = modSettingsButton;
            credits.navigation = navigation1;
            Navigation navigation2 = modSettingsButton.navigation;
            navigation2.selectOnDown = null;
            navigation2.selectOnUp = credits;
            modSettingsButton.navigation = navigation2;

            T17Text text = modSettingsButton.transform.Find("Text").GetComponent<T17Text>();
            if (Localization.GetLanguage() == SupportedLanguages.Chinese)
            {
                text.text = "MOD";
                text.m_LocalizationTag = "\"MOD\"";
            }
            else
            {
                text.text = "MODs";
                text.m_LocalizationTag = "\"MODs\"";
            }

            GameObject modSettingsMenuObj = GameObject.Instantiate(gameSettingsMenu.gameObject);
            modSettingsMenuObj.name = "ModOptions";
            modSettingsMenuObj.SetActive(false);
            modSettingsMenuObj.transform.SetParent(gameSettingsMenu.transform.parent, false);
            modSettingsMenuObj.transform.SetSiblingIndex(2);
            modSettingsMenu = modSettingsMenuObj.GetComponent<FrontendOptionsMenu>();
            text = modSettingsMenu.transform.Find("SettingsBody/HeaderBacker/Header").GetComponent<T17Text>();
            if (Localization.GetLanguage() == SupportedLanguages.Chinese)
            {
                text.text = "MOD设定";
                text.m_LocalizationTag = "\"MOD设定\"";
            }
            else
            {
                text.text = "MOD SETTINGS";
                text.m_LocalizationTag = "\"MOD SETTINGS\"";
            }
            Transform settingsBody = modSettingsMenu.transform.Find("SettingsBody");
            settingsBody.Find("Cancel").gameObject.Destroy();
            settingsBody.Find("Confirm").gameObject.Destroy();
            settingsBody.Find("ContentConsole").gameObject.Destroy();
            settingsBody.Find("VersionNumber").gameObject.Destroy();
            Transform content = settingsBody.Find("ContentPC/Viewport/Content");
            for (int i = content.childCount - 1; i >= 0; i--)
                content.GetChild(i).gameObject.Destroy();
            rect = settingsBody.Find("ContentPC").GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0);
            modSettingsMenu.DoSingleTimeInitialize();
            modSettingsMenu.Hide(true, false);
            modSettingsMenu.set_m_ConsoleTopSelectable(null);
            modSettingsMenu.set_m_SyncOptions(new ISyncUIWithOption[0]);
            modSettingsMenu.set_m_VersionString(null);
            modSettingsMenu.OnShow = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(modSettingsMenu.OnShow, new BaseMenuBehaviour.BaseMenuBehaviourEvent(frontendRootMenu.OnMenuShow));
            modSettingsMenu.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(modSettingsMenu.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(frontendRootMenu.OnMenuHide));

            Button.ButtonClickedEvent buttonClickedEvent = new Button.ButtonClickedEvent();
            buttonClickedEvent.AddListener(delegate ()
            {
                GamepadUser user = frontendRootMenu.get_m_CurrentGamepadUser();
                modSettingsMenu.Show(user, frontendRootMenu, root, false);
            });
            modSettingsButton.onClick = buttonClickedEvent;
        }

        public static void AddUI()
        {
            AddModSettingsUI();
            AddHardHordeSettingsUI();
        }
    }
}