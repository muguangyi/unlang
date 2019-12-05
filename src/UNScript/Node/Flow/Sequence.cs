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
    [NodeInterface("Sequence", "GameBox.Jigsaw/Flow/")]
    public sealed class Sequence : JNode
    {
        public override void Init()
        {
            Add(new JSpot("", JType.Category.Any, this, -1, SpotType.In));
            Add(new JSpot("0", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("1", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("2", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("3", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("4", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("5", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("6", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("7", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("8", JType.Category.Any, this, 1, SpotType.Out));
            Add(new JSpot("9", JType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            if (SpotType.In == spot.Type)
            {
                var instance = args[0] as JInstance;
                instance.LockState(() =>
                {
                    if (instance.IsLockedState()) { GetAt(1).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(2).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(3).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(4).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(5).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(6).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(7).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(8).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(9).Signal(args); }
                    if (instance.IsLockedState()) { GetAt(10).Signal(args); }
                });
            }
        }
    }
}