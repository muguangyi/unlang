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
    [NodeInterface("Entry", "GameBox.Jigsaw/")]
    public sealed class Entry : JNode
    {
        public override void Init()
        {
            Add(new JSpot("", JType.Category.Any, this, 1, SpotType.Out));
        }

        public override void Begin(JInstance instance)
        {
            GetAt(0).Signal(instance);
        }
    }
}