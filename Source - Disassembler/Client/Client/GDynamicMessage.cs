namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GDynamicMessage : GTextButton, IMessage
    {
        private TimeSync m_Dispose;
        private System.Drawing.Rectangle m_ImageRect;
        private IMessageOwner m_Owner;
        private System.Drawing.Rectangle m_Rectangle;
        private float m_SolidDuration;
        private bool m_Unremovable;

        public GDynamicMessage(bool unremovable, Item i, string text, Client.IFont font, IHue hue) : this(unremovable, i, text, font, hue, Engine.ItemDuration)
        {
        }

        public GDynamicMessage(bool unremovable, Mobile m, string text, Client.IFont font, IHue hue) : this(unremovable, m, text, font, hue, Engine.MobileDuration)
        {
        }

        private GDynamicMessage(bool unremovable, IMessageOwner owner, string text, Client.IFont font, IHue hue, float duration) : base(text, font, hue, Hues.Load(0x35), 0, 0, null)
        {
            this.m_Unremovable = unremovable;
            base.m_OverridesCursor = false;
            this.m_Owner = owner;
            this.m_SolidDuration = duration;
            this.m_Dispose = new TimeSync((double)(this.m_SolidDuration + 1f));
        }

        public bool Visible { get { return base.Visible; } }

        protected internal override void Draw(int x, int y)
        {
            Gump[] array = Gumps.Desktop.Children.ToArray();
            float num = 1f;
            for (int i = Array.IndexOf(array, this) + 1; i < array.Length; i++)
            {
                Gump gump = array[i];
                if (gump is IMessage)
                {
                    IMessage message = (IMessage)gump;
                    if (message.Visible && message.ImageRect.IntersectsWith(this.m_ImageRect))
                    {
                        num += message.Alpha;
                    }
                }
            }
            float alpha = this.Alpha;
            this.Alpha = (float)((1.0 / ((double)num)) * alpha);
            base.Draw(x, y);
            this.Alpha = alpha;
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (base.m_Invalidated)
            {
                this.Refresh();
            }
            return (base.m_Draw && base.m_Image.HitTest(x, y));
        }

        public System.Drawing.Rectangle OnBeginRender()
        {
            double elapsed = this.m_Dispose.Elapsed;
            if (elapsed >= (this.m_SolidDuration + 1f))
            {
                base.Visible = false;
                MessageManager.Remove(this);
                return (this.m_ImageRect = this.m_Rectangle = System.Drawing.Rectangle.Empty);
            }
            if (elapsed >= this.m_SolidDuration)
            {
                this.Alpha = (float)(1.0 - (elapsed - this.m_SolidDuration));
            }
            if (this.m_Owner.MessageFrame == Renderer.m_ActFrames)
            {
                this.m_Owner.MessageY -= this.Height + 2;
                this.X = this.m_Owner.MessageX - (this.Width / 2);
                this.Y = this.m_Owner.MessageY;
                base.Visible = true;
            }
            else
            {
                base.Visible = false;
            }
            if ((this.m_Owner is Item) && !((Item)this.m_Owner).InWorld)
            {
                if (this.X < 2)
                {
                    this.X = 2;
                }
                else if ((this.X + this.Width) > (Engine.ScreenWidth - 2))
                {
                    this.X = (Engine.ScreenWidth - 2) - this.Width;
                }
                if (this.Y < 2)
                {
                    this.Y = 2;
                }
                else if ((this.Y + this.Height) > (Engine.ScreenHeight - 2))
                {
                    this.Y = (Engine.ScreenHeight - 2) - this.Height;
                }
            }
            else
            {
                if (this.X < (Engine.GameX + 2))
                {
                    this.X = Engine.GameX + 2;
                }
                else if ((this.X + this.Width) > ((Engine.GameX + Engine.GameWidth) - 2))
                {
                    this.X = ((Engine.GameX + Engine.GameWidth) - 2) - this.Width;
                }
                if (this.Y < (Engine.GameY + 2))
                {
                    this.Y = Engine.GameY + 2;
                }
                else if ((this.Y + this.Height) > ((Engine.GameY + Engine.GameHeight) - 2))
                {
                    this.Y = ((Engine.GameY + Engine.GameHeight) - 2) - this.Height;
                }
            }
            this.m_Rectangle.X = this.X;
            this.m_Rectangle.Y = this.Y;
            this.m_Rectangle.Width = this.Width;
            this.m_Rectangle.Height = this.Height;
            this.m_ImageRect.X = this.X + base.Image.xMin;
            this.m_ImageRect.Y = this.Y + base.Image.yMin;
            this.m_ImageRect.Width = (base.Image.xMax - base.Image.xMin) + 1;
            this.m_ImageRect.Height = (base.Image.yMax - base.Image.yMin) + 1;
            return this.m_Rectangle;
        }

        protected internal override void OnDoubleClick(int x, int y)
        {
            this.m_Owner.OnDoubleClick();
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Client.Point point = base.PointToScreen(new Client.Point(x, y));
                int distance = 0;
                Engine.movingDir = Engine.GetDirection(point.X, point.Y, ref distance);
                Engine.amMoving = true;
            }
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            base.OnMouseEnter(x, y, mb);
            base.BringToTop();
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (((mb & MouseButtons.Right) != MouseButtons.None) && Engine.amMoving)
            {
                Client.Point point = base.PointToScreen(new Client.Point(X, Y));
                int distance = 0;
                Engine.movingDir = Engine.GetDirection(point.X, point.Y, ref distance);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Engine.amMoving = false;
            }
        }

        protected internal override void OnSingleClick(int x, int y)
        {
            if (Gumps.LastOver == this)
            {
                if (Engine.TargetHandler == null)
                {
                    this.m_Owner.OnSingleClick();
                }
                else
                {
                    this.m_Owner.OnTarget();
                }
            }
        }

        public System.Drawing.Rectangle ImageRect
        {
            get
            {
                return this.m_ImageRect;
            }
        }

        public IMessageOwner Owner
        {
            get
            {
                return this.m_Owner;
            }
        }

        public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return this.m_Rectangle;
            }
        }

        public bool Unremovable
        {
            get
            {
                return this.m_Unremovable;
            }
        }
    }
}