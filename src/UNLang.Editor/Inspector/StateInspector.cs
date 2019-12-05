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
    [CustomEditor(typeof(State))]
    public class StateInspector : NodeInspector
    {
        public override void OnInspectorGUI()
        {
            var state = this.target as State;
            state.Tag = EditorGUILayout.TextField("Name:", state.Tag);

            base.OnInspectorGUI();
        }
    }
}