using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
using System.Linq;
// using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditor.Animations;


namespace ThunderNut.Extensions {
    public enum LogColor{ Red, Blue, Green, White, None }

    /// <summary>
    /// Useful functions
    /// </summary>
    public static class Helper {

        #region GameObject Extensions

        /// <summary>
        /// Returns the first gameabject in children with a specific name.
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="FindInChildren"/> method.
        /// <code>
        /// GameObject myobj = gameObject.FindInChildren("myobjectname");
        /// </code>
        public static GameObject FindInChildren(this GameObject go, string name) {
            return (from x in go.GetComponentsInChildren<Transform>()
                where Animator.StringToHash(x.gameObject.name) == Animator.StringToHash(name)
                select x.gameObject).First();
        }


        /// <summary>
        /// Sets all the children gameobjects with a specific game component to true or false.
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="SetActiveAllChildren"/> method.
        /// <code>
        /// gameObject.SetActiveAllChildren<Transform>(false);
        /// </code>
        /// </example>
        public static void SetActiveAllChildren<T>(this GameObject go, bool state) where T : Component {
            // go.GetComponentsInChildren<T>().ToList().ForEach(x => x.gameObject.SetActive(state));
            go.GetComponentsInChildren<T>().ForEach(x => x.gameObject.SetActive(false));
            go.SetActive(true);
        }


        /// <summary>
        /// Destroys all the children gameobjects with a specific game component.
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="DestroyAllChildren"/> method.
        /// <code>
        /// gameObject.DestroyAllChildren<Transform>();
        /// </code>
        /// </example>
        public static void DestroyAllChildren<T>(this GameObject go) where T : Component {
            go.GetComponentsInChildren<T>().ForEach(x => {
                    if (Animator.StringToHash(x.name) != Animator.StringToHash(go.name))
                        UnityEngine.Object.Destroy(x.gameObject);
                }
            );
        }

        #endregion

        #region Animator Extensions

        /// <summary>
        /// Returns all the state names from an animator as a string array.
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="GetStateNames"/> method.
        /// <code>
        /// Animator anim;
        /// AnimatorState[] myStateNames = GetStateNames(anim);
        /// string stateName = myStateNames[0];
        /// </code>
        /// </example>
        public static AnimatorState[] GetStateNames(Animator animator) {
            var controller = animator ? animator.runtimeAnimatorController as AnimatorController : null;
            return controller == null
                ? null
                : controller.layers.SelectMany(l => l.stateMachine.states).Select(s => s.state).ToArray();
        }


        /// <summary>
        /// Checks if animator is playing on a layer
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="IsPlayingOnLayer"/> method.
        /// <code>
        /// WIP
        /// </code>
        /// </example>
        public static bool IsPlayingOnLayer(this Animator animator, int fullPathHash, int layer) {
            return animator.GetCurrentAnimatorStateInfo(layer).fullPathHash == fullPathHash;
        }


        /// <summary>
        /// Returns normalized time of animator on layer
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="NormalizedTime"/> method.
        /// <code>
        /// WIP
        /// </code>
        /// </example>
        public static float NormalizedTime(this Animator animator, int layer) {
            float time = animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
            return time > 1 ? 1 : time;
        }

        #endregion

        #region Generic Extensions

        /// <summary>
        /// Allows a loop with the item and index. <para />
        /// <example>
        /// This sample shows how to call the <see cref="WithIndex"/> method.
        /// <code>
        /// foreach (var (item, index) in collection.WithIndex()) {
        ///     DoSomething(item, index) 
        /// }
        /// </code>
        /// </example>
        /// </summary>
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source) {
            return source.Select((item, index) => (item, index));
        }


        /// <summary>
        /// Allows one line ForEach statement on Arrays and other IEnumerables
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="ForEach"/> method.
        /// <code>
        /// int[] array = new int[] {1, 2, 3};
        /// array.ForEach(x => Debug.Log(x));
        /// </code>
        /// </example>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var item in source)
                action(item);
        }

        public static void SetAndStretchToParentSize(this RectTransform _mRect, RectTransform _parent) {
            _mRect.anchoredPosition = _parent.position;
            _mRect.anchorMin = new Vector2(1, 0);
            _mRect.anchorMax = new Vector2(0, 1);
            _mRect.pivot = new Vector2(0.5f, 0.5f);
            _mRect.sizeDelta = _parent.rect.size;
            _mRect.transform.SetParent(_parent);
        }

        public static bool Raycast(Vector2 origin, Vector2 direction, float distance, LayerMask layer,
            out RaycastHit2D ray) =>
            ray = Physics2D.Raycast(origin, direction, distance, layer);

        public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay) {
            mono.StartCoroutine(CallWithDelayRoutine(method, delay));
        }
        private static IEnumerator CallWithDelayRoutine(Action method, float delay) {
            yield return new WaitForSeconds(delay);
            method();
        }
        
        public static string WithColour(this string text, Color colour) {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(colour)}>{text}</color>";
        }

        public static void CustomLog(string text, LogColor color) {
            switch (color) {
                case LogColor.White:
                    Debug.Log($"<b><color=white>{text}</color></b>");
                    break;
                case LogColor.Blue:
                    Debug.Log($"<b><color=blue>{text}</color></b>");
                    break;
                case LogColor.Green:
                    Debug.Log($"<b><color=green>{text}</color></b>");
                    break;
                case LogColor.Red:
                    Debug.Log($"<b><color=red>{text}</color></b>");
                    break;
                case LogColor.None:
                    Debug.Log($"{text}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(color), color, null);
            }
        }

        #endregion

    }
}
