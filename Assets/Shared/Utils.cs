using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceMiner
{
    public static class Utils
    {
        public static T GetRandomListElement<T>(List<T> elements)
        {
            return elements[UnityEngine.Random.Range(0, elements.Count)];
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
    }
}
