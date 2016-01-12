namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GPacketStats : GAlphaBackground
    {
        private OnPacketHandle m_Event;
        private static GLabel[] m_Labels;
        private GAlphaVSlider m_Slider;
        private static GLabel[] m_Values;
        private int m_xLast;
        private int m_yLast;

        public GPacketStats() : base(3, 3, 0xc2, 0xc2)
        {
            this.m_xLast = -12345;
            this.m_yLast = -12345;
            m_Labels = new GLabel[0x100];
            m_Values = new GLabel[0x100];
            Client.IFont uniFont = Engine.GetUniFont(1);
            IHue bright = Hues.Bright;
            int y = 3;
            for (int i = 0; i < 0x100; i++)
            {
                if (PacketHandlers.m_Handlers[i] != null)
                {
                    PacketHandler handler = PacketHandlers.m_Handlers[i];
                    m_Labels[i] = new GLabel(handler.Name, uniFont, bright, 3, y);
                    m_Labels[i].SetTag("BaseY", y);
                    base.m_Children.Add(m_Labels[i]);
                    m_Values[i] = new GLabel(handler.Count.ToString(), uniFont, bright, 3, y);
                    m_Values[i].SetTag("BaseY", y);
                    m_Values[i].X = 0xaf - m_Values[i].Width;
                    base.m_Children.Add(m_Values[i]);
                    y += m_Labels[i].Height + 3;
                }
            }
            this.m_Slider = new GAlphaVSlider(0xb3, 6, 0x10, 0xb7, 0.0, 0.0, (double)(y - 0xc0), 1.0);
            this.m_Slider.OnValueChange = (OnValueChange)Delegate.Combine(this.m_Slider.OnValueChange, new OnValueChange(this.Slider_OnValueChange));
            this.m_Slider.ScrollOffset = 15.0;
            base.m_Children.Add(this.m_Slider);
            GHotspot toAdd = new GHotspot(0xb2, 1, 0x10, 0xc0, this.m_Slider);
            toAdd.NormalHit = false;
            base.m_Children.Add(toAdd);
            this.m_Event = new OnPacketHandle(this.OnRecv);
            Network.OnPacketHandle = (OnPacketHandle)Delegate.Combine(Network.OnPacketHandle, this.m_Event);
        }

        protected internal override void Draw(int X, int Y)
        {
            if ((X != this.m_xLast) || (Y != this.m_yLast))
            {
                this.m_xLast = X;
                this.m_yLast = Y;
                Clipper clipper = new Clipper(X + 1, Y + 1, base.m_Width - 0x10, base.m_Height - 2);
                for (int i = 0; i < 0x100; i++)
                {
                    if (m_Labels[i] != null)
                    {
                        m_Labels[i].Scissor(clipper);
                    }
                    if (m_Values[i] != null)
                    {
                        m_Values[i].Scissor(clipper);
                    }
                }
            }
            base.Draw(X, Y);
        }

        protected internal override void OnDispose()
        {
            Network.OnPacketHandle = (OnPacketHandle)Delegate.Remove(Network.OnPacketHandle, this.m_Event);
        }

        protected internal override void OnDragStart()
        {
            base.m_IsDragging = false;
            Gumps.Drag = base.m_Parent;
            Gumps.StartDrag = base.m_Parent;
            base.m_Parent.m_IsDragging = true;
            base.m_Parent.m_OffsetX = base.m_OffsetX + base.m_X;
            base.m_Parent.m_OffsetY = base.m_OffsetY + base.m_Y;
            base.m_Parent.OnDragStart();
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            base.m_Parent.OnMouseDown(X + base.m_X, Y + base.m_Y, mb);
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            base.m_Parent.OnMouseEnter(X + base.m_X, Y + base.m_Y, mb);
        }

        protected internal override void OnMouseLeave()
        {
            base.m_Parent.OnMouseLeave();
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            base.m_Parent.OnMouseMove(X + base.m_X, Y + base.m_Y, mb);
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            base.m_Parent.OnMouseUp(X + base.m_X, Y + base.m_Y, mb);
        }

        protected internal override void OnMouseWheel(int Delta)
        {
            this.m_Slider.OnMouseWheel(Delta);
        }

        internal void OnRecv(PacketHandler ph)
        {
            if (m_Values[ph.PacketID] != null)
            {
                m_Values[ph.PacketID].Text = ph.Count.ToString();
                m_Values[ph.PacketID].X = 0xaf - m_Values[ph.PacketID].Width;
            }
        }

        private void Slider_OnValueChange(double vNew, double vOld, Gump who)
        {
            int num = (int)vNew;
            for (int i = 0; i < 0x100; i++)
            {
                if (m_Labels[i] != null)
                {
                    m_Labels[i].Y = ((int)m_Labels[i].GetTag("BaseY")) - num;
                }
                if (m_Values[i] != null)
                {
                    m_Values[i].Y = ((int)m_Values[i].GetTag("BaseY")) - num;
                }
            }
            this.m_xLast = -12345;
            this.m_yLast = -12345;
        }
    }
}