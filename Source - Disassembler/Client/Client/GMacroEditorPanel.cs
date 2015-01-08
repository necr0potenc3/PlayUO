namespace Client
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GMacroEditorPanel : GAlphaBackground
    {
        private static string[] m_Aliases;
        private GSystemButton m_Alt;
        private GSystemButton m_Ctrl;
        private Client.Macro m_Macro;
        private GSystemButton m_Shift;

        public GMacroEditorPanel(Client.Macro m) : base(0, 0, 0x103, 230)
        {
            GMainMenu menu;
            GMenuItem menuFrom;
            this.m_Macro = m;
            base.m_CanDrag = false;
            base.m_NonRestrictivePicking = true;
            base.ShouldHitTest = false;
            this.m_Ctrl = new GSystemButton(10, 10, 40, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), SystemColors.ControlText, "Ctrl", Engine.GetUniFont(2));
            this.m_Alt = new GSystemButton(0x31, 10, 40, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), SystemColors.ControlText, "Alt", Engine.GetUniFont(2));
            this.m_Shift = new GSystemButton(0x58, 10, 0x2a, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), SystemColors.ControlText, "Shift", Engine.GetUniFont(2));
            this.m_Ctrl.OnClick = new OnClick(this.Ctrl_OnClick);
            this.m_Alt.OnClick = new OnClick(this.Alt_OnClick);
            this.m_Shift.OnClick = new OnClick(this.Shift_OnClick);
            this.m_Ctrl.Tooltip = new Tooltip("Toggles the control key modifier", true);
            this.m_Alt.Tooltip = new Tooltip("Toggles the alt key modifier", true);
            this.m_Shift.Tooltip = new Tooltip("Toggles the shift key modifier", true);
            base.m_Children.Add(this.m_Ctrl);
            base.m_Children.Add(this.m_Alt);
            base.m_Children.Add(this.m_Shift);
            this.UpdateModifiers();
            GAlphaBackground toAdd = new GAlphaBackground(0x81, 10, 0x4a, 20) {
                FillAlpha = 1f,
                FillColor = GumpColors.Window
            };
            base.m_Children.Add(toAdd);
            GMacroKeyEntry entry = new GMacroKeyEntry(GetKeyName(m.Key), 0x81, 10, 0x4a, 20) {
                Tooltip = new Tooltip("Press any key here to change the macro", true)
            };
            base.m_Children.Add(entry);
            GSystemButton button = new GSystemButton(10, 10, 40, 20, GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f), SystemColors.ControlText, "Delete", Engine.GetUniFont(2));
            button.SetBackColor(GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.25f));
            button.InactiveColor = GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f);
            button.Tooltip = new Tooltip("Deletes the entire macro", true);
            button.OnClick = new OnClick(this.Delete_OnClick);
            button.X = (this.Width - 10) - button.Width;
            base.m_Children.Add(button);
            base.FillAlpha = 0.15f;
            for (int i = 0; i < m.Actions.Length; i++)
            {
                try
                {
                    Action action = m.Actions[i];
                    if (action.Handler != null)
                    {
                        ActionHandler ah = action.Handler;
                        menu = new GMainMenu(10, 0x23 + (i * 0x17));
                        menuFrom = new GActionMenu(this, m, action);
                        menu.Add(this.FormatMenu(menuFrom));
                        if (ah.Params == null)
                        {
                            GAlphaBackground background2 = new GAlphaBackground(0x81, 0x23 + (i * 0x17), 120, 0x18) {
                                FillAlpha = 1f,
                                FillColor = GumpColors.Window
                            };
                            base.m_Children.Add(background2);
                            IHue windowText = GumpHues.WindowText;
                            GTextBox box = new GMacroParamEntry(action, action.Param, background2.X, background2.Y, background2.Width, background2.Height) {
                                MaxChars = 0xef
                            };
                            base.m_Children.Add(box);
                        }
                        else if (ah.Params.Length != 0)
                        {
                            string name = Find(action.Param, ah.Params);
                            if (name == null)
                            {
                                name = action.Param;
                            }
                            menuFrom = this.GetMenuFrom(new ParamNode(name, ah.Params), action, ah);
                            menuFrom.DropDown = i == (m.Actions.Length - 1);
                            menu.Add(menuFrom);
                        }
                        menu.LeftToRight = true;
                        base.m_Children.Add(menu);
                    }
                }
                catch
                {
                }
            }
            menu = new GMainMenu(10, 0x23 + (m.Actions.Length * 0x17));
            menuFrom = this.GetMenuFrom(ActionHandler.Root);
            menuFrom.Tooltip = new Tooltip("To create a new instruction pick one from the menu below", false, 200);
            menuFrom.Text = "New...";
            menuFrom.DropDown = true;
            menu.Add(this.FormatMenu(menuFrom));
            menu.LeftToRight = true;
            base.m_Children.Add(menu);
        }

        private void Alt_OnClick(Gump g)
        {
            this.m_Macro.Alt = !this.m_Macro.Alt;
            this.UpdateModifier(this.m_Alt, "Alt", this.m_Macro.Alt);
            this.NotifyParent();
        }

        private void Ctrl_OnClick(Gump g)
        {
            this.m_Macro.Control = !this.m_Macro.Control;
            this.UpdateModifier(this.m_Ctrl, "Ctrl", this.m_Macro.Control);
            this.NotifyParent();
        }

        private void Delete_OnClick(Gump g)
        {
            Macros.List.Remove(this.m_Macro);
            GMacroEditorForm parent = base.m_Parent.Parent as GMacroEditorForm;
            if (parent != null)
            {
                parent.Current = null;
                parent.UpdateKeyboard();
            }
        }

        protected internal override void Draw(int X, int Y)
        {
        }

        public static string Find(string toFind, ParamNode[] nodes)
        {
            for (int i = 0; (nodes != null) && (i < nodes.Length); i++)
            {
                string str = Find(toFind, nodes[i]);
                if (str != null)
                {
                    return str;
                }
            }
            return null;
        }

        public static string Find(string toFind, ParamNode n)
        {
            if (n.Param == toFind)
            {
                return n.Name;
            }
            return Find(toFind, n.Nodes);
        }

        private GMenuItem FormatMenu(GMenuItem mi)
        {
            mi.FillAlpha = 1f;
            mi.DefaultColor = GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f);
            mi.OverColor = GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.5f);
            mi.ExpandedColor = GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.5f);
            mi.SetHue(Hues.Load(1));
            return mi;
        }

        public static string GetKeyName(Keys key)
        {
            if (key == 0x11000)
            {
                return "Wheel Up";
            }
            if (key == 0x11001)
            {
                return "Wheel Down";
            }
            if (key == 0x11002)
            {
                return "Wheel Press";
            }
            if (m_Aliases == null)
            {
                LoadAliases();
            }
            int index = (int) key;
            if (((index >= 0) && (index < m_Aliases.Length)) && (m_Aliases[index] != null))
            {
                return m_Aliases[index];
            }
            return key.ToString();
        }

        private GMenuItem GetMenuFrom(ActionNode n)
        {
            GMenuItem mi = new GMenuItem(n.Name);
            for (int i = 0; i < n.Nodes.Count; i++)
            {
                mi.Add(this.GetMenuFrom((ActionNode) n.Nodes[i]));
            }
            for (int j = 0; j < n.Handlers.Count; j++)
            {
                ActionHandler action = (ActionHandler) n.Handlers[j];
                GMenuItem item2 = new GNewActionMenu(this, this.m_Macro, action);
                for (int k = 0; (action.Params != null) && (k < action.Params.Length); k++)
                {
                    item2.Add(this.GetMenuFrom(action.Params[k], null, action));
                }
                mi.Add(this.FormatMenu(item2));
            }
            return this.FormatMenu(mi);
        }

        private GMenuItem GetMenuFrom(ParamNode n, Action a, ActionHandler ah)
        {
            GMenuItem item;
            if (n.Param != null)
            {
                item = new GParamMenu(n, ah, a);
            }
            else
            {
                item = new GMenuItem(n.Name);
            }
            if (n.Nodes != null)
            {
                for (int i = 0; i < n.Nodes.Length; i++)
                {
                    item.Add(this.GetMenuFrom(n.Nodes[i], a, ah));
                }
            }
            return this.FormatMenu(item);
        }

        private static void LoadAliases()
        {
            m_Aliases = new string[0x100];
            SetAlias(Keys.Add, "Num +");
            SetAlias(Keys.Back, "Backspace");
            SetAlias(Keys.Capital, "Caps Lock");
            SetAlias(Keys.ControlKey, "Control");
            SetAlias(Keys.D0, "0");
            SetAlias(Keys.D1, "1");
            SetAlias(Keys.D2, "2");
            SetAlias(Keys.D3, "3");
            SetAlias(Keys.D4, "4");
            SetAlias(Keys.D5, "5");
            SetAlias(Keys.D6, "6");
            SetAlias(Keys.D7, "7");
            SetAlias(Keys.D8, "8");
            SetAlias(Keys.D9, "9");
            SetAlias(Keys.Decimal, "Num .");
            SetAlias(Keys.Divide, "Num /");
            SetAlias(Keys.Menu, "Alt");
            SetAlias(Keys.Multiply, "Num *");
            SetAlias(Keys.NumLock, "Num Lock");
            SetAlias(Keys.NumPad0, "Num 0");
            SetAlias(Keys.NumPad1, "Num 1");
            SetAlias(Keys.NumPad2, "Num 2");
            SetAlias(Keys.NumPad3, "Num 3");
            SetAlias(Keys.NumPad4, "Num 4");
            SetAlias(Keys.NumPad5, "Num 5");
            SetAlias(Keys.NumPad6, "Num 6");
            SetAlias(Keys.NumPad7, "Num 7");
            SetAlias(Keys.NumPad8, "Num 8");
            SetAlias(Keys.NumPad9, "Num 9");
            SetAlias(Keys.OemClear, "Clear");
            SetAlias(Keys.Oem6, "]");
            SetAlias(Keys.Oemcomma, ",");
            SetAlias(Keys.OemMinus, "-");
            SetAlias(Keys.Oem4, "[");
            SetAlias(Keys.OemPeriod, ".");
            SetAlias(Keys.Oem5, @"\");
            SetAlias(Keys.Oem102, @"\");
            SetAlias(Keys.Oemplus, "+");
            SetAlias(Keys.Oem2, "?");
            SetAlias(Keys.Oem7, "'");
            SetAlias(Keys.Oem1, ";");
            SetAlias(Keys.Oem3, "~");
            SetAlias(Keys.Next, "Page Down");
            SetAlias(Keys.Next, "Page Down");
            SetAlias(Keys.PageUp, "Page Up");
            SetAlias(Keys.PageUp, "Page Up");
            SetAlias(Keys.PrintScreen, "Print Screen");
            SetAlias(Keys.Scroll, "Scroll Lock");
            SetAlias(Keys.ShiftKey, "Shift");
            SetAlias(Keys.Subtract, "Num -");
        }

        public void NotifyParent()
        {
            GMacroEditorForm parent = base.m_Parent.Parent as GMacroEditorForm;
            if (parent != null)
            {
                parent.UpdateKeyboard();
            }
        }

        private static void SetAlias(Keys key, string alias)
        {
            m_Aliases[(int) key] = alias;
        }

        private void Shift_OnClick(Gump g)
        {
            this.m_Macro.Shift = !this.m_Macro.Shift;
            this.UpdateModifier(this.m_Shift, "Shift", this.m_Macro.Shift);
            this.NotifyParent();
        }

        private void UpdateModifier(GSystemButton btn, string prefix, bool opt)
        {
            if (opt)
            {
                btn.SetBackColor(GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.5f));
            }
            else
            {
                btn.SetBackColor(GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.25f));
                btn.InactiveColor = GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f);
            }
        }

        public void UpdateModifiers()
        {
            this.UpdateModifier(this.m_Ctrl, "Ctrl", this.m_Macro.Control);
            this.UpdateModifier(this.m_Alt, "Alt", this.m_Macro.Alt);
            this.UpdateModifier(this.m_Shift, "Shift", this.m_Macro.Shift);
        }

        public Client.Macro Macro
        {
            get
            {
                return this.m_Macro;
            }
        }
    }
}

