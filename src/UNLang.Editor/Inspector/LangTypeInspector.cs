/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UnityEditor;

namespace UNLang.Editor
{
    public static class LangTypeInspector
    {
        public static void OnGUI(LangType type)
        {
            var t = (LangType.Category)EditorGUILayout.EnumPopup(type.Type);
            if (t != type.Type)
            {
                type.OnChange(t);
            }
        }
    }
}