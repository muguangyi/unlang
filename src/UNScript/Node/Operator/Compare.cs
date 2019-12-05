/*
 * This file is part of the UNScript package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.IO;
using UNode;

namespace UNScript
{
    [NodeInterface("Compare", "GameBox.Jigsaw/Operator/")]
    public sealed class Compare : JNode
    {
        public override void Init()
        {
            this.type = new JType();
            this.type.Dispatcher.AddListener(JType.CHANGE, OnNotify);

            this.v0 = new JValue(this.type);
            this.v1 = new JValue(this.type);

            Add(this.s0 = new JSpot("A", JType.Category.Object, this, 1, SpotType.In));
            Add(this.s1 = new JSpot("B", JType.Category.Object, this, 1, SpotType.In));
            Add(new JSpot("A == B", JType.Category.Object, this, 1, SpotType.Out));
            Add(new JSpot("A != B", JType.Category.Object, this, 1, SpotType.Out));
            Add(new JSpot("A > B", JType.Category.Object, this, 1, SpotType.Out));
            Add(new JSpot("A < B", JType.Category.Object, this, 1, SpotType.Out));
            Add(new JSpot("A >= B", JType.Category.Object, this, 1, SpotType.Out));
            Add(new JSpot("A <= B", JType.Category.Object, this, 1, SpotType.Out));
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
                case JType.Category.Boolean:
                    DoCompare<bool>(this.v0, this.v1, args);
                    break;
                case JType.Category.Integer:
                    DoCompare<int>(this.v0, this.v1, args);
                    break;
                case JType.Category.Float:
                    DoCompare<float>(this.v0, this.v1, args);
                    break;
                case JType.Category.String:
                    DoCompare<string>(this.v0, this.v1, args);
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
        }

        private JType type = null;
        private JValue v0 = null;
        private JValue v1 = null;
        private JSpot s0 = null;
        private JSpot s1 = null;
        private uint pcount = 0;

        private void DoCompare<T>(JValue v0, JValue v1, params object[] args) where T : IComparable
        {
            var p0 = JValue.Cast<T>(v0.Value);
            var p1 = JValue.Cast<T>(v1.Value);
            var result = p0.CompareTo(p1);

            if (0 == result)
            {
                GetAt(2).Signal(args);
            }
            else
            {
                GetAt(3).Signal(args);
            }

            if (result > 0)
            {
                GetAt(4).Signal(args);
            }
            else if (result < 0)
            {
                GetAt(5).Signal(args);
            }

            if (result >= 0)
            {
                GetAt(6).Signal(args);
            }
            if (result <= 0)
            {
                GetAt(7).Signal(args);
            }
        }
    }
}