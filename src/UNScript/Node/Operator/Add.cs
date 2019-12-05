/*
 * This file is part of the UNScript package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System.IO;
using UNode;

namespace UNScript
{
    [NodeInterface("+", "GameBox.Jigsaw/Operator/")]
    public sealed class Add : JNode
    {
        public override void Init()
        {
            this.type = new JType();
            this.type.Dispatcher.AddListener(JType.CHANGE, OnNotify);

            this.v0 = new JValue(this.type);
            this.v1 = new JValue(this.type);

            Add(this.s0 = new JSpot("A", JType.Category.Object, this, 1, SpotType.In));
            Add(this.s1 = new JSpot("B", JType.Category.Object, this, 1, SpotType.In));
            Add(this.sr = new JSpot("A + B", JType.Category.Object, this, 1, SpotType.Out));
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
                case JType.Category.Integer:
                    {
                        var r = JValue.Cast<int>(this.v0.Value) + JValue.Cast<int>(this.v1.Value);
                        GetAt(2).Signal(args[0], JValue.Cast<int>(r));
                    }
                    break;
                case JType.Category.Float:
                    {
                        var r = JValue.Cast<float>(this.v0.Value) + JValue.Cast<float>(this.v1.Value);
                        GetAt(2).Signal(args[0], JValue.Cast<float>(r));
                    }
                    break;
                case JType.Category.String:
                    {
                        var r = JValue.Cast<string>(this.v0.Value) + JValue.Cast<string>(this.v1.Value);
                        GetAt(2).Signal(args[0], r);
                    }
                    break;
                }
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
            this.s0.OnChangeTypeCategory(this.type.Type);
            this.s1.OnChangeTypeCategory(this.type.Type);
            this.sr.OnChangeTypeCategory(this.type.Type);
        }

        private JType type = null;
        private JValue v0 = null;
        private JValue v1 = null;
        private JSpot s0 = null;
        private JSpot s1 = null;
        private JSpot sr = null;
        private uint pcount = 0;
    }
}