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
    public sealed class JSpot : Spot
    {
        public JSpot(string name, JType.Category category, Node owner, int capacity, SpotType type)
            : base(owner, capacity, type)
        {
            this.Name = name;
            OnChangeTypeCategory(category);
        }

        public void OnChangeTypeCategory(JType.Category category)
        {
            this.category = category;
            this.priority = QueryPriority(category);
        }

        protected override bool CanConnect(Spot spot)
        {
            if (!(spot is JSpot))
            {
                return false;
            }

            var shuttleSpot = spot as JSpot;
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

        private JType.Category category = JType.Category.Any;
        private uint priority = 0;

        private static uint QueryPriority(JType.Category valueType)
        {
            switch (valueType)
            {
            case JType.Category.Object:
                return 1;
            case JType.Category.Boolean:
            case JType.Category.Integer:
            case JType.Category.Float:
            case JType.Category.String:
            case JType.Category.Vector2:
            case JType.Category.Vector3:
            case JType.Category.Vector4:
                return 2;
            default:
                return 0;
            }
        }
    }
}