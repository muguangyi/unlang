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
    [NodeInterface("In", "GameBox.Jigsaw/Module/")]
    [GraphInterface(SpotType.In)]
    public sealed class In : LangNode
    {
        public override void Init()
        {
            this.type = new LangType();
            this.type.Dispatcher.AddListener(LangType.CHANGE, OnNotify);

            Add(this.s = new LangSpot("In", LangType.Category.Any, this, -1, SpotType.In));
            Add(this.ss = new LangSpot("", LangType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            GetAt(1).Signal(args);
        }

        public void OnChange(string spotName)
        {
            this.s.Name = spotName;
        }

        public string SpotName
        {
            get
            {
                return this.s.Name;
            }
        }

        public LangType Type
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

        private LangSpot s = null;
        private LangSpot ss = null;
        private LangType type = null;
    }
}