/*
 * This file is part of the UNScript package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UNode;

namespace UNScript
{
    [NodeInterface("State", "GameBox.Jigsaw/State/")]
    public class State : JNode
    {
        public override void Init()
        {
            this.subinstance = new JInstance();
            this.stateVars = new JVars();

            Add(new JSpot("Begin", JType.Category.Any, this, -1, SpotType.In));
            Add(new JSpot("Abort", JType.Category.Any, this, -1, SpotType.In));

            Add(new JSpot("OnEnter", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("OnUpdate", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("OnExit", JType.Category.Any, this, 1, SpotType.Out));

            Add(new JSpot("Substate", JType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            var instance = args[0] as JInstance;
            if ("Begin" == spot.Name)
            {
                Begin(instance);
            }
            else if ("Abort" == spot.Name)
            {
                Abort(instance);
            }
        }

        public override void Begin(JInstance instance)
        {
            instance.ChangeState(this);
        }

        public void Abort(JInstance instance)
        {
            instance.ChangeState(null);
        }

        public void Enter(JInstance instance)
        {
            this.validity = true;
            this.subinstance.Set(instance);
            GetAt(2).Signal(instance);
            GetAt(5).Signal(this.subinstance);
            OnEnter(instance);
        }

        public void Execute(JInstance instance)
        {
            if (this.validity) { this.subinstance.Update(); }
            if (this.validity) { GetAt(3).Signal(instance); }
            if (this.validity) { OnExecute(instance); }
        }

        public void Exit(JInstance instance)
        {
            GetAt(4).Signal(instance);
            OnExit(instance);
            this.subinstance.Reset();
            this.stateVars.Reset();
            this.validity = false;
        }

        public void DrawGizmos(JInstance instance)
        {
            if (this.validity) { this.subinstance.OnDrawGizmos(); }
            if (this.validity) { OnGizmos(instance); }
        }

        internal void Notify<T>(JInstance instance, T m, params object[] args)
        {
            if (this.validity) { this.subinstance.Notify(m, args); }
            if (this.validity) { OnNotify<T>(instance, m, args); }
        }

        internal JVars StateVars
        {
            get
            {
                return this.stateVars;
            }
        }

        protected virtual void OnEnter(JInstance instance)
        { }

        protected virtual void OnExecute(JInstance instance)
        { }

        protected virtual void OnExit(JInstance instance)
        { }

        protected virtual void OnNotify<T>(JInstance instance, T m, params object[] args)
        { }

        protected virtual void OnGizmos(JInstance instance)
        { }

        private JInstance subinstance = null;
        private JVars stateVars = null;
        private bool validity = true;
    }
}