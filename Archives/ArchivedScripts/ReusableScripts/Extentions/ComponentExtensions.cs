using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public static class ComponentExtensions
    {
        #region Renderer

        /// <summary>
        /// Finds the Renderer of any Given Component
        /// </summary>
        public static Renderer GetRenderer(this Component argComponent)
        {
            /// Check Current Component For Renderer
            Renderer result = argComponent.renderer;
            if (result != null)
                return result;

            /// Check to See if Transform Has Children
            Transform trans = argComponent.transform;
            if (trans.childCount != 0)
            {
                /// Check Next Teir For Renderer
                foreach (Transform t in trans)
                {
                    result = t.GetRenderer();
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the Renderer of any Given Component
        /// </summary>
        public static Renderer[] GetRenderers(this Component argComponent)
        {
            List<Renderer> results = new List<Renderer>();

            /// Check Current Component For Renderer
            Renderer result = argComponent.renderer;
            if (result != null)
                results.Add(result);

            /// Check to See if Transform Has Children
            Transform trans = argComponent.transform;
            if (trans.childCount != 0)
            {
                /// Check Next Teir For Renderer
                foreach (Transform t in trans)
                {
                    result = t.GetRenderer();
                    if (result != null)
                        results.Add(result);
                }
            }

            return results.ToArray();
        }

        #endregion

        #region Components

        /// <summary>
        /// Gets Components or Components with Interface
        /// </summary>
        public static Component[] GetInterfaceComponents<T>(this Component argComponent)
        {
            return argComponent.GetComponents<Component>().Where(x => x is T).ToArray();
        }

        /// <summary>
        /// Gets Component or Component with Interface
        /// </summary>
        public static Component GetInterfaceComponent<T>(this Component argTransform)
        {
            return argTransform.GetComponents<Component>().Single(x => x is T);
        }

        #endregion
    }

}