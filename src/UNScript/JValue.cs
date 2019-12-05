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

namespace UNScript
{
    public sealed class JValue : UnityEngine.Object, IDisposable
    {
        public JValue(JType type)
        {
            this.type = type;
        }

        public void Dispose()
        {
            this.type.Dispose();
            this.type = null;
            this.value = null;
        }

        public byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var bytes = this.type.Export();
                    writer.Write(bytes.Length);
                    writer.Write(bytes);
                    switch (this.type.Type)
                    {
                    case JType.Category.Boolean:
                        writer.Write((bool)this.value);
                        break;
                    case JType.Category.Integer:
                        writer.Write((int)this.value);
                        break;
                    case JType.Category.Float:
                        writer.Write((float)this.value);
                        break;
                    case JType.Category.String:
                        writer.Write((string)this.value);
                        break;
                    default:
                        break;
                    }
                }

                return stream.ToArray();
            }
        }

        public void Import(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var length = reader.ReadInt32();
                    var bytes = reader.ReadBytes(length);
                    this.type.Import(bytes);
                    switch (this.type.Type)
                    {
                    case JType.Category.Boolean:
                        this.value = reader.ReadBoolean();
                        break;
                    case JType.Category.Integer:
                        this.value = reader.ReadInt32();
                        break;
                    case JType.Category.Float:
                        this.value = reader.ReadSingle();
                        break;
                    case JType.Category.String:
                        this.value = reader.ReadString();
                        break;
                    default:
                        break;
                    }
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

        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        private JType type = null;
        private object value = null;

        public static T Cast<T>(object value)
        {
            return (null != value ? (T)value : default(T));
        }
    }
}