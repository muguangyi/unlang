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
    /// <summary>
    /// UNLang value.
    /// </summary>
    public sealed class LangValue : UnityEngine.Object, IDisposable
    {
        public LangValue(LangType type)
        {
            this.Type = type;
        }

        public void Dispose()
        {
            this.Type.Dispose();
            this.Type = null;
            this.Value = null;
        }

        public byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var bytes = this.Type.Export();
                    writer.Write(bytes.Length);
                    writer.Write(bytes);
                    switch (this.Type.Type)
                    {
                    case LangType.Category.Boolean:
                        writer.Write((bool)this.Value);
                        break;
                    case LangType.Category.Integer:
                        writer.Write((int)this.Value);
                        break;
                    case LangType.Category.Float:
                        writer.Write((float)this.Value);
                        break;
                    case LangType.Category.String:
                        writer.Write((string)this.Value);
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
                    this.Type.Import(bytes);
                    switch (this.Type.Type)
                    {
                    case LangType.Category.Boolean:
                        this.Value = reader.ReadBoolean();
                        break;
                    case LangType.Category.Integer:
                        this.Value = reader.ReadInt32();
                        break;
                    case LangType.Category.Float:
                        this.Value = reader.ReadSingle();
                        break;
                    case LangType.Category.String:
                        this.Value = reader.ReadString();
                        break;
                    default:
                        break;
                    }
                }
            }
        }

        public LangType Type { get; private set; } = null;

        public object Value { get; set; } = null;

        public static T Cast<T>(object value)
        {
            return (null != value ? (T)value : default(T));
        }
    }
}