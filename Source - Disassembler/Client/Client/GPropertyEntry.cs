namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GPropertyEntry : GEmpty
    {
        private Clipper m_Clipper;
        private ObjectEditorEntry m_Entry;
        private GAlphaBackground m_Hue;
        private GLabel m_Name;
        private GAlphaBackground m_NameBack;
        private object m_Object;
        private GPropertyHuePicker m_Picker;
        private GLabel m_Value;
        private GAlphaBackground m_ValueBack;

        public GPropertyEntry(object obj, ObjectEditorEntry entry) : base(0, 0, 0x117, 0x16)
        {
            Client.IFont uniFont;
            this.m_Object = obj;
            this.m_Entry = entry;
            base.m_NonRestrictivePicking = true;
            this.m_NameBack = new GAlphaBackground(0, 0, 140, 0x16);
            this.m_NameBack.FillColor = GumpColors.Window;
            this.m_NameBack.FillAlpha = 1f;
            this.m_NameBack.DrawBorder = false;
            this.m_NameBack.ShouldHitTest = false;
            base.m_Children.Add(this.m_NameBack);
            this.m_ValueBack = new GAlphaBackground(0x8b, 0, 140, 0x16);
            this.m_ValueBack.FillColor = GumpColors.Window;
            this.m_ValueBack.FillAlpha = 1f;
            this.m_ValueBack.BorderColor = GumpColors.Control;
            this.m_ValueBack.ShouldHitTest = false;
            this.m_ValueBack.DrawBorder = false;
            base.m_Children.Add(this.m_ValueBack);
            this.m_Name = new GLabel(entry.Optionable.Name, Engine.GetUniFont(2), GumpHues.WindowText, 0, 0);
            this.m_Name.X = 5 - this.m_Name.Image.xMin;
            this.m_Name.Y = ((0x16 - ((this.m_Name.Image.yMax - this.m_Name.Image.yMin) + 1)) / 2) - this.m_Name.Image.yMin;
            this.m_NameBack.Children.Add(this.m_Name);
            object val = entry.Property.GetValue(obj, null);
            string valString = this.GetValString(val);
            if ((val is ValueType) ? val.Equals(entry.Optionable.Default) : object.ReferenceEquals(val, entry.Optionable.Default))
            {
                uniFont = Engine.GetUniFont(2);
            }
            else
            {
                uniFont = Engine.GetUniFont(1);
            }
            this.m_Value = new GLabel(valString, uniFont, GumpHues.WindowText, 0, 0);
            if (entry.Hue != null)
            {
                GAlphaBackground toAdd = new GAlphaBackground(4, 4, 0x16, 14);
                toAdd.FillColor = Engine.C16232(Hues.Load((int)val).Pixel(0xffff));
                toAdd.FillAlpha = 1f;
                toAdd.ShouldHitTest = false;
                this.m_ValueBack.Children.Add(toAdd);
                this.m_Value.Text = "Hue";
                this.m_Value.X = 30 - this.m_Value.Image.xMin;
                this.m_Value.Y = ((0x16 - ((this.m_Value.Image.yMax - this.m_Value.Image.yMin) + 1)) / 2) - this.m_Value.Image.yMin;
                this.m_Hue = toAdd;
            }
            else
            {
                this.m_Value.X = 5 - this.m_Value.Image.xMin;
                this.m_Value.Y = ((0x16 - ((this.m_Value.Image.yMax - this.m_Value.Image.yMin) + 1)) / 2) - this.m_Value.Image.yMin;
            }
            this.m_ValueBack.Children.Add(this.m_Value);
        }

        private string GetValString(object val)
        {
            if (val == null)
            {
                return "null";
            }
            if (val is bool)
            {
                return (((bool)val) ? "On" : "Off");
            }
            if (val is Item)
            {
                return Localization.GetString(0xf9060 + (((Item)val).ID & 0x3fff));
            }
            return val.ToString();
        }

        protected internal override bool HitTest(int X, int Y)
        {
            Point p = base.PointToScreen(new Point(X, Y));
            return ((this.m_Clipper == null) || this.m_Clipper.Evaluate(p));
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            this.m_NameBack.FillColor = GumpPaint.Blend(GumpColors.Window, GumpColors.Highlight, (float)0.9f);
            this.m_ValueBack.FillColor = this.m_NameBack.FillColor;
        }

        protected internal override void OnMouseLeave()
        {
            this.m_NameBack.FillColor = GumpColors.Window;
            this.m_ValueBack.FillColor = this.m_NameBack.FillColor;
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Left)
            {
                ((GEditorPanel)base.Parent.Parent).Reset();
                object obj2 = this.m_Entry.Property.GetValue(this.m_Object, null);
                if (obj2 is bool)
                {
                    this.SetValue(!((bool)obj2));
                }
                else if ((obj2 is Item) || (this.m_Entry.Property.PropertyType == typeof(Item)))
                {
                    Engine.TargetHandler = new SetItemPropertyTarget(this);
                }
                else if (obj2 is Enum)
                {
                    Array values = Enum.GetValues(obj2.GetType());
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values.GetValue(i).Equals(obj2))
                        {
                            this.SetValue(values.GetValue((int)((i + 1) % values.Length)));
                            break;
                        }
                    }
                }
                else if (this.m_Entry.Hue != null)
                {
                    if (this.m_Picker == null)
                    {
                        GPropertyHuePicker toAdd = this.m_Picker = new GPropertyHuePicker(this);
                        toAdd.X = this.Width - 1;
                        toAdd.Y = 0;
                        base.m_Children.Add(toAdd);
                    }
                }
                else if (this.m_Entry.Property.IsDefined(typeof(MacroEditorAttribute), true))
                {
                    Gumps.Destroy(base.Parent.Parent.Parent.Parent);
                    GMacroEditorForm.Open();
                }
            }
        }

        public void Reset()
        {
            if (this.m_Picker != null)
            {
                Gumps.Destroy(this.m_Picker);
            }
            this.m_Picker = null;
        }

        public void SetClipper(Clipper c)
        {
            this.m_Clipper = c;
            this.m_Name.Clipper = c;
            this.m_Value.Clipper = c;
            this.m_NameBack.Clipper = c;
            this.m_ValueBack.Clipper = c;
            if (this.m_Hue != null)
            {
                this.m_Hue.Clipper = c;
            }
        }

        public void SetValue(object val)
        {
            Client.IFont uniFont;
            this.m_Entry.Property.SetValue(this.m_Object, val, null);
            if ((val is ValueType) ? val.Equals(this.m_Entry.Optionable.Default) : object.ReferenceEquals(val, this.m_Entry.Optionable.Default))
            {
                uniFont = Engine.GetUniFont(2);
            }
            else
            {
                uniFont = Engine.GetUniFont(1);
            }
            if (this.m_Hue == null)
            {
                this.m_Value.Text = this.GetValString(val);
            }
            else
            {
                this.m_Hue.FillColor = Engine.C16232(Hues.Load((int)val).Pixel(0xffff));
            }
            this.m_Value.Font = uniFont;
        }

        public ObjectEditorEntry Entry
        {
            get
            {
                return this.m_Entry;
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