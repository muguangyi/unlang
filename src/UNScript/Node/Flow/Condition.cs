/*
 * This file is part of the UNScript package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UNode;

namespace UNScript
{
    [NodeInterface("Condition", "GameBox.Jigsaw/Flow/")]
    public sealed class Condition : JNode
    {
        public override void Init()
        {
            Add(new JSpot("", JType.Category.Boolean, this, 1, SpotType.In));
            Add(new JSpot("True", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("False", JType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            if (SpotType.In == spot.Type)
            {
                var result = (bool)args[1];
                if (result)
                {
                    GetAt(1).Signal(args);
                }
                else
                {
                    GetAt(2).Signal(args);
                }
            }
        }
    }
}