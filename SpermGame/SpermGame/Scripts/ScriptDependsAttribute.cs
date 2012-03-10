using System;
using System.Linq;
using System.Reflection;

namespace SpermGame.Scripts {
    //[Immutable]
    sealed class ScriptDependsAttribute : Attribute {
        private readonly ScriptBase script;
        public ScriptBase Script {
            get { return this.script; }
        }

        public ScriptDependsAttribute(Type type) {
            var members = type.GetMember(
                "Instance",
                MemberTypes.Field | MemberTypes.Property,
                BindingFlags.Static | BindingFlags.Public
            );

            var script = members.Select(GetScript)
                .FirstOrDefault((s) => s != null);
            if (script == null) {
                throw new ArgumentException("Type does not have a script instance", "type");
            }

            this.script = script;
        }

        public ScriptDependsAttribute(ScriptBase script) {
            this.script = script;
        }

        //[Pure]
        private static ScriptBase GetScript(MemberInfo member) {
            var field = member as FieldInfo;
            if (field != null && field.FieldType.IsSubclassOf(typeof(ScriptBase))) {
                return (ScriptBase) field.GetValue(member.DeclaringType);
            }

            var prop = member as PropertyInfo;
            if (prop != null && prop.PropertyType.IsSubclassOf(typeof(ScriptBase))) {
                return (ScriptBase) prop.GetValue(member.DeclaringType, null);
            }

            return null;
        }
    }
}