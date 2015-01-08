namespace Client
{
    using Client.Prompts;
    using Client.Targeting;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Display : Form
    {
        private Container components = null;

        public Display()
        {
            this.InitializeComponent();
            base.KeyPress += new KeyPressEventHandler(this.Display_KeyPress);
            base.MouseDown += new MouseEventHandler(Engine.MouseDown);
            base.MouseMove += new MouseEventHandler(Engine.MouseMove);
            base.MouseUp += new MouseEventHandler(Engine.MouseUp);
            base.MouseWheel += new MouseEventHandler(Engine.MouseWheel);
            this.Cursor.Dispose();
        }

        public void Display_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Engine.m_EventOk)
            {
                if (Gumps.KeyDown(e.KeyChar))
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                    if (e.KeyChar == '\x001b')
                    {
                        if (Engine.TargetHandler != null)
                        {
                            Engine.TargetHandler.OnCancel(TargetCancelType.UserCancel);
                            Engine.TargetHandler = null;
                            return;
                        }
                        if (Engine.Prompt != null)
                        {
                            Engine.Prompt.OnCancel(PromptCancelType.UserCancel);
                            Engine.Prompt = null;
                            return;
                        }
                    }
                    if (!Engine.m_Locked)
                    {
                        if (e.KeyChar == '\b')
                        {
                            if (Engine.m_Text.Length > 0)
                            {
                                Engine.m_Text = Engine.m_Text.Substring(0, Engine.m_Text.Length - 1);
                                Renderer.SetText(Engine.m_Text);
                            }
                        }
                        else if (e.KeyChar == '\r')
                        {
                            Engine.commandEntered(Engine.Encode(Engine.m_Text));
                            Engine.m_Text = "";
                            Renderer.SetText("");
                        }
                        else if (e.KeyChar < ' ')
                        {
                            e.Handled = false;
                            e.Handled = true;
                        }
                        else
                        {
                            int num;
                            string input = Engine.m_Text + e.KeyChar;
                            string text = Engine.Encode(input) + "_";
                            Mobile player = World.Player;
                            if (((player != null) && player.OpenedStatus) && (player.StatusBar == null))
                            {
                                num = Engine.GameWidth - 0x2e;
                            }
                            else
                            {
                                num = Engine.GameWidth - 4;
                            }
                            if (Engine.GetUniFont(3).GetStringWidth(text) < num)
                            {
                                Engine.m_Text = input;
                                Renderer.SetText(input);
                            }
                        }
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.AutoScaleBaseSize = new Size(5, 13);
            this.BackColor = Color.Black;
            base.ClientSize = new Size(640, 480);
            this.ForeColor = SystemColors.ControlText;
            base.Name = "Display";
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ultima Online";
        }

        protected override void OnClick(EventArgs e)
        {
            if (Engine.m_EventOk)
            {
                Engine.ClickMessage(this, e);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Engine.exiting = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Engine.exiting = true;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (Engine.m_EventOk)
            {
                Engine.DoubleClick(this, e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((Engine.m_EventOk && (e.KeyCode != Keys.ShiftKey)) && ((e.KeyCode != Keys.ControlKey) && (e.KeyCode != Keys.Menu)))
            {
                GMacroKeyEntry focus = Gumps.Focus as GMacroKeyEntry;
                if (focus != null)
                {
                    focus.Start();
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs key)
        {
            if (Engine.m_EventOk)
            {
                Engine.KeyUp(key);
                GMacroKeyEntry focus = Gumps.Focus as GMacroKeyEntry;
                if (focus != null)
                {
                    focus.Finish(key.KeyCode, key.Modifiers);
                }
            }
        }

        protected override void OnResize(EventArgs ea)
        {
            if ((Engine.m_EventOk && !Engine.m_Fullscreen) && (base.WindowState != FormWindowState.Minimized))
            {
                base.OnResize(ea);
                Engine.ScreenWidth = base.ClientSize.Width;
                Engine.ScreenHeight = base.ClientSize.Height;
                GC.Collect();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if ((Engine.m_EventOk && !Engine.m_Fullscreen) && (base.WindowState != FormWindowState.Minimized))
            {
                base.OnSizeChanged(e);
            }
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            GumpColors.Invalidate();
            GumpHues.Invalidate();
            GumpPaint.Invalidate();
        }

        protected override bool ProcessDialogKey(Keys key)
        {
            if (!Engine.m_EventOk)
            {
                return false;
            }
            if (Gumps.Focus is GMacroKeyEntry)
            {
                return false;
            }
            KeyEventArgs e = new KeyEventArgs(key);
            Engine.KeyDown(this, e);
            return e.Handled;
        }
    }
}

