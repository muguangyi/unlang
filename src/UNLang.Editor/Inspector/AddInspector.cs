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
    [CustomEditor(typeof(Add))]
    public class AddInspector : NodeInspector
    {
        public override void OnInspectorGUI()
        {
            var op = this.target as Add;
            LangTypeInspector.OnGUI(op.Type);

            base.OnInspectorGUI();
        }
    }
}