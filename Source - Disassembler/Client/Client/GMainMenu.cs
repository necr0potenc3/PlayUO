namespace Client
{
    using System;

    public class GMainMenu : Gump
    {
        private bool m_LeftToRight;

        public GMainMenu(int x, int y) : base(x, y)
        {
            base.m_NonRestrictivePicking = true;
        }

        public void Add(GMenuItem child)
        {
            base.m_Children.Remove(child);
            if (this.m_LeftToRight)
            {
                child.X = base.m_Children.Count * 0x77;
                child.Y = 0;
            }
            else
            {
                child.X = 0;
                child.Y = base.m_Children.Count * 0x17;
            }
            base.m_Children.Add(child);
        }

        public bool Contains(GMenuItem child)
        {
            return (base.m_Children.IndexOf(child) >= 0);
        }

        public void Layout()
        {
            int num = 0;
            Gump[] gumpArray = base.m_Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                GMenuItem item = gumpArray[i] as GMenuItem;
                if (item != null)
                {
                    if (this.m_LeftToRight)
                    {
                        item.X = num++ * 0x77;
                        item.Y = 0;
                    }
                    else
                    {
                        item.X = 0;
                        item.Y = num++ * 0x17;
                    }
                }
            }
        }

        public void Remove(GMenuItem child)
        {
            base.m_Children.Remove(child);
            this.Layout();
        }

        public override int Height
        {
            get
            {
                return (this.m_LeftToRight ? 0x18 : (1 + (base.m_Children.Count * 0x77)));
            }
            set
            {
            }
        }

        public bool LeftToRight
        {
            get
            {
                return this.m_LeftToRight;
            }
            set
            {
                this.m_LeftToRight = value;
                this.Layout();
            }
        }

        public override int Width
        {
            get
            {
                return (this.m_LeftToRight ? (1 + (base.m_Children.Count * 0x17)) : 120);
            }
            set
            {
            }
        }
    }
}

