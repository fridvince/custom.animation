using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace fridvince.Game.Common.Animation
{
    [RequireComponent(typeof(AutoHeader))]
    public class AutoStart : MonoBehaviour
    {
        private readonly List<IAutoAnimatable> _autoScripts = new();

        private void Awake()
        {
            FindAutoScriptsRecursively(transform);
        }

        private void FindAutoScriptsRecursively(Transform parent)
        {
            foreach (MonoBehaviour script in parent.GetComponents<MonoBehaviour>())
            {
                if (script is IAutoAnimatable autoScript)
                {
                    _autoScripts.Add(autoScript);
                }
            }

            foreach (Transform child in parent)
            {
                FindAutoScriptsRecursively(child);
            }
        }

        public void StartAll()
        {
            int startedCount = 0;

            foreach (IAutoAnimatable autoScript in _autoScripts)
            {
                if (autoScript is MonoBehaviour script)
                {
                    MethodInfo startMethod = FindStartMethod(script);
                    if (startMethod != null)
                    {
                        startMethod.Invoke(script, null);
                        startedCount++;
                        Debug.Log($"Started {startMethod.Name} on {script.gameObject.name} ({script.GetType().Name})");
                    }
                }
            }

            Debug.Log($"Started {startedCount} auto animations.");
        }

        private MethodInfo FindStartMethod(MonoBehaviour script)
        {
            MethodInfo[] methods = script.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo method in methods)
            {
                if (method.Name.StartsWith("Start"))
                {
                    return method;
                }
            }

            return null;
        }
    }

    public interface IAutoAnimatable
    {
        void StartAnimation();
    }
}
