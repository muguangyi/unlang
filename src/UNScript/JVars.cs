/*
 * This file is part of the UNScript package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace UNScript
{
    public sealed class JVars : IDisposable
    {
        public enum Scope
        {
            Global,
            Owner,
            Instance,
            State,
        }

        public JVars(object host = null)
        {
            this.vars = new Dictionary<string, IObject>();

            if (null != host)
            {
                var type = host.GetType();
                var properties = type.GetProperties();
                foreach (var p in properties)
                {
                    this.vars.Add(p.Name, new HostObject(host, p));
                }
            }
        }

        public void Dispose()
        {
            this.vars = null;
        }

        public void Reset()
        {
            this.vars.Clear();
        }

        public void Set(string key, object value)
        {
            IObject obj = null;
            if (!this.vars.TryGetValue(key, out obj))
            {
                this.vars.Add(key, obj = new ValueObject());
            }

            obj.Value = value;
        }

        public object Get(string key)
        {
            return this.vars[key].Value;
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            IObject obj = null;
            if (this.vars.TryGetValue(key, out obj))
            {
                return (T)obj.Value;
            }
            else
            {
                this.vars.Add(key, new ValueObject { Value = defaultValue });
                return defaultValue;
            }
        }

        private interface IObject
        {
            object Value { get; set; }
        }

        private class ValueObject : IObject
        {
            public ValueObject()
            { }

            public object Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    this.value = value;
                }
            }

            private object value;
        }

        private class HostObject : IObject
        {
            public HostObject(object host, PropertyInfo property)
            {
                this.host = host;
                this.property = property;
            }

            public object Value
            {
                get
                {
                    if (this.property.GetGetMethod().IsPublic)
                    {
                        return this.property.GetValue(this.host, null);
                    }
                    else
                    {
                        return null;
                    }
                }
                set
                {
                    if (this.property.GetSetMethod().IsPublic)
                    {
                        this.property.SetValue(this.host, value, null);
                    }
                }
            }

            private object host = null;
            private PropertyInfo property = null;
        }

        private Dictionary<string, IObject> vars = null;

        public static JVars GlobalVars
        {
            get
            {
                if (null == globalVars)
                {
                    globalVars = new JVars();
                }

                return globalVars;
            }
        }

        private static JVars globalVars = null;
    }
}