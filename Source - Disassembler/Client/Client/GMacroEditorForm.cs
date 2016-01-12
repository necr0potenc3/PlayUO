namespace Client
{
    using Microsoft.Win32;
    using System.Drawing;

    public class GMacroEditorForm : GWindowsForm
    {
        private Macro m_Current;
        private static GMacroEditorForm m_Instance;
        private GMacroKeyboard m_Keyboard;
        private GSystemButton m_KeyboardFlipper;
        private GLabel m_NoSel;
        private GMacroEditorPanel m_Panel;
        private GAlphaBackground m_Sunken;

        public GMacroEditorForm() : base(0, 0, 0x10d, 0x11b)
        {
            Gumps.Focus = this;
            base.m_NonRestrictivePicking = true;
            base.Client.m_NonRestrictivePicking = true;
            base.Text = "Macro Editor";
            GAlphaBackground toAdd = this.m_Sunken = new GAlphaBackground(1, 2, 0x103, 230);
            toAdd.ShouldHitTest = false;
            toAdd.FillAlpha = 1f;
            toAdd.FillColor = GumpColors.AppWorkspace;
            toAdd.DrawBorder = false;
            base.Client.Children.Add(toAdd);
            this.m_KeyboardFlipper = new GSystemButton(0x47, 0xec, 120, 20, SystemColors.Control, SystemColors.ControlText, "Show Keyboard", Engine.GetUniFont(2));
            this.m_KeyboardFlipper.OnClick = new OnClick(this.KeyboardFlipper_OnClick);
            base.Client.Children.Add(this.m_KeyboardFlipper);
            GSystemButton button = new GSystemButton(240, 0xec, 20, 20, SystemColors.Control, SystemColors.ControlText, "→", Engine.GetUniFont(2));
            button.Tooltip = new Tooltip("Advance to the next macro", true);
            button.OnClick = new OnClick(this.Next_OnClick);
            base.Client.Children.Add(button);
            GSystemButton button2 = new GSystemButton(1, 0xec, 20, 20, SystemColors.Control, SystemColors.ControlText, "←", Engine.GetUniFont(2));
            button2.Tooltip = new Tooltip("Go back to the previous macro", true);
            button2.OnClick = new OnClick(this.Prev_OnClick);
            base.Client.Children.Add(button2);
            this.Center();
            this.Y -= 0x5c;
            if (Macros.List.Count > 0)
            {
                this.Current = (Macro)Macros.List[0];
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            base.Draw(X, Y);
            Renderer.SetTexture(null);
            GumpPaint.DrawSunken3D(((X + base.Client.X) + this.m_Sunken.X) - 1, ((Y + base.Client.Y) + this.m_Sunken.Y) - 1, this.m_Sunken.Width + 2, this.m_Sunken.Height + 2);
        }

        private void KeyboardFlipper_OnClick(Gump g)
        {
            this.ShowKeyboard = !this.ShowKeyboard;
        }

        private void Next_OnClick(Gump g)
        {
            if (Macros.List.Count != 0)
            {
                int num = (Macros.List.IndexOf(this.m_Current) + 1) % Macros.List.Count;
                if ((num >= 0) && (num < Macros.List.Count))
                {
                    this.Current = (Macro)Macros.List[num];
                }
            }
        }

        protected internal override void OnDispose()
        {
            m_Instance = null;
            Macros.Save();
            base.OnDispose();
        }

        public static void Open()
        {
            if (m_Instance == null)
            {
                m_Instance = new GMacroEditorForm();
                Gumps.Desktop.Children.Add(m_Instance);
                Gumps.Focus = m_Instance;
            }
        }

        private void Prev_OnClick(Gump g)
        {
            if (Macros.List.Count != 0)
            {
                int num = (Macros.List.IndexOf(this.m_Current) - 1) % Macros.List.Count;
                if (num < 0)
                {
                    num += Macros.List.Count;
                }
                if ((num >= 0) && (num < Macros.List.Count))
                {
                    this.Current = (Macro)Macros.List[num];
                }
            }
        }

        private static System.Drawing.Color ReadRegistryColor(string name)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Colors", false))
                {
                    string[] strArray = (key.GetValue(name) as string).Split(new char[] { ' ' });
                    return System.Drawing.Color.FromArgb(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]));
                }
            }
            catch
            {
            }
            return System.Drawing.Color.White;
        }

        public void UpdateKeyboard()
        {
            if (this.m_Keyboard != null)
            {
                this.m_Keyboard.Update();
            }
        }

        public Macro Current
        {
            get
            {
                return this.m_Current;
            }
            set
            {
                bool flag = ((this.m_Current != null) && (this.m_Current != value)) && (this.m_Current.Actions.Length == 0);
                if (flag)
                {
                    Macros.List.Remove(this.m_Current);
                }
                if (this.m_Panel != null)
                {
                    Gumps.Destroy(this.m_Panel);
                }
                this.m_Panel = null;
                this.m_Current = value;
                if (this.m_Current != null)
                {
                    this.m_Panel = new GMacroEditorPanel(this.m_Current);
                    this.m_Panel.X = 1;
                    this.m_Panel.Y = 2;
                    base.Client.Children.Add(this.m_Panel);
                }
                if ((this.m_Current != null) && (this.m_NoSel != null))
                {
                    Gumps.Destroy(this.m_NoSel);
                    this.m_NoSel = null;
                }
                else if ((this.m_Current == null) && (this.m_NoSel == null))
                {
                    this.m_NoSel = new GLabel("No macro is currently selected", Engine.GetUniFont(1), Hues.Load(0x481), 0x10, 0x12);
                    base.Client.Children.Add(this.m_NoSel);
                }
                if (flag)
                {
                    this.UpdateKeyboard();
                }
            }
        }

        public static bool IsOpen
        {
            get
            {
                return (m_Instance != null);
            }
        }

        public GMacroKeyboard Keyboard
        {
            get
            {
                return this.m_Keyboard;
            }
        }

        public bool ShowKeyboard
        {
            get
            {
                return (this.m_Keyboard != null);
            }
            set
            {
                if (value)
                {
                    if (this.m_Keyboard == null)
                    {
                        this.m_KeyboardFlipper.Text = "Hide Keyboard";
                        this.m_Keyboard = new GMacroKeyboard();
                        base.m_Children.Insert(0, this.m_Keyboard);
                        this.m_Keyboard.Center();
                        this.m_Keyboard.Y = this.Height - 1;
                    }
                }
                else if (this.m_Keyboard != null)
                {
                    this.m_KeyboardFlipper.Text = "Show Keyboard";
                    Gumps.Destroy(this.m_Keyboard);
                    this.m_Keyboard = null;
                }
            }
        }
    }
}