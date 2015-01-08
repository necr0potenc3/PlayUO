namespace Client
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    public class GObjectEditor : GWindowsForm
    {
        private static GObjectEditor m_Instance;
        private object m_Object;
        private GEditorPanel m_Panel;

        public GObjectEditor(object obj) : base(0, 0, 0x13d, 0x188)
        {
            Gumps.Focus = this;
            this.m_Object = obj;
            base.m_NonRestrictivePicking = true;
            base.Text = "Option Editor";
            PropertyInfo[] properties = obj.GetType().GetProperties();
            Hashtable c = new Hashtable();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo mi = properties[i];
                OptionableAttribute optionable = this.GetAttribute(mi, typeof(OptionableAttribute)) as OptionableAttribute;
                if (optionable != null)
                {
                    ArrayList list = (ArrayList) c[optionable.Category];
                    if (list == null)
                    {
                        c[optionable.Category] = list = new ArrayList();
                    }
                    list.Add(new ObjectEditorEntry(mi, obj, optionable, this.GetAttribute(mi, typeof(OptionRangeAttribute)), this.GetAttribute(mi, typeof(OptionHueAttribute))));
                }
            }
            ArrayList list2 = new ArrayList(c);
            list2.Sort(new CategorySorter());
            ArrayList panels = new ArrayList();
            foreach (DictionaryEntry entry in list2)
            {
                string key = (string) entry.Key;
                ArrayList entries = (ArrayList) entry.Value;
                GCategoryPanel panel = new GCategoryPanel(obj, key, entries);
                panels.Add(panel);
            }
            GEditorPanel toAdd = new GEditorPanel(panels, 360);
            this.m_Panel = toAdd;
            toAdd.X += 2;
            toAdd.Y += 3;
            base.Client.m_NonRestrictivePicking = true;
            base.Client.Children.Add(toAdd);
            this.Center();
        }

        protected internal override void Draw(int X, int Y)
        {
            base.Draw(X, Y);
            if (((Gumps.Focus is GSliderBase) || (Gumps.Focus == null)) || !Gumps.Focus.IsChildOf(this))
            {
                this.m_Panel.Reset();
            }
        }

        public object GetAttribute(MemberInfo mi, Type type)
        {
            object[] customAttributes = mi.GetCustomAttributes(type, false);
            if (customAttributes == null)
            {
                return null;
            }
            if (customAttributes.Length == 0)
            {
                return null;
            }
            return customAttributes[0];
        }

        protected internal override void OnDispose()
        {
            if (this.m_Object is CharData)
            {
                ((CharData) this.m_Object).Save();
            }
            if (Engine.TargetHandler is SetItemPropertyTarget)
            {
                Engine.TargetHandler = null;
            }
            m_Instance = null;
        }

        protected internal override void OnDragStart()
        {
            base.OnDragStart();
            this.m_Panel.Reset();
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            this.m_Panel.Reset();
        }

        public static void Open(object obj)
        {
            if (m_Instance == null)
            {
                m_Instance = new GObjectEditor(obj);
                Gumps.Desktop.Children.Add(m_Instance);
                Gumps.Focus = m_Instance;
            }
        }

        public static bool IsOpen
        {
            get
            {
                return (m_Instance != null);
            }
        }

        public object Object
        {
            get
            {
                return this.m_Object;
            }
        }
    }
}

