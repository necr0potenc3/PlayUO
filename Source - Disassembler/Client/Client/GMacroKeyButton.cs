namespace Client
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GMacroKeyButton : GSystemButton
    {
        private GLabel m_Dots;
        private Keys m_Key;
        private object m_Macro;

        public GMacroKeyButton(Keys key, string name, bool bold, int x, int y, int w, int h) : base(x, y, w, h, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), SystemColors.ControlText, name, bold ? Engine.GetUniFont(1) : Engine.GetUniFont(2))
        {
            this.m_Key = key;
            base.Tooltip = new Tooltip(string.Format("{0}\nClick to create", GMacroEditorPanel.GetKeyName(this.m_Key)), true);
            base.FillAlpha = 1f;
            base.m_QuickDrag = false;
            base.m_CanDrag = true;
            base.OnClick = new OnClick(this.Clicked);
        }

        private void Clicked(Gump g)
        {
            GMacroEditorForm parent = base.m_Parent.Parent as GMacroEditorForm;
            if (parent != null)
            {
                if (this.m_Macro == null)
                {
                    Keys none = Keys.None;
                    MacroModifiers all = MacroModifiers.All;
                    if (parent.Keyboard != null)
                    {
                        all = parent.Keyboard.Mods;
                    }
                    if (all == MacroModifiers.All)
                    {
                        none = Control.ModifierKeys;
                    }
                    else
                    {
                        if ((all & MacroModifiers.Alt) != MacroModifiers.None)
                        {
                            none |= Keys.Alt;
                        }
                        if ((all & MacroModifiers.Shift) != MacroModifiers.None)
                        {
                            none |= Keys.Shift;
                        }
                        if ((all & MacroModifiers.Ctrl) != MacroModifiers.None)
                        {
                            none |= Keys.Control;
                        }
                    }
                    Client.Macro macro = new Client.Macro(this.m_Key, none, new Action[0]);
                    Macros.List.Add(macro);
                    parent.Current = macro;
                    parent.UpdateKeyboard();
                }
                else if (this.m_Macro is Client.Macro)
                {
                    parent.Current = (Client.Macro) this.m_Macro;
                }
                else if (this.m_Macro is Client.Macro[])
                {
                    Client.Macro[] array = (Client.Macro[]) this.m_Macro;
                    int index = Array.IndexOf(array, parent.Current);
                    parent.Current = array[(index + 1) % array.Length];
                }
            }
        }

        protected internal override void OnDragStart()
        {
            if (base.m_Parent.Parent != null)
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                Point point = base.PointToScreen(new Point(0, 0)) - base.m_Parent.Parent.PointToScreen(new Point(0, 0));
                base.m_Parent.Parent.m_OffsetX = point.X + base.m_OffsetX;
                base.m_Parent.Parent.m_OffsetY = point.Y + base.m_OffsetY;
                base.m_Parent.Parent.m_IsDragging = true;
                Gumps.Drag = base.m_Parent.Parent;
            }
        }

        public override float Darkness
        {
            get
            {
                return 0.25f;
            }
        }

        public Keys Key
        {
            get
            {
                return this.m_Key;
            }
        }

        public object Macro
        {
            get
            {
                return this.m_Macro;
            }
            set
            {
                if (this.m_Macro != value)
                {
                    this.m_Macro = value;
                    if (this.m_Macro == null)
                    {
                        base.SetBackColor(GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f));
                        if (this.m_Dots != null)
                        {
                            Gumps.Destroy(this.m_Dots);
                        }
                        this.m_Dots = null;
                        base.Tooltip = new Tooltip(string.Format("{0}\nClick to create", GMacroEditorPanel.GetKeyName(this.m_Key)), true);
                    }
                    else
                    {
                        base.SetBackColor(GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.5f));
                        int count = 1;
                        if (this.m_Macro is Client.Macro)
                        {
                            base.Tooltip = new Tooltip("Jump to the macro", true);
                        }
                        else
                        {
                            base.Tooltip = new Tooltip("Jump to the macros", true);
                            count = ((Client.Macro[]) this.m_Macro).Length;
                        }
                        if (this.m_Dots == null)
                        {
                            this.m_Dots = new GLabel(new string('.', count), Engine.GetUniFont(0), Hues.Load(0x481), 4, 4);
                            this.m_Dots.X -= this.m_Dots.Image.xMin;
                            this.m_Dots.Y -= this.m_Dots.Image.yMin;
                            base.m_Children.Add(this.m_Dots);
                        }
                        else if (this.m_Dots.Text.Length != count)
                        {
                            this.m_Dots.Text = new string('.', count);
                        }
                    }
                }
            }
        }
    }
}

