/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using UnityEngine;
using UNode;

namespace UNLang
{
    [NodeInterface("DeltaTime", "UNLang/UnityEngine/")]
    public sealed class DeltaTime : LangNode
    {
        public override void Init()
        {
            Add(new LangSpot("", LangType.Category.Object, this, -1, SpotType.In));
            Add(new LangSpot("", LangType.Category.Float, this, 1, SpotType.Out));
        }

        public override void OnSignal(Spot spot, params object[] args)
        {
            GetAt(1).Signal(args[0], Time.deltaTime);
        }
    }
}