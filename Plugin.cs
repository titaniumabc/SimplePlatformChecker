using System;
using BepInEx;
using UnityEngine;
using Utilla;
using HarmonyLib;

namespace PlatformChecker
{
    [BepInPlugin("titanium.simpleplatformchecker", "SimplePlatformChecker", "1.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance = new Plugin();
        
        void Update() => RunChecks();
        
        void UpdateName(VRRig rig, string Key, string color)
        {
            rig.playerText1.text = rig.Creator.NickName.ToUpper();
            rig.playerText2.text = rig.Creator.NickName.ToUpper();
            if (rig.fps <= 55)
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
                if (rig.concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                {
                    UpdateName(rig, "STEAM", "#640aff");
                }
                else if (concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") || rig.Creator.GetPlayerRef().CustomProperties.Count >= 2)
                {
                    UpdateName(rig, "PC", "#f700ca");
                }
                else
                {
                    UpdateName(rig, "QUEST?", "#00fff2");
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

