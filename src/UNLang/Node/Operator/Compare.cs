/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.IO;
using UNode;

namespace UNLang
{
    /// <summary>
    /// Compare module to compare A with B, then trigger different signal with the result.
    /// </summary>
    [NodeInterface("Compare", "UNLang/Operator/")]
    public sealed class Compare : LangNode
    {
        private LangValue v0 = null;
        private LangValue v1 = null;
        private LangSpot s0 = null;
        private LangSpot s1 = null;
        private uint pcount = 0;

        public override void Init()
        {
            this.Type = new LangType();
            this.Type.Dispatcher.AddListener(LangType.CHANGE, OnNotify);

            this.v0 = new LangValue(this.Type);
            this.v1 = new LangValue(this.Type);

            Add(this.s0 = new LangSpot("A", LangType.Category.Object, this, 1, SpotType.In));
            Add(this.s1 = new LangSpot("B", LangType.Category.Object, this, 1, SpotType.In));
            Add(new LangSpot("A == B", LangType.Category.Object, this, 1, SpotType.Out));
            Add(new LangSpot("A != B", LangType.Category.Object, this, 1, SpotType.Out));
            Add(new LangSpot("A > B", LangType.Category.Object, this, 1, SpotType.Out));
            Add(new LangSpot("A < B", LangType.Category.Object, this, 1, SpotType.Out));
            Add(new LangSpot("A >= B", LangType.Category.Object, this, 1, SpotType.Out));
            Add(new LangSpot("A <= B", LangType.Category.Object, this, 1, SpotType.Out));
        }

        public override byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var bytes = this.Type.Export();
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
                    this.Type.Import(reader.ReadBytes(length));
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
                switch (this.Type.Type)
                {
                case LangType.Category.Boolean:
                    DoCompare<bool>(this.v0, this.v1, args);
                    break;
                case LangType.Category.Integer:
                    DoCompare<int>(this.v0, this.v1, args);
                    break;
                case LangType.Category.Float:
                    DoCompare<float>(this.v0, this.v1, args);
                    break;
                case LangType.Category.String:
                    DoCompare<string>(this.v0, this.v1, args);
                    break;
                }
            }
        }

        public LangType Type { get; private set; } = null;

        private void OnNotify(object target, Message message)
        {
            this.s0.OnChangeTypeCategory(this.Type.Type);
            this.s1.OnChangeTypeCategory(this.Type.Type);
        }

        private void DoCompare<T>(LangValue v0, LangValue v1, params object[] args) where T : IComparable
        {
            var p0 = LangValue.Cast<T>(v0.Value);
            var p1 = LangValue.Cast<T>(v1.Value);
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