/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UNode;

namespace UNLang
{
    /// <summary>
    /// LangInstance maintains script runtime.
    /// </summary>
    public sealed class LangInstance : IDisposable
    {
        public LangInstance()
        {
            this.instanceVars = new LangVars();
            this.cache = new Dictionary<int, LangNode>();
        }

        public LangInstance(object owner, LangVars ownerVars)
        {
            this.owner = owner;
            this.ownerVars = ownerVars;
            this.instanceVars = new LangVars();
            this.cache = new Dictionary<int, LangNode>();
        }

        public void Dispose()
        {
            Reset();
            this.instanceVars = null;
            this.cache = null;
        }

        public void Set(LangInstance instance)
        {
            this.owner = instance.owner;
            this.ownerVars = instance.ownerVars;
        }

        public void Reset()
        {
            ChangeState(null);
            Clear();
            this.script = null;
            this.runner = null;
            this.owner = null;
            this.ownerVars = null;
            this.lockState = null;
        }

        public void Load(string script)
        {
            Clear();

            this.script = script;
            this.runner = NodeVM.Pick(script);
        }

        public void LoadAsync(string script, Action callback = null)
        {
            Clear();

            this.script = script;
            this.runner = NodeVM.Pick(script);
            if (null != callback)
            {
                callback();
            }
        }

        public bool Run<T>(NodeRunner runner = null) where T : LangNode
        {
            if (null != runner) { this.runner = runner; Clear(); }
            var node = FindNode<T>();
            if (null != node) { node.Begin(this); }

            return (null != node);
        }

        public void Update()
        {
            if (null != this.state)
            {
                this.state.Execute(this);
            }
        }

        public void Notify<T>(T m, params object[] args)
        {
            if (null != this.state)
            {
                this.state.Notify(this, m, args);
            }
        }

        public void OnDrawGizmos()
        {
            if (null != this.state)
            {
                this.state.DrawGizmos(this);
            }
        }

        public LangVars GetVars(LangVars.Scope scope)
        {
            switch (scope)
            {
            case LangVars.Scope.Global:
                return LangVars.GlobalVars;
            case LangVars.Scope.Owner:
                return this.ownerVars;
            case LangVars.Scope.Instance:
                return this.instanceVars;
            case LangVars.Scope.State:
                return this.state.StateVars;
            }

            return null;
        }

        public void LockState(Action procedure)
        {
            this.lockState = this.state;
            {
                procedure();
            }
            this.lockState = null;
        }

        public bool IsLockedState()
        {
            return (this.lockState == this.state);
        }

        public bool IsState<T>() where T : State
        {
            return (null != this.state && this.state is T);
        }

        public object Owner
        {
            get
            {
                return this.owner;
            }
        }

        internal void ChangeState(State state)
        {
            if (null != this.state)
            {
                this.state.Exit(this);
            }

            this.state = state;

            if (null != this.state)
            {
                this.state.Enter(this);
            }
        }

        private LangNode FindNode<T>() where T : LangNode
        {
            var key = typeof(T).GetHashCode();
            LangNode node = null;
            if (!this.cache.TryGetValue(key, out node))
            {
                node = this.runner.Nodes.First(n => n is T) as LangNode;
                if (null != node)
                {
                    this.cache.Add(key, node);
                }
            }

            return node;
        }

        private void Clear()
        {
            this.cache.Clear();

            if (null != this.script && null != this.runner)
            {
                NodeVM.Drop(this.script, this.runner);
            }
        }

        private string script = null;
        private NodeRunner runner = null;
        private object owner = null;
        private LangVars ownerVars = null;
        private LangVars instanceVars = null;
        private State state = null;
        private State lockState = null;
        private Dictionary<int, LangNode> cache = null;
    }
}