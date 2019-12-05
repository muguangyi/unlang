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
    [NodeInterface("Out", "GameBox.Jigsaw/Module/")]
    [GraphInterface(SpotType.Out)]
    public sealed class Out : JNode
    {
        public override void Init()
        {
            this.type = new JType();
            this.type.Dispatcher.AddListener(JType.CHANGE, OnNotify);

            Add(this.s = new JSpot("", JType.Category.Any, this, -1, SpotType.In));
            Add(this.ss = new JSpot("Out", JType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            this.ss.Signal(args);
        }

        public void OnChange(string spotName)
        {
            this.ss.Name = spotName;
        }

        public string SpotName
        {
            get
            {
                return this.ss.Name;
            }
        }

        public JType Type
        {
            get
            {
                return this.type;
            }
        }

        private void OnNotify(object target, Message message)
        {
            this.s.OnChangeTypeCategory(this.type.Type);
            this.ss.OnChangeTypeCategory(this.type.Type);
        }

        private JSpot s = null;
        private JSpot ss = null;
        private JType type = null;
    }
}