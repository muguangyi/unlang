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
    public class LangType : UnityEngine.Object, IDisposable
    {
        public enum Category
        {
            Any,
            Object,
            Boolean,
            Integer,
            Float,
            String,
            Vector2,
            Vector3,
            Vector4,
        }

        public const string CHANGE = "type.change";

        public LangType()
        {
            this.dispatcher = new Dispatcher(this);
        }

        public void Dispose()
        {
            this.dispatcher.Dispose();
            this.dispatcher = null;
        }

        public byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)this.type);
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
                    OnChange((Category)reader.ReadByte());
                }
            }
        }

        public void OnChange(Category type)
        {
            this.type = type;
            this.dispatcher.Notify(new Message(CHANGE));
        }

        public Category Type
        {
            get
            {
                return this.type;
            }
        }

        public Dispatcher Dispatcher
        {
            get
            {
                return this.dispatcher;
            }
        }

        private Category type = Category.Any;
        private Dispatcher dispatcher = null;
    }
}