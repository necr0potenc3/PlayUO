namespace Client
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public class GMacroKeyboard : GAlphaBackground
    {
        private GSystemButton m_All;
        private GSystemButton m_Alt;
        private bool m_Bold;
        private object[] m_Buttons;
        private GSystemButton m_Ctrl;
        private float m_fX;
        private float m_fY;
        private object[] m_HighButtons;
        private MacroModifiers m_Mods;
        private GSystemButton m_Shift;
        private const int Size = 0x1c;

        public GMacroKeyboard() : base(0, 0, 0x27f, 0xb8)
        {
            this.m_Bold = true;
            this.m_Buttons = new object[0x100];
            this.m_HighButtons = new object[0x100];
            base.FillColor = GumpColors.Control;
            base.FillAlpha = 1f;
            base.m_NonRestrictivePicking = true;
            int x = this.Width - 0x62;
            this.m_All = new GSystemButton(x - 0x13, 10, 20, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), Color.Black, "", Engine.GetUniFont(2));
            this.m_Ctrl = new GSystemButton(x, 10, 0x20, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), Color.Black, "Ctrl", Engine.GetUniFont(2));
            this.m_Alt = new GSystemButton(x + 0x1f, 10, 0x20, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), Color.Black, "Alt", Engine.GetUniFont(2));
            this.m_Shift = new GSystemButton(x + 0x3e, 10, 0x20, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), Color.Black, "Shift", Engine.GetUniFont(2));
            this.m_All.OnClick = new OnClick(this.All_OnClick);
            this.m_Ctrl.OnClick = new OnClick(this.Ctrl_OnClick);
            this.m_Alt.OnClick = new OnClick(this.Alt_OnClick);
            this.m_Shift.OnClick = new OnClick(this.Shift_OnClick);
            base.m_Children.Add(this.m_All);
            base.m_Children.Add(this.m_Ctrl);
            base.m_Children.Add(this.m_Alt);
            base.m_Children.Add(this.m_Shift);
            this.PlaceKey(Keys.Escape, "Esc");
            this.Skip();
            this.PlaceKey(Keys.F1);
            this.PlaceKey(Keys.F2);
            this.PlaceKey(Keys.F3);
            this.PlaceKey(Keys.F4);
            this.Skip(0.5f);
            this.PlaceKey(Keys.F5);
            this.PlaceKey(Keys.F6);
            this.PlaceKey(Keys.F7);
            this.PlaceKey(Keys.F8);
            this.Skip(0.5f);
            this.PlaceKey(Keys.F9);
            this.PlaceKey(Keys.F10);
            this.PlaceKey(Keys.F11);
            this.PlaceKey(Keys.F12);
            this.Skip(0.25f);
            this.m_Bold = false;
            this.PlaceKey(Keys.PrintScreen, "Prnt");
            this.PlaceKey(Keys.Scroll, "Scrl");
            this.PlaceKey(Keys.Pause, "Paus");
            this.m_Bold = true;
            this.m_fX = 0f;
            this.m_fY += 1.25f;
            this.PlaceKey(Keys.Oem3, "~");
            this.PlaceKey(Keys.D1, "1");
            this.PlaceKey(Keys.D2, "2");
            this.PlaceKey(Keys.D3, "3");
            this.PlaceKey(Keys.D4, "4");
            this.PlaceKey(Keys.D5, "5");
            this.PlaceKey(Keys.D6, "6");
            this.PlaceKey(Keys.D7, "7");
            this.PlaceKey(Keys.D8, "8");
            this.PlaceKey(Keys.D9, "9");
            this.PlaceKey(Keys.D0, "0");
            this.PlaceKey(Keys.OemMinus, "-");
            this.PlaceKey(Keys.Oemplus, "+");
            this.m_Bold = false;
            this.PlaceKey(Keys.Back, "Backspace", 2f);
            this.m_Bold = true;
            this.Skip(0.25f);
            this.m_Bold = false;
            this.PlaceKey(Keys.Insert, "Ins");
            this.PlaceKey(Keys.Home);
            this.m_Bold = true;
            this.PlaceKey(Keys.PageUp, "↑");
            this.Skip(0.25f);
            this.PlaceKey(Keys.NumLock, "Num");
            this.PlaceKey(Keys.Divide, "/");
            this.PlaceKey(Keys.Multiply, "*");
            this.PlaceKey(Keys.Subtract, "-");
            this.m_fX = 0f;
            this.m_fY++;
            this.PlaceKey(Keys.Tab, (float) 1.5f);
            this.PlaceKey(Keys.Q);
            this.PlaceKey(Keys.W);
            this.PlaceKey(Keys.E);
            this.PlaceKey(Keys.R);
            this.PlaceKey(Keys.T);
            this.PlaceKey(Keys.Y);
            this.PlaceKey(Keys.U);
            this.PlaceKey(Keys.I);
            this.PlaceKey(Keys.O);
            this.PlaceKey(Keys.P);
            this.PlaceKey(Keys.Oem4, "[");
            this.PlaceKey(Keys.Oem6, "]");
            this.PlaceKey(Keys.Oem5, @"\", 1.5f);
            this.Skip(0.25f);
            this.m_Bold = false;
            this.PlaceKey(Keys.Delete, "Del");
            this.PlaceKey(Keys.End);
            this.m_Bold = true;
            this.PlaceKey(Keys.Next, "↓");
            this.Skip(0.25f);
            this.PlaceKey(Keys.NumPad7, "7");
            this.PlaceKey(Keys.NumPad8, "8");
            this.PlaceKey(Keys.NumPad9, "9");
            this.PlaceKey(Keys.Add, "+", 1f, 2f);
            this.m_fX = 0f;
            this.m_fY++;
            this.PlaceKey(Keys.Capital, "Caps", 1.75f);
            this.PlaceKey(Keys.A);
            this.PlaceKey(Keys.S);
            this.PlaceKey(Keys.D);
            this.PlaceKey(Keys.F);
            this.PlaceKey(Keys.G);
            this.PlaceKey(Keys.H);
            this.PlaceKey(Keys.J);
            this.PlaceKey(Keys.K);
            this.PlaceKey(Keys.L);
            this.PlaceKey(Keys.Oem1, ";");
            this.PlaceKey(Keys.Oem7, "'");
            this.PlaceKey(Keys.Enter, (float) 2.25f);
            this.Skip(3.5f);
            this.PlaceKey(Keys.NumPad4, "4");
            this.PlaceKey(Keys.NumPad5, "5");
            this.PlaceKey(Keys.NumPad6, "6");
            this.m_fX = 0f;
            this.m_fY++;
            this.PlaceKey(Keys.ShiftKey, "Shift", 2.25f);
            this.PlaceKey(Keys.Z);
            this.PlaceKey(Keys.X);
            this.PlaceKey(Keys.C);
            this.PlaceKey(Keys.V);
            this.PlaceKey(Keys.B);
            this.PlaceKey(Keys.N);
            this.PlaceKey(Keys.M);
            this.PlaceKey(Keys.Oemcomma, ",");
            this.PlaceKey(Keys.OemPeriod, ".");
            this.PlaceKey(Keys.Oem2, "/");
            this.PlaceKey(Keys.ShiftKey, "Shift", 2.75f);
            this.Skip(1.25f);
            this.PlaceKey(Keys.Up, "↑");
            this.Skip(1.25f);
            this.PlaceKey(Keys.NumPad1, "1");
            this.PlaceKey(Keys.NumPad2, "2");
            this.PlaceKey(Keys.NumPad3, "3");
            this.m_Bold = false;
            this.PlaceKey(Keys.Enter, "Entr", 1f, 2f);
            this.m_Bold = true;
            this.m_fX = 0f;
            this.m_fY++;
            this.PlaceKey(Keys.ControlKey, "Ctrl", 1.5f);
            this.PlaceKey(Keys.LWin, "Win", 1.25f);
            this.PlaceKey(Keys.Menu, "Alt", 1.25f);
            this.PlaceKey(Keys.Space, (float) 5.75f);
            this.PlaceKey(Keys.Menu, "Alt", 1.25f);
            this.PlaceKey(Keys.RWin, "Win", 1.25f);
            this.PlaceKey(Keys.Apps, (float) 1.25f);
            this.PlaceKey(Keys.ControlKey, "Ctrl", 1.5f);
            this.Skip(0.25f);
            this.PlaceKey(Keys.Left, "←");
            this.PlaceKey(Keys.Down, "↓");
            this.PlaceKey(Keys.Right, "→");
            this.Skip(0.25f);
            this.PlaceKey(Keys.NumPad0, "0", 2f);
            this.PlaceKey(Keys.Decimal, ".");
            this.Mods = MacroModifiers.All;
        }

        private void All_OnClick(Gump g)
        {
            this.Mods ^= MacroModifiers.All;
        }

        private void Alt_OnClick(Gump g)
        {
            if ((this.m_Mods & MacroModifiers.All) == MacroModifiers.None)
            {
                this.Mods ^= MacroModifiers.Alt;
            }
        }

        private void Ctrl_OnClick(Gump g)
        {
            if ((this.m_Mods & MacroModifiers.All) == MacroModifiers.None)
            {
                this.Mods ^= MacroModifiers.Ctrl;
            }
        }

        private object GetButton(Keys key)
        {
            int num;
            if (key == Keys.Shift)
            {
                num = 0x10000;
            }
            else if (key == Keys.Alt)
            {
                num = 0x10001;
            }
            else if (key == Keys.Control)
            {
                num = 0x10002;
            }
            else
            {
                num = (int) key;
            }
            if ((num >= 0) && (num < this.m_Buttons.Length))
            {
                return this.m_Buttons[num];
            }
            num -= 0x10000;
            if ((num >= 0) && (num < this.m_HighButtons.Length))
            {
                return this.m_HighButtons[num];
            }
            return null;
        }

        protected internal override void OnDragStart()
        {
            if (base.m_Parent != null)
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                Point point = base.PointToScreen(new Point(0, 0)) - base.m_Parent.PointToScreen(new Point(0, 0));
                base.m_Parent.m_OffsetX = point.X + base.m_OffsetX;
                base.m_Parent.m_OffsetY = point.Y + base.m_OffsetY;
                base.m_Parent.m_IsDragging = true;
                Gumps.Drag = base.m_Parent;
            }
        }

        public void PlaceKey(Keys key)
        {
            this.PlaceKey(key, key.ToString(), 1f);
        }

        public void PlaceKey(Keys key, float w)
        {
            this.PlaceKey(key, key.ToString(), w);
        }

        public void PlaceKey(Keys key, string name)
        {
            this.PlaceKey(key, name, 1f);
        }

        public void PlaceKey(Keys key, string name, float w)
        {
            this.PlaceKey(key, name, w, 1f);
        }

        public void PlaceKey(Keys key, string name, float w, float h)
        {
            GMacroKeyButton btn = new GMacroKeyButton(key, name, this.m_Bold, 4 + ((int) (this.m_fX * 28f)), 4 + ((int) (this.m_fY * 28f)), 1 + ((int) (w * 28f)), 1 + ((int) (h * 28f)));
            this.SetButton(key, btn);
            base.m_Children.Add(btn);
            this.m_fX += w;
        }

        protected internal override void Render(int X, int Y)
        {
            base.Render(X, Y);
            if (Gumps.LastOver is GMenuItem)
            {
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(0.4f);
                Renderer.SolidRect(0, (X + base.m_X) + 1, (Y + base.m_Y) + 1, base.m_Width - 2, base.m_Height - 2);
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
        }

        public void SetButton(Keys key, GMacroKeyButton btn)
        {
            int num;
            if (key == Keys.Shift)
            {
                num = 0x10000;
            }
            else if (key == Keys.Alt)
            {
                num = 0x10001;
            }
            else if (key == Keys.Control)
            {
                num = 0x10002;
            }
            else
            {
                num = (int) key;
            }
            if ((num >= 0) && (num < this.m_Buttons.Length))
            {
                this.SetButton(this.m_Buttons, num, btn);
            }
            else
            {
                num -= 0x10000;
                if ((num >= 0) && (num < this.m_HighButtons.Length))
                {
                    this.SetButton(this.m_HighButtons, num, btn);
                }
            }
        }

        private void SetButton(object[] objs, int index, GMacroKeyButton btn)
        {
            object obj2 = objs[index];
            if (!(obj2 is GMacroKeyButton[]))
            {
                if (obj2 is GMacroKeyButton)
                {
                    objs[index] = new GMacroKeyButton[] { (GMacroKeyButton) obj2, btn };
                }
                else
                {
                    objs[index] = btn;
                }
            }
        }

        private void SetMacro(ArrayList list, GMacroKeyButton btn, Macro mc)
        {
            if (list.Contains(btn) || (btn.Macro == null))
            {
                list.Remove(btn);
                btn.Macro = mc;
            }
            else if (btn.Macro is Macro)
            {
                btn.Macro = new Macro[] { (Macro) btn.Macro, mc };
            }
            else if (btn.Macro is Macro[])
            {
                Macro[] macro = (Macro[]) btn.Macro;
                Macro[] macroArray2 = new Macro[macro.Length + 1];
                for (int i = 0; i < macro.Length; i++)
                {
                    macroArray2[i] = macro[i];
                }
                macroArray2[macro.Length] = mc;
                btn.Macro = macroArray2;
            }
        }

        private void Shift_OnClick(Gump g)
        {
            if ((this.m_Mods & MacroModifiers.All) == MacroModifiers.None)
            {
                this.Mods ^= MacroModifiers.Shift;
            }
        }

        public void Skip()
        {
            this.Skip(1f);
        }

        public void Skip(float w)
        {
            this.m_fX += w;
        }

        public void Update()
        {
            ArrayList dataStore = Engine.GetDataStore();
            Gump[] gumpArray = base.m_Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                GMacroKeyButton button = gumpArray[i] as GMacroKeyButton;
                if ((button != null) && (button.Macro != null))
                {
                    dataStore.Add(button);
                }
            }
            bool flag = (this.m_Mods & MacroModifiers.All) != MacroModifiers.None;
            bool flag2 = (this.m_Mods & MacroModifiers.Ctrl) != MacroModifiers.None;
            bool flag3 = (this.m_Mods & MacroModifiers.Alt) != MacroModifiers.None;
            bool flag4 = (this.m_Mods & MacroModifiers.Shift) != MacroModifiers.None;
            ArrayList list = Macros.List;
            for (int j = 0; j < list.Count; j++)
            {
                Macro mc = (Macro) list[j];
                if (flag || (((mc.Control == flag2) && (mc.Alt == flag3)) && (mc.Shift == flag4)))
                {
                    object obj2 = this.GetButton(mc.Key);
                    if (obj2 is GMacroKeyButton[])
                    {
                        GMacroKeyButton[] buttonArray = (GMacroKeyButton[]) obj2;
                        for (int m = 0; m < buttonArray.Length; m++)
                        {
                            this.SetMacro(dataStore, buttonArray[m], mc);
                        }
                    }
                    else if (obj2 is GMacroKeyButton)
                    {
                        this.SetMacro(dataStore, (GMacroKeyButton) obj2, mc);
                    }
                }
            }
            for (int k = 0; k < dataStore.Count; k++)
            {
                ((GMacroKeyButton) dataStore[k]).Macro = null;
            }
            Engine.ReleaseDataStore(dataStore);
        }

        private void UpdateModifier(GSystemButton btn, string prefix, bool enabled, bool opt)
        {
            if (!enabled)
            {
                btn.InactiveColor = btn.ActiveColor = btn.PressedColor = SystemColors.Control;
            }
            else if (opt)
            {
                btn.SetBackColor(GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.5f));
            }
            else
            {
                btn.SetBackColor(GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.25f));
                btn.InactiveColor = GumpPaint.Blend(Color.White, SystemColors.Control, (float) 0.5f);
            }
        }

        private void UpdateModifiers()
        {
            bool opt = (this.m_Mods & MacroModifiers.All) == MacroModifiers.None;
            this.UpdateModifier(this.m_All, "", true, opt);
            this.UpdateModifier(this.m_Ctrl, "Ctrl", opt, (this.m_Mods & MacroModifiers.Ctrl) != MacroModifiers.None);
            this.UpdateModifier(this.m_Alt, "Alt", opt, (this.m_Mods & MacroModifiers.Alt) != MacroModifiers.None);
            this.UpdateModifier(this.m_Shift, "Shift", opt, (this.m_Mods & MacroModifiers.Shift) != MacroModifiers.None);
        }

        public MacroModifiers Mods
        {
            get
            {
                return this.m_Mods;
            }
            set
            {
                this.m_Mods = value;
                this.Update();
                this.UpdateModifiers();
            }
        }
    }
}

