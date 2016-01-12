namespace Client
{
    public class GDesktopBorder : GBackground
    {
        public static GDesktopBorder Instance;

        public GDesktopBorder() : base(Engine.GameX - 4, Engine.GameY - 4, Engine.GameWidth + 8, Engine.GameHeight + 8, 0xa8c, 0xa8c, 0xa8c, 0xa8d, 0, 0xa8d, 0xa8c, 0xa8c, 0xa8c)
        {
            Instance = this;
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            base.m_DragClipX = Engine.GameWidth + 4;
            base.m_DragClipY = Engine.GameHeight + 4;
            base.CanClose = false;
        }

        public void DoRender()
        {
            Engine.GameX = this.X + 4;
            Engine.GameY = this.Y + 4;
            base.Render((Engine.GameX - 4) - this.X, (Engine.GameY - 4) - this.Y);
        }

        protected internal override bool HitTest(int X, int Y)
        {
            if (Engine.amMoving)
            {
                return false;
            }
            return ((((X < 4) || (Y < 4)) || (X >= (Engine.GameWidth + 4))) || (Y >= (Engine.GameHeight + 4)));
        }

        protected internal override void Render(int X, int Y)
        {
        }
    }
}