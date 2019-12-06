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
    /// <summary>
    /// Condition module checks input boolean value and trigger different output signal.
    /// </summary>
    [NodeInterface("Condition", "UNLang/Flow/")]
    public sealed class Condition : LangNode
    {
        public override void Init()
        {
            Add(new LangSpot("", LangType.Category.Boolean, this, 1, SpotType.In));
            Add(new LangSpot("True", LangType.Category.Any, this, 1, SpotType.Out));
            Add(new LangSpot("False", LangType.Category.Any, this, 1, SpotType.Out));
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