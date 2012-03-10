using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SpermGame.Scripts {
    static class ScriptCollection {
        [Pure]
        public static IEnumerable<ScriptBase> GetAllScripts(IEnumerable<ScriptBase> scripts) {
            // TODO Use yield return
            // TODO Watch for circular dependencies and throw

            var loaded = new List<ScriptBase>();

            Action<ScriptBase> load = null;
            load = (script) => {
                if (loaded.Contains(script)) {
                    return;
                }

                var newDeps = script.GetDependencies()
                    .Where((s) => !loaded.Contains(s));

                foreach (var dep in newDeps) {
                    load(dep);
                }

                loaded.Add(script);
            };

            foreach (var script in scripts) {
                load(script);
            }

            return loaded;
        }

        [Pure]
        public static IEnumerable<ScriptBase> GetDependencies(this ScriptBase script) {
            return script.GetType()
                .GetCustomAttributes(typeof(ScriptDependsAttribute), false)
                .OfType<ScriptDependsAttribute>()
                .Select((attr) => attr.Script);
        }
    }
}
