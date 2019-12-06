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
    /// <summary>
    /// SetVar module to define a named variable.
    /// </summary>
    [NodeInterface("SetVar", "UNLang/Value/")]
    public sealed class SetVar : LangNode
    {
        private LangSpot v = null;
        private LangSpot vv = null;

        public override void Init()
        {
            this.Type = new LangType();
            this.Type.Dispatcher.AddListener(LangType.CHANGE, OnNotify);

            Add(this.v = new LangSpot("Value", LangType.Category.Object, this, 1, SpotType.In));
            Add(this.vv = new LangSpot("", LangType.Category.Object, this, 1, SpotType.Out));
        }

        public override byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)this.Scope);
                    writer.Write(this.Variable);
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
                    this.Scope = (LangVars.Scope)reader.ReadByte();
                    this.Variable = reader.ReadString();
                    var length = reader.ReadInt32();
                    this.Type.Import(reader.ReadBytes(length));
                }
            }
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            var instance = args[0] as LangInstance;
            LangVars vars = instance.GetVars(this.Scope);
            vars.Set(this.Variable, args[1]);
            GetAt(1).Signal(instance, args[1]);
        }

        public LangVars.Scope Scope { get; set; } = LangVars.Scope.State;

        public LangType Type { get; private set; } = null;

        public string Variable { get; set; } = null;

        private void OnNotify(object target, Message message)
        {
            this.v.OnChangeTypeCategory(this.Type.Type);
            this.vv.OnChangeTypeCategory(this.Type.Type);
        }
    }
}