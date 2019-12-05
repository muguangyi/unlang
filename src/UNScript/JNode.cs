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
    public abstract class JNode : Node
    {
        public override void Init()
        { }

        public override byte[] Export()
        {
            return null;
        }

        public override void Import(byte[] data)
        { }

        public override void Loaded()
        { }

        public override void OnSignal(Spot spot, params object[] args)
        { }

        public virtual void Begin(JInstance instance)
        { }

        protected JInstance Instance
        {
            get
            {
                return this.instance;
            }
        }

        private JInstance instance = null;
    }
}