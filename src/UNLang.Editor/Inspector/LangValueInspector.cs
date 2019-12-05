/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UnityEditor;
using UnityEngine;

namespace UNLang.Editor
{
    public static class LangValueInspector
    {
        public static void OnGUI(LangValue value)
        {
            LangTypeInspector.OnGUI(value.Type);
            switch (value.Type.Type)
            {
            case LangType.Category.Boolean:
                value.Value = EditorGUILayout.Toggle("Value", SafeGetValue<bool>(value.Value));
                break;
            case LangType.Category.Integer:
                value.Value = EditorGUILayout.IntField("Value", SafeGetValue<int>(value.Value));
                break;
            case LangType.Category.Float:
                value.Value = EditorGUILayout.FloatField("Value", SafeGetValue<float>(value.Value));
                break;
            case LangType.Category.String:
                value.Value = EditorGUILayout.TextField("Value", SafeGetValue<string>(value.Value));
                break;
            case LangType.Category.Vector2:
                value.Value = EditorGUILayout.Vector2Field("Value", SafeGetValue<Vector2>(value.Value));
                break;
            case LangType.Category.Vector3:
                value.Value = EditorGUILayout.Vector3Field("Value", SafeGetValue<Vector3>(value.Value));
                break;
            case LangType.Category.Vector4:
                value.Value = EditorGUILayout.Vector4Field("Value", SafeGetValue<Vector4>(value.Value));
                break;
            }
        }

        private static T SafeGetValue<T>(object data)
        {
            try
            {
                return (null != data ? (T)data : default(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}