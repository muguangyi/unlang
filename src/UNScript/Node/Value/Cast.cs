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
using UnityEngine;
using UNode;

namespace UNScript
{
    [NodeInterface("Cast", "GameBox.Jigsaw/Value/")]
    public sealed class Cast : JNode
    {
        public override void Init()
        {
            this.tf = new JType();
            this.tt = new JType();

            Add(new JSpot("From", JType.Category.Object, this, 1, SpotType.In));
            Add(new JSpot("To", JType.Category.Object, this, 1, SpotType.Out));
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
                    case JType.Category.Boolean:
                        {
                            result = Convert.ChangeType(args[1], typeof(bool));
                        }
                        break;
                    case JType.Category.Integer:
                        {
                            result = Convert.ChangeType(args[1], typeof(int));
                        }
                        break;
                    case JType.Category.Float:
                        {
                            result = Convert.ChangeType(args[1], typeof(float));
                        }
                        break;
                    case JType.Category.String:
                        {
                            result = Convert.ChangeType(args[1], typeof(string));
                        }
                        break;
                    case JType.Category.Vector2:
                        {
                            result = Convert.ChangeType(args[1], typeof(Vector2));
                        }
                        break;
                    case JType.Category.Vector3:
                        {
                            result = Convert.ChangeType(args[1], typeof(Vector3));
                        }
                        break;
                    case JType.Category.Vector4:
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

        public JType From
        {
            get
            {
                return this.tf;
            }
        }

        public JType To
        {
            get
            {
                return this.tt;
            }
        }

        private JType tf = null;
        private JType tt = null;
    }
}