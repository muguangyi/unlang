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
    [NodeInterface("GetVar", "GameBox.Jigsaw/Value/")]
    public sealed class GetVar : JNode
    {
        public override void Init()
        {
            this.type = new JType();
            this.type.Dispatcher.AddListener(JType.CHANGE, OnNotify);

            Add(new JSpot("", JType.Category.Any, this, 1, SpotType.In));
            Add(this.spot = new JSpot("Value", JType.Category.Object, this, 1, SpotType.Out));
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
                    this.scope = (JVars.Scope)reader.ReadByte();
                    this.variable = reader.ReadString();
                    var length = reader.ReadInt32();
                    this.type.Import(reader.ReadBytes(length));
                }
            }
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            var instance = args[0] as JInstance;
            JVars vars = instance.GetVars(this.scope);
            var value = vars.Get(this.Variable);
            GetAt(1).Signal(instance, value);
        }

        public JVars.Scope Scope
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

        public JType Type
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

        private JVars.Scope scope = JVars.Scope.State;
        private JType type = null;
        private string variable = null;
        private JSpot spot = null;
    }
}