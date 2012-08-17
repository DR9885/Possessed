using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine
{
    public static class GameObjectExtensions
    {

        public static IEnumerable<Component> GetInterfaceComponents<T>(this GameObject argGameObject)
        {
            return argGameObject.GetComponents<Component>().Where(
                component => component is T);
        }

        public static Component GetInterfaceComponent<T>(this GameObject argGameObject)
        {
            return argGameObject.GetComponents<Component>().Single(
                component => component is T);

        }

        /// <summary>
        /// Search to find the component on the corresponding game object.
        /// If the component is not found an exception is thrown.
        /// </summary>
        /// <typeparam name="T"> The Component to get </typeparam>
        /// <param name="argMainObject"> The object that should contain the component </param>
        /// <param name="argObject"> possible fallback objects that could contain the component </param>
        /// <returns> T if found </returns>
        public static T FindComponent<T>(this GameObject argGameObject, params Transform[] argOtherObjects) where T : Component
        {
            T myResult = FindComponentInternal<T>(argGameObject, argOtherObjects);

//            /// If still null, Display Error
//            if (myResult == null)
//                //debug.logError("FindComponent Failed: " + argMainObject.name + "'s " + myResult.GetType().Name);

            return myResult;
        }

        /// <summary>
        /// Search to find the component on the corresponding game object.
        /// If the component is not found it is added to the game object.
        /// </summary>
        /// <typeparam name="T"> The Component to get or add </typeparam>
        /// <param name="argMainObject"> The object that should contain the component </param>
        /// <param name="argObject"> possible fallback objects that could contain the component </param>
        /// <returns> T if found </returns>
        public static T RequireComponentAdd<T>(this GameObject argMainObject, params Transform[] argOtherObjects) where T : Component
        {
            /// Search for component
            T myResult = FindComponentInternal<T>(argMainObject, argOtherObjects);

            /// Add Component to Object
            if (myResult == null)
                myResult = argMainObject.AddComponent(typeof(T)) as T;

//            /// If still null, Display Error
//            if (myResult == null)
//                //debug.logError("RequireComponentAdd Failed: " + argMainObject.name + "'s " + myResult.GetType().Name);

            return myResult;
        }
		
		public static T FindInstance<T>() where T : Component
		{
			UnityEngine.Object obj = GameObject.FindObjectOfType(typeof(T));
			return obj as T;
//			T myInstance = GameObject.FindObjectOfType(typeof(T)) as T;
//			return myInstance;
		}
		
        /// <summary>
        /// Search to find the gameObject in the world.
        /// If the gameObject is not found it is generated.
        /// 
        /// USED FOR MONOBEHAVIOR *SINGLETONS*
        /// </summary>
        /// <typeparam name="T"> The Instance Type </typeparam>
        /// <returns> result </returns>
        public static T RequireInstance<T>() where T : Component
        {
            /// Search for GameObject
            T myInstance = GameObject.FindObjectOfType(typeof(T)) as T;

            /// If Null, Generate
            if (myInstance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                myInstance = obj.AddComponent(typeof(T)) as T;
            }

//            /// If still null, Display Error
//            if (myInstance == null)
//                //debug.logError("RequireComponentAdd Failed: " + typeof(T).Name);

            return myInstance;
        }


        /// <summary>
        /// Search to find the component on the corresponding game object.
        /// If the component is not found an exception is thrown.
        /// </summary>
        /// <typeparam name="T"> The Component to get </typeparam>
        /// <param name="argMainObject"> The object that should contain the component </param>
        /// <param name="argObject"> possible fallback objects that could contain the component </param>
        /// <returns> T if found </returns>
        private static T FindComponentInternal<T>(GameObject argMainObject, params Transform[] argOtherObjects) where T : Component
        {
            T myResult = null;

            /// Find in main
            myResult = argMainObject.GetComponent<T>();

            /// Find in others
            if (myResult == null)
                foreach (Transform obj in argOtherObjects)
                {
                    if (obj != null)
                    {
                        myResult = obj.GetComponent<T>();
                        if (myResult)
                            break;
                    }
                }

            /// Find in all main children
            if (myResult == null)
                argMainObject.GetComponentInChildren<T>();

            return myResult;
        }

    }
}