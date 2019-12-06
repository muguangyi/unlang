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
    [NodeInterface("GetVar", "UNLang/Value/")]
    public sealed class GetVar : LangNode
    {
        public override void Init()
        {
            this.type = new LangType();
            this.type.Dispatcher.AddListener(LangType.CHANGE, OnNotify);

            Add(new LangSpot("", LangType.Category.Any, this, 1, SpotType.In));
            Add(this.spot = new LangSpot("Value", LangType.Category.Object, this, 1, SpotType.Out));
        }

        public override byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)this.scope);
                    writer.Write(this.variable);
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
                    this.scope = (LangVars.Scope)reader.ReadByte();
                    this.variable = reader.ReadString();
                    var length = reader.ReadInt32();
                    this.type.Import(reader.ReadBytes(length));
                }
            }
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            var instance = args[0] as LangInstance;
            LangVars vars = instance.GetVars(this.scope);
            var value = vars.Get(this.Variable);
            GetAt(1).Signal(instance, value);
        }

        public LangVars.Scope Scope
        {
            get
            {
                return this.scope;
            }
            set
            {
                this.scope = value;
            }
        }

        public LangType Type
        {
            get
            {
                return this.type;
            }
        }

        public string Variable
        {
            get
            {
                return this.variable;
            }
            set
            {
                this.variable = value;
            }
        }

        private void OnNotify(object target, Message message)
        {
            this.spot.OnChangeTypeCategory(this.type.Type);
        }

        private LangVars.Scope scope = LangVars.Scope.State;
        private LangType type = null;
        private string variable = null;
        private LangSpot spot = null;
    }
}