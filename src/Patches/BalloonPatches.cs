using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

namespace BalloonFloater.Patches
{
    [HarmonyPatch(typeof(BalloonHoldable))]
    [HarmonyPatch("Grab", MethodType.Normal)]
    internal class BalloonGrabPatch
    {
        internal static void Postfix(BalloonHoldable __instance)
        {
            Player owner = Traverse.Create(__instance).Field("originalOwner").GetValue<Player>();
            if (owner == PhotonNetwork.LocalPlayer)
            {

                Rigidbody rb = Traverse.Create(__instance).Field("rb").GetValue<Rigidbody>();

                if (!Plugin.Instance.bfmanager.rigidbodies.Contains(rb))
                {
                    Plugin.Instance.EquippedBalloons++;
                    Plugin.Instance.bfmanager.rigidbodies.Add(rb);
                }
            }
        }
    }

    [HarmonyPatch(typeof(BalloonHoldable))]
    [HarmonyPatch("OwnerPopBalloon", MethodType.Normal)]
    internal class BalloonOwnerPopPatch
    {
        internal static void Prefix(BalloonHoldable __instance)
        {
            Player owner = Traverse.Create(__instance).Field("originalOwner").GetValue<Player>();
            if (owner == PhotonNetwork.LocalPlayer)
            {
                Rigidbody rb = Traverse.Create(__instance).Field("rb").GetValue<Rigidbody>();
                
                if (Plugin.Instance.bfmanager.rigidbodies.Contains(rb))
                {
                    Plugin.Instance.bfmanager.rigidbodies.Remove(rb);
                }
            }
        }
    }

    [HarmonyPatch(typeof(BalloonHoldable))]
    [HarmonyPatch("Release", MethodType.Normal)]
    internal class BalloonReleasePatch
    {
        internal static void Postfix(BalloonHoldable __instance)
        {
            Player owner = Traverse.Create(__instance).Field("originalOwner").GetValue<Player>();
            if (owner == PhotonNetwork.LocalPlayer)
            {
                Rigidbody rb = Traverse.Create(__instance).Field("rb").GetValue<Rigidbody>();

                if (Plugin.Instance.inRoom)
                {
                    if (Plugin.Instance.bfmanager.rigidbodies.Contains(rb))
                    {
                        Plugin.Instance.EquippedBalloons--;
                        Plugin.Instance.bfmanager.DestroyBalloon(rb);
                    }
                }
            }
        }
    }
}
