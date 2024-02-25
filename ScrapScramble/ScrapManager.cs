using System;
using System.Collections.Generic;

namespace ScrapScramble
{
    class ScrapManager
    {
        private static List<GrabbableObject> LostItems;

        public static void AddItem(GrabbableObject g)
        {
            if (LostItems == null) LostItems = new List<GrabbableObject>();

            LostItems.Add(g);
        }

        public static GrabbableObject[] GetSavedItems()
        {
            return LostItems == null ? null : LostItems.ToArray();
        }

        public static void ClearSavedItems()
        {
            if (LostItems != null) LostItems.Clear();
        }
    }
}
