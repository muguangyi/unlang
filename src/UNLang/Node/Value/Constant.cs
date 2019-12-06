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
    [NodeInterface("Constant", "UNLang/Value/")]
    public sealed class Constant : LangNode
    {
        public override void Init()
        {
            this.value = new LangValue(new LangType());
            this.value.Type.Dispatcher.AddListener(LangType.CHANGE, OnNotify);

            Add(new LangSpot("", LangType.Category.Any, this, -1, SpotType.In));
            Add(this.spot = new LangSpot("Value", LangType.Category.Object, this, 1, SpotType.Out));
        }

        public override byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var bytes = this.value.Export();
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
                    this.value.Import(reader.ReadBytes(length));
                }
            }
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            this.spot.Signal(args[0], this.value.Value);
        }

        public LangValue Value
        {
            get
            {
                return this.value;
            }
        }

        private void OnNotify(object target, Message message)
        {
            this.spot.OnChangeTypeCategory(this.value.Type.Type);
        }

        private LangValue value = null;
        private LangSpot spot = null;
    }
}