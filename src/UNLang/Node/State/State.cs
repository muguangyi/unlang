/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UNode;

namespace UNLang
{
    [NodeInterface("State", "GameBox.Jigsaw/State/")]
    public class State : LangNode
    {
        public override void Init()
        {
            this.subinstance = new LangInstance();
            this.stateVars = new LangVars();

            Add(new LangSpot("Begin", LangType.Category.Any, this, -1, SpotType.In));
            Add(new LangSpot("Abort", LangType.Category.Any, this, -1, SpotType.In));

            Add(new LangSpot("OnEnter", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("OnUpdate", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("OnExit", LangType.Category.Any, this, 1, SpotType.Out));

            Add(new LangSpot("Substate", LangType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            var instance = args[0] as LangInstance;
            if ("Begin" == spot.Name)
            {
                Begin(instance);
            }
            else if ("Abort" == spot.Name)
            {
                Abort(instance);
            }
        }

        public override void Begin(LangInstance instance)
        {
            instance.ChangeState(this);
        }

        public void Abort(LangInstance instance)
        {
            instance.ChangeState(null);
        }

        public void Enter(LangInstance instance)
        {
            this.validity = true;
            this.subinstance.Set(instance);
            GetAt(2).Signal(instance);
            GetAt(5).Signal(this.subinstance);
            OnEnter(instance);
        }

        public void Execute(LangInstance instance)
        {
            if (this.validity) { this.subinstance.Update(); }
            if (this.validity) { GetAt(3).Signal(instance); }
            if (this.validity) { OnExecute(instance); }
        }

        public void Exit(LangInstance instance)
        {
            GetAt(4).Signal(instance);
            OnExit(instance);
            this.subinstance.Reset();
            this.stateVars.Reset();
            this.validity = false;
        }

        public void DrawGizmos(LangInstance instance)
        {
            if (this.validity) { this.subinstance.OnDrawGizmos(); }
            if (this.validity) { OnGizmos(instance); }
        }

        internal void Notify<T>(LangInstance instance, T m, params object[] args)
        {
            if (this.validity) { this.subinstance.Notify(m, args); }
            if (this.validity) { OnNotify<T>(instance, m, args); }
        }

        internal LangVars StateVars
        {
            get
            {
                return this.stateVars;
            }
        }

        protected virtual void OnEnter(LangInstance instance)
        { }

        protected virtual void OnExecute(LangInstance instance)
        { }

        protected virtual void OnExit(LangInstance instance)
        { }

        protected virtual void OnNotify<T>(LangInstance instance, T m, params object[] args)
        { }

        protected virtual void OnGizmos(LangInstance instance)
        { }

        private LangInstance subinstance = null;
        private LangVars stateVars = null;
        private bool validity = true;
    }
}