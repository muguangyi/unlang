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
    public sealed class LangSpot : Spot
    {
        public LangSpot(string name, LangType.Category category, Node owner, int capacity, SpotType type)
            : base(owner, capacity, type)
        {
            this.Name = name;
            OnChangeTypeCategory(category);
        }

        public void OnChangeTypeCategory(LangType.Category category)
        {
            this.category = category;
            this.priority = QueryPriority(category);
        }

        protected override bool CanConnect(Spot spot)
        {
            if (!(spot is LangSpot))
            {
                return false;
            }

            var shuttleSpot = spot as LangSpot;
            if (shuttleSpot.priority == this.priority)
            {
                return (shuttleSpot.category == this.category);
            }
            else if ((SpotType.In == this.Type && shuttleSpot.priority > this.priority) ||
                     (SpotType.Out == this.Type && shuttleSpot.priority < this.priority))
            {
                return true;
            }

            return false;
        }

        private LangType.Category category = LangType.Category.Any;
        private uint priority = 0;

        private static uint QueryPriority(LangType.Category valueType)
        {
            switch (valueType)
            {
            case LangType.Category.Object:
                return 1;
            case LangType.Category.Boolean:
            case LangType.Category.Integer:
            case LangType.Category.Float:
            case LangType.Category.String:
            case LangType.Category.Vector2:
            case LangType.Category.Vector3:
            case LangType.Category.Vector4:
                return 2;
            default:
                return 0;
            }
        }
    }
}