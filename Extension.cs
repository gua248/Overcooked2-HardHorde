using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine.UI;

namespace OC2HardHorde.Extension
{
    public static class FrontendRootMenuExtension
    {
        private static readonly FieldInfo fieldInfo_m_CurrentGamepadUser = AccessTools.Field(typeof(FrontendRootMenu), "m_CurrentGamepadUser");
        private static readonly MethodInfo methodInfo_OnMenuHide = AccessTools.Method(typeof(FrontendRootMenu), "OnMenuHide");
        private static readonly MethodInfo methodInfo_OnMenuShow = AccessTools.Method(typeof(FrontendRootMenu), "OnMenuShow");
        
        public static GamepadUser get_m_CurrentGamepadUser(this FrontendRootMenu instance)
        {
            return (GamepadUser)fieldInfo_m_CurrentGamepadUser.GetValue(instance);
        }

        public static void OnMenuShow(this FrontendRootMenu instance, BaseMenuBehaviour menu)
        {
            methodInfo_OnMenuShow.Invoke(instance, new object[] { menu });
        }

        public static void OnMenuHide(this FrontendRootMenu instance, BaseMenuBehaviour menu)
        {
            methodInfo_OnMenuHide.Invoke(instance, new object[] { menu });
        }
    }

    public static class FrontendOptionsMenuExtension
    {
        private static readonly FieldInfo fieldInfo_m_ConsoleTopSelectable = AccessTools.Field(typeof(FrontendOptionsMenu), "m_ConsoleTopSelectable");
        private static readonly FieldInfo fieldInfo_m_SyncOptions = AccessTools.Field(typeof(FrontendOptionsMenu), "m_SyncOptions");
        private static readonly FieldInfo fieldInfo_m_discard = AccessTools.Field(typeof(FrontendOptionsMenu), "m_discard");
        private static readonly FieldInfo fieldInfo_m_VersionString = AccessTools.Field(typeof(FrontendOptionsMenu), "m_VersionString");
        public static void set_m_VersionString(this FrontendOptionsMenu instance, T17Text text)
        {
            fieldInfo_m_VersionString.SetValue(instance, text);
        }

        public static void set_m_ConsoleTopSelectable(this FrontendOptionsMenu instance, Selectable selectable)
        {
            fieldInfo_m_ConsoleTopSelectable.SetValue(instance, selectable);
        }

        public static ISyncUIWithOption[] get_m_SyncOptions(this FrontendOptionsMenu instance)
        {
            return (ISyncUIWithOption[])fieldInfo_m_SyncOptions.GetValue(instance);
        }

        public static void set_m_SyncOptions(this FrontendOptionsMenu instance, ISyncUIWithOption[] options)
        {
            fieldInfo_m_SyncOptions.SetValue(instance, options);
        }

        //public static void set_m_discard(this FrontendOptionsMenu instance, bool discard)
        //{
        //    fieldInfo_m_discard.SetValue(instance, discard);
        //}
    }

    public static class BaseUIOptionExtension
    {
        private static readonly FieldInfo fieldInfo_m_OptionType = AccessTools.Field(typeof(BaseUIOption<IOption>), "m_OptionType");
        private static readonly FieldInfo fieldInfo_m_Option = AccessTools.Field(typeof(BaseUIOption<IOption>), "m_Option");

        public static void set_m_OptionType(this BaseUIOption<IOption> instance, OptionsData.OptionType type)
        {
            fieldInfo_m_OptionType.SetValue(instance, type);
        }

        public static void set_m_Option(this BaseUIOption<IOption> instance, IOption option)
        {
            fieldInfo_m_Option.SetValue(instance, option);
        }
    }
}
