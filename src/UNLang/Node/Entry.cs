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
    [NodeInterface("Entry", "UNLang/")]
    public sealed class Entry : LangNode
    {
        public override void Init()
        {
            Add(new LangSpot("", LangType.Category.Any, this, 1, SpotType.Out));
        }

        public override void Begin(LangInstance instance)
        {
            GetAt(0).Signal(instance);
        }
    }
}