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
    [NodeInterface("DeltaTime", "GameBox.Jigsaw/UnityEngine/")]
    public sealed class DeltaTime : JNode
    {
        public override void Init()
        {
            Add(new JSpot("", JType.Category.Object, this, -1, SpotType.In));
            Add(new JSpot("", JType.Category.Float, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            GetAt(1).Signal(args[0], Time.deltaTime);
        }
    }
}