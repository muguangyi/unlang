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
using UnityEngine;
using UNode;

namespace UNLang
{
    [NodeInterface("Cast", "UNLang/Value/")]
    public sealed class Cast : LangNode
    {
        public override void Init()
        {
            this.tf = new LangType();
            this.tt = new LangType();

            Add(new LangSpot("From", LangType.Category.Object, this, 1, SpotType.In));
            Add(new LangSpot("To", LangType.Category.Object, this, 1, SpotType.Out));
        }

        public override byte[] Export()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    var fbytes = this.tf.Export();
                    writer.Write(fbytes.Length);
                    writer.Write(fbytes);
                    var tbytes = this.tt.Export();
                    writer.Write(tbytes.Length);
                    writer.Write(tbytes);
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
                    this.tf.Import(reader.ReadBytes(reader.ReadInt32()));
                    this.tf.Import(reader.ReadBytes(reader.ReadInt32()));
                }
            }
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            if (SpotType.In == spot.Type)
            {
                object result = null;
                try
                {
                    switch (this.tt.Type)
                    {
                    case LangType.Category.Boolean:
                        {
                            result = Convert.ChangeType(args[1], typeof(bool));
                        }
                        break;
                    case LangType.Category.Integer:
                        {
                            result = Convert.ChangeType(args[1], typeof(int));
                        }
                        break;
                    case LangType.Category.Float:
                        {
                            result = Convert.ChangeType(args[1], typeof(float));
                        }
                        break;
                    case LangType.Category.String:
                        {
                            result = Convert.ChangeType(args[1], typeof(string));
                        }
                        break;
                    case LangType.Category.Vector2:
                        {
                            result = Convert.ChangeType(args[1], typeof(Vector2));
                        }
                        break;
                    case LangType.Category.Vector3:
                        {
                            result = Convert.ChangeType(args[1], typeof(Vector3));
                        }
                        break;
                    case LangType.Category.Vector4:
                        {
                            result = Convert.ChangeType(args[1], typeof(Vector4));
                        }
                        break;
                    }
                }
                finally
                {
                    GetAt(2).Signal(args[0], result);
                }
            }
        }

        public LangType From
        {
            get
            {
                return this.tf;
            }
        }

        public LangType To
        {
            get
            {
                return this.tt;
            }
        }

        private LangType tf = null;
        private LangType tt = null;
    }
}