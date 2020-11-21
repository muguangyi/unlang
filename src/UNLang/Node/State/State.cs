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
    /// <summary>
    /// State module to define the base state machine.
    /// </summary>
    [NodeInterface("State", "UNLang/State/")]
    public class State : LangNode
    {
        private LangInstance subinstance = null;
        private bool validity = true;

        public override void Init()
        {
            this.subinstance = new LangInstance();
            this.StateVars = new LangVars();

            Add(new LangSpot("Begin", LangType.Category.Any, this, -1, SpotType.In));

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
        }

        protected override void OnBegin()
        {
            this.Instance.ChangeState(this);
        }

        public void Enter()
        {
            this.validity = true;
            this.subinstance.Set(this.Instance);
            GetAt(1).Signal(this.Instance);
            GetAt(4).Signal(this.subinstance);
            OnEnter();
        }

        public void Execute()
        {
            if (this.validity) { this.subinstance.Update(); }
            if (this.validity) { GetAt(2).Signal(this.Instance); }
            if (this.validity) { OnExecute(); }
        }

        public void Exit()
        {
            GetAt(3).Signal(this.Instance);
            OnExit();
            this.subinstance.Reset();
            this.StateVars.Reset();
            this.validity = false;
        }

        public void DrawGizmos()
        {
            if (this.validity) { this.subinstance.OnDrawGizmos(); }
            if (this.validity) { OnGizmos(); }
        }

        internal void Notify<T>(T m, params object[] args)
        {
            if (this.validity) { this.subinstance.Notify(m, args); }
            if (this.validity) { OnNotify(m, args); }
        }

        internal LangVars StateVars { get; private set; } = null;

        protected virtual void OnEnter()
        { }

        protected virtual void OnExecute()
        { }

        protected virtual void OnExit()
        { }

        protected virtual void OnNotify<T>(T m, params object[] args)
        { }

        protected virtual void OnGizmos()
        { }
    }
}