namespace Client
{
    using System;
    using System.Reflection;

    public class ObjectEditorEntry : IComparable
    {
        private OptionHueAttribute m_Hue;
        private object m_Object;
        private OptionableAttribute m_Optionable;
        private PropertyInfo m_Property;
        private OptionRangeAttribute m_Range;

        public ObjectEditorEntry(PropertyInfo prop, object obj, object optionable, object range, object hue)
        {
            this.m_Property = prop;
            this.m_Object = obj;
            this.m_Optionable = optionable as OptionableAttribute;
            this.m_Range = range as OptionRangeAttribute;
            this.m_Hue = hue as OptionHueAttribute;
        }

        public int CompareTo(object obj)
        {
            ObjectEditorEntry entry = (ObjectEditorEntry)obj;
            return this.m_Optionable.Name.CompareTo(entry.m_Optionable.Name);
        }

        public OptionHueAttribute Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public object Object
        {
            get
            {
                return this.m_Object;
            }
        }

        public OptionableAttribute Optionable
        {
            get
            {
                return this.m_Optionable;
            }
        }

        public PropertyInfo Property
        {
            get
            {
                return this.m_Property;
            }
        }

        public OptionRangeAttribute Range
        {
            get
            {
                return this.m_Range;
            }
        }
    }
}