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
    [CustomEditor(typeof(In))]
    public class InInspector : NodeInspector
    {
        public override void OnInspectorGUI()
        {
            var op = this.target as In;
            var spotName = EditorGUILayout.TextField("Spot:", op.SpotName);
            if (spotName != op.SpotName)
            {
                op.OnChange(spotName);
            }
            LangTypeInspector.OnGUI(op.Type);

            base.OnInspectorGUI();
        }
    }
}