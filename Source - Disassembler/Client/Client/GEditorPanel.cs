namespace Client
{
    using System;
    using System.Collections;

    public class GEditorPanel : GEmpty
    {
        private Clipper m_Clipper;
        private GCategoryPanel[] m_Panels;
        private GEditorScroller m_Scroller;
        private int m_xLast;
        private int m_yLast;

        public GEditorPanel(ArrayList panels, int height) : base(0, 0, 0, height)
        {
            this.m_Panels = (GCategoryPanel[]) panels.ToArray(typeof(GCategoryPanel));
            base.m_NonRestrictivePicking = true;
            this.Layout();
        }

        protected internal override void Draw(int X, int Y)
        {
            if ((this.m_xLast != X) || (this.m_yLast != Y))
            {
                this.m_xLast = X;
                this.m_yLast = Y;
                this.m_Clipper = new Clipper(this.m_xLast + 5, this.m_yLast, this.Width - 10, this.Height);
                for (int i = 0; i < this.m_Panels.Length; i++)
                {
                    this.m_Panels[i].SetClipper(this.m_Clipper);
                }
            }
            Renderer.SetTexture(null);
            GumpPaint.DrawSunken3D(X - 2, Y - 2, this.Width + 4, this.Height + 4);
        }

        public void Layout()
        {
            int num = 5;
            int num2 = 0;
            int width = 0;
            int num4 = 0;
            if (this.m_Scroller != null)
            {
                num2 = -this.m_Scroller.Value;
            }
            for (int i = 0; i < this.m_Panels.Length; i++)
            {
                GCategoryPanel toAdd = this.m_Panels[i];
                toAdd.X = 5;
                toAdd.Y = num + num2;
                if (this.m_Scroller == null)
                {
                    base.m_Children.Add(toAdd);
                }
                if (toAdd.Width > width)
                {
                    width = toAdd.Width;
                }
                if ((num + toAdd.Height) > num4)
                {
                    num4 = num + toAdd.Height;
                }
                num += toAdd.Height - 1;
            }
            width += 0x1a;
            this.Width = width;
            if (this.m_Scroller == null)
            {
                this.m_Scroller = new GEditorScroller(this);
                this.m_Scroller.X = width - 0x10;
                this.m_Scroller.Y = 0;
                this.m_Scroller.Height = this.Height;
                this.m_Scroller.Width = 0x10;
                this.m_Scroller.Maximum = (num4 - this.Height) + 5;
                base.m_Children.Insert(0, this.m_Scroller);
            }
        }

        public void Reset()
        {
            for (int i = 0; i < this.m_Panels.Length; i++)
            {
                this.m_Panels[i].Reset();
            }
        }

        public GCategoryPanel[] Panels
        {
            get
            {
                return this.m_Panels;
            }
        }
    }
}

