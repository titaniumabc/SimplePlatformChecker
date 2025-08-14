using System;
using BepInEx;
using UnityEngine;
using Utilla;
using HarmonyLib;

namespace PlatformChecker
{
    internal class PluginInfo
    {
        public const string GUID = "com.titanium.gorillatag.platformchecker";
        public const string Name = "PlatformChecker";
        public const string Version = "1.0.0";
    }
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance = new Plugin();
        float cooldown = 2f;
        void FixedUpdate()
        {
            if (Time.time > cooldown)
            {
                RunChecks();
                cooldown += 2f;
            }
        }

        void UpdateName(VRRig rig, string Key, string color)
        {
            rig.playerText1.text = rig.Creator.NickName.ToUpper();
            rig.playerText2.text = rig.Creator.NickName.ToUpper();
            if (rig.fps < 55)
            {
                rig.playerText1.text += $"\nFPS: <color=#fc0000>{rig.fps.ToString()}</color>\n<color={color}>{Key}</color>";
                rig.playerText2.text += $"\nFPS: {rig.fps.ToString()}\n{Key}";
            }
            else
            {
                rig.playerText1.text += $"\nFPS: <color=#5cff69>{rig.fps.ToString()}</color>\n<color={color}>{Key}</color>";
                rig.playerText2.text += $"\nFPS: {rig.fps.ToString()}\n{Key}";
            }
        }

        public void RunChecks()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig.concatStringOfCosmeticsAllowed.Contains("first login"))
                {
                    UpdateName(rig, "STEAMVR", "#640aff");
                }
                else if (rig.concatStringOfCosmeticsAllowed.Contains("game-purchase"))
                {
                    UpdateName(rig, "OCULUS PCVR", "#f700ca");
                }
                else
                {
                    UpdateName(rig, "QUEST", "#00fff2");
                }
            }
        }
    }

    [HarmonyPatch(typeof(VRRigCache))]
    class VRRigCachePatches
    {
        [HarmonyPatch(nameof(VRRigCache.OnJoinedRoom))]
        [HarmonyPrefix]
        static bool OnJoinedRoom()
        {
            Plugin.Instance.RunChecks();
            return true;
        }
        [HarmonyPatch(nameof(VRRigCache.OnPlayerEnteredRoom))]
        [HarmonyPrefix]
        static bool OnPlayerEnteredRoom()
        {
            Plugin.Instance.RunChecks();
            return true;
        }
        [HarmonyPatch(nameof(VRRigCache.OnPlayerLeftRoom))]
        [HarmonyPrefix]
        static bool OnPlayerLeftRoom()
        {
            Plugin.Instance.RunChecks();
            return true;
        }
    }
}
