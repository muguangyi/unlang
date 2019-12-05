/*
 * This file is part of the UNScript package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UnityEngine;
using UNode;

namespace UNScript
{
    [NodeInterface("Console", "GameBox.Jigsaw/Debug/")]
    public sealed class Console : JNode
    {
        public override void Init()
        {
            Add(new JSpot("", JType.Category.Object, this, -1, SpotType.In));
            Add(new JSpot("", JType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            if (SpotType.In == spot.Type)
            {
                Debug.Log(args[1]);
                GetAt(1).Signal(args);
            }
        }
    }
}