/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System.IO;
using UNode;

namespace UNLang
{
    [NodeInterface("+", "GameBox.Jigsaw/Operator/")]
    public sealed class Add : LangNode
    {
        public override void Init()
        {
            this.type = new LangType();
            this.type.Dispatcher.AddListener(LangType.CHANGE, OnNotify);

            this.v0 = new LangValue(this.type);
            this.v1 = new LangValue(this.type);

            Add(this.s0 = new LangSpot("A", LangType.Category.Object, this, 1, SpotType.In));
            Add(this.s1 = new LangSpot("B", LangType.Category.Object, this, 1, SpotType.In));
            Add(this.sr = new LangSpot("A + B", LangType.Category.Object, this, 1, SpotType.Out));
        }

        public override byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var bytes = this.type.Export();
                    writer.Write(bytes.Length);
                    writer.Write(bytes);
                }

                return stream.ToArray();
            }
        }

        public override void Import(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var length = reader.ReadInt32();
                    this.type.Import(reader.ReadBytes(length));
                }
            }
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            ++this.pcount;
            if (spot.Name == "A")
            {
                this.v0.Value = args[1];
                if (1 == this.pcount)
                {
                    GetAt(1).Signal(args[0]);
                }
            }
            else if (spot.Name == "B")
            {
                this.v1.Value = args[1];
                if (1 == this.pcount)
                {
                    GetAt(0).Signal(args[0]);
                }
            }

            if (2 == this.pcount)
            {
                this.pcount = 0;
                switch (this.type.Type)
                {
                case LangType.Category.Integer:
                    {
                        var r = LangValue.Cast<int>(this.v0.Value) + LangValue.Cast<int>(this.v1.Value);
                        GetAt(2).Signal(args[0], LangValue.Cast<int>(r));
                    }
                    break;
                case LangType.Category.Float:
                    {
                        var r = LangValue.Cast<float>(this.v0.Value) + LangValue.Cast<float>(this.v1.Value);
                        GetAt(2).Signal(args[0], LangValue.Cast<float>(r));
                    }
                    break;
                case LangType.Category.String:
                    {
                        var r = LangValue.Cast<string>(this.v0.Value) + LangValue.Cast<string>(this.v1.Value);
                        GetAt(2).Signal(args[0], r);
                    }
                    break;
                }
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
            this.s0.OnChangeTypeCategory(this.type.Type);
            this.s1.OnChangeTypeCategory(this.type.Type);
            this.sr.OnChangeTypeCategory(this.type.Type);
        }

        private LangType type = null;
        private LangValue v0 = null;
        private LangValue v1 = null;
        private LangSpot s0 = null;
        private LangSpot s1 = null;
        private LangSpot sr = null;
        private uint pcount = 0;
    }
}