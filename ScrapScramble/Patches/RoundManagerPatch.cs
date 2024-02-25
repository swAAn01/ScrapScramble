using HarmonyLib;
using UnityEngine;

namespace ScrapScramble
{
    [HarmonyPatch(typeof(RoundManager))]
    class RoundManagerPatch
    {

        private static bool itemsSaved;

        /*
         * In the prefix, we keep a record of all items on the ship.
         */
        [HarmonyPrefix]
        [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound))]
        static void SaveItemsBeforeDespawn()
        {
            if (StartOfRound.Instance.allPlayersDead)
            {
                Debug.Log("All Players Dead. Saving scrap info.");
                int scrapsaved = 0;
                foreach (GrabbableObject g in Object.FindObjectsOfType<GrabbableObject>())
                {
                    if (g.isInShipRoom && g.itemProperties.isScrap)
                    {
                        Debug.Log("Saving a piece of scrap: " + g.itemProperties.itemName);

                        GrabbableObject g2 = Object.Instantiate(g);
                        g2.scrapPersistedThroughRounds = true;
                        ScrapManager.AddItem(g2);
                        scrapsaved++;
                    }
                }
                Debug.Log("Saved " + scrapsaved + " pieces of scrap.");
                itemsSaved = true;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("SpawnOutsideHazards")]
        static void SpawnSavedItems(ref RoundManager __instance)
        {
            if (itemsSaved)
            {
                Debug.Log("Spawning lost items.");
                int itemcount = 0;
                foreach (GrabbableObject g in ScrapManager.GetSavedItems())
                {
                    Vector3 position = __instance.GetRandomNavMeshPositionInRadius(__instance.shipSpawnPathPoints[__instance.shipSpawnPathPoints.Length - 1].position, 30f);

                    GrabbableObject g2 = Object.Instantiate(g, position, Quaternion.identity, __instance.spawnedScrapContainer);
                    g2.transform.rotation = Quaternion.Euler(g.itemProperties.restingRotation);
                    itemcount++;
                }
                Debug.Log("Spawned in " + itemcount + " items.");
            }
            itemsSaved = false;
        }
    }
}
