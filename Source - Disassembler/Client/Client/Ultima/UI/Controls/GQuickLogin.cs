namespace Client
{
    public class GQuickLogin : GAlphaBackground
    {
        protected bool m_Compacted;
        protected int m_CompactHeight;
        protected bool m_Compacting;
        protected bool m_Expanded;
        protected int m_ExpandedHeight;
        protected bool m_Expanding;
        protected int m_From;
        protected int m_LastX;
        protected int m_LastY;
        protected TimeSync m_Sync;
        protected Timer m_Timer;
        protected int m_To;

        public GQuickLogin() : base(0, 0, 200, 20)
        {
            this.m_CompactHeight = 20;
            this.m_ExpandedHeight = Engine.GameHeight / 2;
            base.m_CanDrag = false;
            this.m_Timer = new Timer(new OnTick(this.Roll_OnTick), 0);
            GLabel toAdd = new GLabel("Quick Login", Engine.GetUniFont(0), Hues.Default, 2, 2);
            this.Height = toAdd.Height + 4;
            this.m_CompactHeight = base.m_Height;
            base.m_Children.Add(toAdd);
            toAdd.Center();
            QuickLogin.Load();
            int count = QuickLogin.Entries.Count;
            if (count == 0)
            {
                base.Visible = false;
            }
            else
            {
                if (count > 12)
                {
                    count = 12;
                }
                int num2 = (2 + toAdd.Height) + 4;
                if (count == 0)
                {
                    num2 -= 2;
                }
                OnClick onClick = new OnClick(Engine.QuickLogin_OnClick);
                Clipper clipper = new Clipper(base.m_X + 1, base.m_Y + 1, base.m_Width - 2, base.m_Height - 2);
                for (int i = 0; i < count; i++)
                {
                    Entry entry = (Entry)QuickLogin.Entries[i];
                    GTextButton button = new GTextButton(entry.CharName, Engine.GetUniFont(0), Hues.Load(0x58), Hues.Load(0x35), 2, 2, onClick);
                    base.m_Children.Add(button);
                    button.Center();
                    button.Y = num2;
                    button.Scissor(clipper);
                    button.SetTag("Index", i);
                    num2 += button.Height;
                    int num4 = 0;
                    bool flag = false;
                    for (int j = 0; j < count; j++)
                    {
                        Entry entry2 = (Entry)QuickLogin.Entries[j];
                        if (entry2.CharName == entry.CharName)
                        {
                            if (j <= i)
                            {
                                num4++;
                            }
                            if (j != i)
                            {
                                flag = true;
                            }
                        }
                    }
                    if (flag)
                    {
                        button.Tooltip = new Tooltip(string.Format("{0}\n{1}", num4, entry.ServerName));
                    }
                    else
                    {
                        button.Tooltip = new Tooltip(entry.ServerName);
                    }
                }
                if (count != 0)
                {
                    num2 += 2;
                }
                if (num2 > 480)
                {
                    num2 = 480;
                }
                this.m_ExpandedHeight = num2;
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            this.m_LastX = X;
            if (Y != this.m_LastY)
            {
                this.m_LastY = Y;
                Clipper clipper = new Clipper(this.m_LastX + 1, this.m_LastY + 1, base.m_Width - 2, base.m_Height - 2);
                for (int i = 1; i < base.m_Children.Count; i++)
                {
                    ((GTextButton)base.m_Children[i]).Scissor(clipper);
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
            Renderer.SetAlpha(0.5f);
            Renderer.AlphaTestEnable = false;
            Renderer.DrawLine(X + 20, (Y + this.m_CompactHeight) - 1, (X + base.m_Width) - 20, (Y + this.m_CompactHeight) - 1);
            Renderer.AlphaTestEnable = true;
            Renderer.SetAlphaEnable(false);
            base.Draw(X, Y);
        }

        protected void Roll_OnTick(Timer t)
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
    }
}