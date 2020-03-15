﻿using Harmony;
using UnityEngine;

namespace EXILED.Patches
{
    [HarmonyPatch(typeof(Scp106PlayerScript), nameof(Scp106PlayerScript.CallCmdMakePortal))]
    public class CallCmdMakePortalOverride
    {
        public static bool Prefix(Scp106PlayerScript __instance)
        {
            if (EventPlugin.Scp106CreatedPortalEventDisable)
                return true;

            if (!__instance._interactRateLimit.CanExecute(true))
                return false;

            if (!__instance.GetComponent<FallDamage>().isGrounded)
                return false;

            bool rayCastHit = Physics.Raycast(new Ray(__instance.transform.position, -__instance.transform.up), out RaycastHit raycastHit, 10f, __instance.teleportPlacementMask);

            bool allow = true;
            Vector3 portalPosition = raycastHit.point - Vector3.up;

            Events.InvokeScp106CreatedPortal(__instance.gameObject, ref allow, ref portalPosition);

            Debug.DrawRay(__instance.transform.position, -__instance.transform.up, Color.red, 10f);

            if (allow && __instance.iAm106 && !__instance.goingViaThePortal && rayCastHit)
                __instance.SetPortalPosition(portalPosition);

            return false;
        }
    }
}
