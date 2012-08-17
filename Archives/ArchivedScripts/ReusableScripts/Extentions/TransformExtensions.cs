using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Finds a Child with a specific name
        /// </summary>
        public static Transform FindSubChild(this Transform argTransform, string argName)
        {
            /// Check to See if Transform Has Children
            if (argTransform.childCount != 0)
            {
                /// Check Local Children
                foreach (Transform t in argTransform)
                {
                    if (t.name == argName)
                        return t;
                }

                /// Check Next Teir
                foreach (Transform t in argTransform)
                {
                    Transform result = t.FindSubChild(argName);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a List of all Children
        /// </summary>
        public static Transform[] GetChildren(this Transform argTransform)
        {
            List<Transform> trans = new List<Transform>();
            foreach (Transform t in argTransform)
                trans.Add(t);
            return trans.ToArray();
        }
    }

}