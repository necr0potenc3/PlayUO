namespace Client
{
    using System;

    public class StaticMessage : TextMessage
    {
        private int m_Serial;

        public StaticMessage(int xMouse, int yMouse, int Serial, string Message) : base(Message, Engine.ItemDuration, Engine.DefaultFont, Hues.Load(0x3b2))
        {
            this.m_Serial = Serial;
            base.m_X = xMouse - (base.m_Image.Width >> 1);
            base.m_Y = yMouse - base.m_Image.Height;
        }

        public void Offset(int X, int Y)
        {
            base.m_X -= X;
            base.m_Y -= Y;
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }

        public override int X
        {
            get
            {
                return (base.m_X - Renderer.m_xScroll);
            }
        }

        public override int Y
        {
            get
            {
                return (base.m_Y - Renderer.m_yScroll);
            }
        }
    }
}

