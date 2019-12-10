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
using UNode;
using UNode.Editor;

namespace UNLang.Editor
{
    /// <summary>
    /// UNLang editor.
    /// </summary>
    public class UNLangEditor : NodeEditor<LangNode>
    {
        private INodeRunnerDebugger debugger = null;
        private Vector2 debugScrollPos = new Vector2();

        [MenuItem("UNLang/IDE...")]
        public static void Open()
        {
            GetWindow<UNLangEditor>("UNLang");
        }

        public UNLangEditor()
        {
            this.OnLoadCompleted += OnHandleLoadCompleted;
        }

        public void OnDestroy()
        {
            this.OnLoadCompleted -= OnHandleLoadCompleted;
        }

        public override void OnGUI()
        {
            DrawToolbar();

            using (new EditorGUILayout.HorizontalScope())
            {
                if (null != this.debugger)
                {
                    this.debugScrollPos = EditorGUILayout.BeginScrollView(this.debugScrollPos, GUILayout.Width(200));
                    {
                        using (new EditorGUILayout.VerticalScope())
                        {
                            var current = this.debugger.Runners;
                            while (current.MoveNext())
                            {
                                var runner = current.Current;
                                if (GUILayout.Button("" + runner.GetHashCode()))
                                {
                                    this.CurrentRunner = runner;
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }

                base.OnGUI();
            }
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button(null != this.debugger ? "Stop" : "Debug", EditorStyles.toolbarPopup, GUILayout.Width(150f)))
                {
                    if (null != this.debugger)
                    {
                        this.debugger = null;
                        this.CurrentRunner = null;
                        NodeVM.StopDebugger();
                    }
                    else
                    {
                        this.debugger = NodeVM.StartDebugger(this.FilePath);
                    }
                }
                GUILayout.Label(this.FilePath.Replace(Application.dataPath, ""));
            }
        }

        private void OnHandleLoadCompleted(string filePath)
        {
            if (null != this.debugger)
            {
                this.debugger = null;
                NodeVM.StopDebugger();
            }
        }
    }
}

