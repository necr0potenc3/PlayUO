namespace Client
{
    using Client.Prompts;
    using System.Windows.Forms;

    public class GQuickHues : GAlphaBackground
    {
        protected GBrightnessBar m_Brightness;
        protected bool m_Compacted;
        protected int m_CompactHeight;
        protected bool m_Compacting;
        protected bool m_Expanded;
        protected int m_ExpandedHeight;
        protected bool m_Expanding;
        protected int m_From;
        protected int m_LastX;
        protected int m_LastY;
        protected GFlatButton m_Okay;
        protected GHuePicker m_Picker;
        protected TimeSync m_Sync;
        protected Client.Timer m_Timer;
        protected int m_To;

        public GQuickHues(GHuePicker Picker, GBrightnessBar Brightness, GFlatButton Okay) : base(3, 0x57, 0x76, 20)
        {
            this.m_CompactHeight = 20;
            this.m_ExpandedHeight = Engine.GameHeight / 2;
            this.m_Picker = Picker;
            this.m_Brightness = Brightness;
            this.m_Okay = Okay;
            base.m_CanDrag = false;
            this.m_Timer = new Client.Timer(new OnTick(this.Roll_OnTick), 0);
            GLabel toAdd = new GLabel("Quick Hues", Engine.GetUniFont(0), Hues.Default, 2, 2);
            this.Height = 20;
            this.m_CompactHeight = base.m_Height;
            base.m_Children.Add(toAdd);
            toAdd.Center();
            QuickHues.Load();
            int count = QuickHues.Entries.Count;
            int num2 = 0x16;
            Clipper clipper = new Clipper(base.m_X + 1, base.m_Y + 1, base.m_Width - 2, base.m_Height - 2);
            GTextButton button = new GTextButton("Create new..", Engine.GetUniFont(0), Hues.Default, Hues.Load(0x35), 2, 2, new OnClick(this.Add_OnClick));
            base.m_Children.Add(button);
            button.Center();
            button.Y = num2;
            button.Scissor(clipper);
            num2 += button.Height;
            OnClick onClick = new OnClick(this.Entry_OnClick);
            OnHighlight highlight = new OnHighlight(this.Entry_OnHighlight);
            for (int i = 0; i < count; i++)
            {
                QuickHueEntry entry = (QuickHueEntry)QuickHues.Entries[i];
                GTextButton button2 = new GTextButton(entry.Name, Engine.GetUniFont(0), Hues.Load(0x58), Hues.Load(0x35), 2, 2, onClick);
                base.m_Children.Add(button2);
                button2.Center();
                button2.Y = num2;
                button2.Scissor(clipper);
                button2.SetTag("HueID", entry.Hue);
                button2.SetTag("Index", i);
                num2 += button2.Height;
                button2.OnHighlight = highlight;
                button2.Tooltip = new Tooltip(string.Format("0x{0:X}", entry.Hue));
            }
            num2 += 2;
            this.m_ExpandedHeight = num2;
        }

        private void Add_OnClick(Gump g)
        {
            Engine.AddTextMessage("Enter hue name:");
            Engine.Prompt = new HuePrompt(this.m_Picker.Hue, this);
        }

        protected internal override void Draw(int X, int Y)
        {
            this.m_LastX = X;
            if (Y != this.m_LastY)
            {
                this.m_LastY = Y;
                Clipper clipper = new Clipper(this.m_LastX + 1, this.m_LastY + 1, base.m_Width - 2, base.m_Height - 2);
                Gump[] gumpArray = base.m_Children.ToArray();
                for (int i = 1; i < gumpArray.Length; i++)
                {
                    ((GTextButton)gumpArray[i]).Scissor(clipper);
                }
            }
            if ((Gumps.LastOver != null) && Gumps.LastOver.IsChildOf(this))
            {
                if (!this.m_Expanded && !this.m_Expanding)
                {
                    this.m_From = this.Height;
                    this.m_To = this.m_ExpandedHeight;
                    this.m_Sync = new TimeSync(0.10000000149011612);
                    this.m_Timer.Start(false);
                    this.m_Expanded = false;
                    this.m_Expanding = true;
                    this.m_Compacted = false;
                    this.m_Compacting = false;
                }
            }
            else if (!this.m_Compacted && !this.m_Compacting)
            {
                this.m_From = this.Height;
                this.m_To = this.m_CompactHeight;
                this.m_Sync = new TimeSync(0.10000000149011612);
                this.m_Timer.Start(false);
                this.m_Expanded = false;
                this.m_Expanding = false;
                this.m_Compacted = false;
                this.m_Compacting = true;
            }
            Renderer.SetTexture(null);
            Renderer.SetAlphaEnable(true);
            Renderer.AlphaTestEnable = false;
            Renderer.SetAlpha(0.5f);
            Renderer.DrawLine(X + 20, (Y + this.m_CompactHeight) - 1, (X + base.m_Width) - 20, (Y + this.m_CompactHeight) - 1);
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = true;
            base.Draw(X, Y);
        }

        private void Entry_OnClick(Gump g)
        {
            if ((g.HasTag("HueID") && g.HasTag("Buttons")) && g.HasTag("Index"))
            {
                int tag = (int)g.GetTag("HueID");
                MouseButtons buttons = (MouseButtons)g.GetTag("Buttons");
                int num2 = (int)g.GetTag("Index");
                if ((buttons & MouseButtons.Right) != MouseButtons.None)
                {
                    QuickHues.Remove(num2);
                    Gumps.Destroy(this);
                    GQuickHues toAdd = new GQuickHues(this.m_Picker, this.m_Brightness, this.m_Okay);
                    if (this.m_Expanded)
                    {
                        toAdd.m_Expanded = true;
                        toAdd.Height = toAdd.m_ExpandedHeight;
                    }
                    base.m_Parent.Children.Add(toAdd);
                }
                else if ((tag >= 2) && (tag < 0x3ea))
                {
                    tag -= 2;
                    int num3 = tag % 5;
                    tag /= 5;
                    int num4 = tag % 20;
                    tag /= 20;
                    int num5 = tag;
                    this.m_Picker.Brightness = num3;
                    this.m_Picker.ShadeX = num4;
                    this.m_Picker.ShadeY = num5;
                    this.m_Brightness.Refresh();
                    this.m_Okay.Click();
                }
            }
        }

        private void Entry_OnHighlight(Gump g)
        {
            if (g.HasTag("HueID"))
            {
                int tag = (int)g.GetTag("HueID");
                if ((tag >= 2) && (tag < 0x3ea))
                {
                    tag -= 2;
                    int num2 = tag % 5;
                    tag /= 5;
                    int num3 = tag % 20;
                    tag /= 20;
                    int num4 = tag;
                    this.m_Picker.Brightness = num2;
                    this.m_Picker.ShadeX = num3;
                    this.m_Picker.ShadeY = num4;
                    this.m_Brightness.Refresh();
                }
            }
        }

        protected void Roll_OnTick(Client.Timer t)
        {
            double normalized = this.m_Sync.Normalized;
            this.Height = this.m_From + ((int)((this.m_To - this.m_From) * normalized));
            Engine.Redraw();
            if (normalized >= 1.0)
            {
                this.m_Timer.Stop();
                if (this.m_Expanding)
                {
                    this.m_Expanded = true;
                    this.m_Expanding = false;
                    this.m_Compacted = false;
                    this.m_Compacting = false;
                }
                else if (this.m_Compacting)
                {
                    this.m_Expanded = false;
                    this.m_Expanding = false;
                    this.m_Compacted = true;
                    this.m_Compacting = false;
                }
                else
                {
                    this.m_Expanded = false;
                    this.m_Expanding = false;
                    this.m_Compacted = false;
                    this.m_Compacting = false;
                }
            }
        }

        public override int Height
        {
            get
            {
                return base.m_Height;
            }
            set
            {
                base.m_Height = value;
                Clipper clipper = new Clipper(this.m_LastX + 1, this.m_LastY + 1, base.m_Width - 2, base.m_Height - 2);
                Gump[] gumpArray = base.m_Children.ToArray();
                for (int i = 1; i < gumpArray.Length; i++)
                {
                    ((GTextButton)gumpArray[i]).Scissor(clipper);
                }
            }
        }

        private class HuePrompt : IPrompt
        {
            private int m_Hue;
            private GQuickHues m_Target;

            public HuePrompt(int Hue, GQuickHues Target)
            {
                this.m_Hue = Hue;
                this.m_Target = Target;
            }

            public void OnCancel(PromptCancelType type)
            {
                if (type == PromptCancelType.UserCancel)
                {
                    Engine.AddTextMessage("Hue creation canceled.");
                }
            }

            public void OnReturn(string message)
            {
                QuickHueEntry e = new QuickHueEntry();
                e.Name = message;
                e.Hue = this.m_Hue;
                QuickHues.Add(e);
                Gumps.Destroy(this.m_Target);
                GQuickHues toAdd = new GQuickHues(this.m_Target.m_Picker, this.m_Target.m_Brightness, this.m_Target.m_Okay);
                if (this.m_Target.m_Expanded)
                {
                    toAdd.m_Expanded = true;
                    toAdd.Height = toAdd.m_ExpandedHeight;
                }
                this.m_Target.m_Parent.Children.Add(toAdd);
                Engine.AddTextMessage(string.Format("Hue created under name '{0}'.", message));
            }
        }
    }
}