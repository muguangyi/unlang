/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UNode;

namespace UNLang
{
    [NodeInterface("Sequence", "UNLang/Flow/")]
    public sealed class Sequence : LangNode
    {
        public override void Init()
        {
            Add(new LangSpot("", LangType.Category.Any, this, -1, SpotType.In));
            Add(new LangSpot("0", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("1", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("2", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("3", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("4", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("5", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("6", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("7", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("8", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("9", LangType.Category.Any, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            if (SpotType.In == spot.Type)
            {
                var instance = args[0] as LangInstance;
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