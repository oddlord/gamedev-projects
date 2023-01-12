using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace SpaceMiner
{
    public static class Utils
    {
        public static T GetRandomArrayElement<T>(T[] elements)
        {
            return elements[UnityEngine.Random.Range(0, elements.Length)];
        }

        public static T[] ShuffleArray<T>(T[] elements)
        {
            return elements.OrderBy(a => Guid.NewGuid()).ToArray();
        }

        public static Quaternion GetRandom2DRotation()
        {
            float z = UnityEngine.Random.Range(0, 360);
            return Quaternion.Euler(0, 0, z);
        }

        public static void SetLocalizedString(LocalizeStringEvent localizeString, string key, Dictionary<string, object> args = null, string table = null)
        {
            localizeString.StringReference.Arguments = new List<object> { args };
            if (!string.IsNullOrEmpty(table)) localizeString.SetTable(table);
            localizeString.SetEntry(key);
            localizeString.StringReference.RefreshString();
        }
    }
}
