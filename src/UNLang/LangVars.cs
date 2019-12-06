/*
 * This file is part of the UNLang package.
 *
 * (c) MuGuangyi <muguangyi@hotmail.com>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace UNLang
{
    /// <summary>
    /// UNLang variable container.
    /// </summary>
    public sealed class LangVars : IDisposable
    {
        public enum Scope
        {
            Global,
            Owner,
            Instance,
            State,
        }

        private readonly Dictionary<string, IObject> vars = null;
        private static LangVars globalVars = null;

        public LangVars(object host = null)
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
        { }

        public void Reset()
        {
            this.vars.Clear();
        }

        public void Set(string key, object value)
        {
            if (!this.vars.TryGetValue(key, out IObject obj))
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
            if (this.vars.TryGetValue(key, out IObject obj))
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

            public object Value { get; set; }
        }

        private class HostObject : IObject
        {
            private readonly object host = null;
            private readonly PropertyInfo property = null;

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
        }

        public static LangVars GlobalVars
        {
            get
            {
                if (null == globalVars)
                {
                    globalVars = new LangVars();
                }

                return globalVars;
            }
        }
    }
}