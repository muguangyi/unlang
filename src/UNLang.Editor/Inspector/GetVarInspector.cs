/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UnityEditor;
using UNode.Editor;

namespace UNLang.Editor
{
    [CustomEditor(typeof(GetVar))]
    public class GetVarInspector : NodeInspector
    {
        public override void OnInspectorGUI()
        {
            var var = this.target as GetVar;
            var.Scope = (LangVars.Scope)EditorGUILayout.EnumPopup(var.Scope);
            LangTypeInspector.OnGUI(var.Type);
            var.Variable = EditorGUILayout.TextField("Variable:", var.Variable);

            base.OnInspectorGUI();
        }
    }
}