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

namespace UNLang
{
    public sealed class LangValue : UnityEngine.Object, IDisposable
    {
        public LangValue(LangType type)
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
                    case LangType.Category.Boolean:
                        writer.Write((bool)this.value);
                        break;
                    case LangType.Category.Integer:
                        writer.Write((int)this.value);
                        break;
                    case LangType.Category.Float:
                        writer.Write((float)this.value);
                        break;
                    case LangType.Category.String:
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
                    case LangType.Category.Boolean:
                        this.value = reader.ReadBoolean();
                        break;
                    case LangType.Category.Integer:
                        this.value = reader.ReadInt32();
                        break;
                    case LangType.Category.Float:
                        this.value = reader.ReadSingle();
                        break;
                    case LangType.Category.String:
                        this.value = reader.ReadString();
                        break;
                    default:
                        break;
                    }
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

        private LangType type = null;
        private object value = null;

        public static T Cast<T>(object value)
        {
            return (null != value ? (T)value : default(T));
        }
    }
}