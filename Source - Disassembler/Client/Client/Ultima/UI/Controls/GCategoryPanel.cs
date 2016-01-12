namespace Client
{
    using System.Collections;

    public class GCategoryPanel : GAlphaBackground
    {
        private GPropertyEntry[] m_Entries;
        private GLabel m_Label;

        public GCategoryPanel(object obj, string category, ArrayList entries) : base(0, 0, 0x117, 0x16)
        {
            base.FillColor = GumpColors.Control;
            base.BorderColor = GumpColors.ControlDarkDark;
            base.FillAlpha = 1f;
            base.ShouldHitTest = false;
            base.m_NonRestrictivePicking = true;
            this.m_Label = new GLabel(category, Engine.GetUniFont(1), GumpHues.ControlText, 0, 0);
            this.m_Label.X = 5 - this.m_Label.Image.xMin;
            this.m_Label.Y = ((0x16 - ((this.m_Label.Image.yMax - this.m_Label.Image.yMin) + 1)) / 2) - this.m_Label.Image.yMin;
            base.m_Children.Add(this.m_Label);
            entries.Sort();
            this.m_Entries = new GPropertyEntry[entries.Count];
            int num = 0x15;
            for (int i = 0; i < entries.Count; i++)
            {
                this.m_Entries[i] = new GPropertyEntry(obj, (ObjectEditorEntry)entries[i]);
                this.m_Entries[i].Y = num;
                num += 0x15;
                base.m_Children.Add(this.m_Entries[i]);
            }
            this.Height = num + 1;
        }

        protected internal override void Draw(int X, int Y)
        {
            base.Draw(X, Y);
            Renderer.SetTexture(null);
            if (base.m_Clipper == null)
            {
                Renderer.SolidRect(GumpColors.ControlDark, X + 1, Y + 0x15, this.Width - 2, this.Height - 0x16);
            }
            else
            {
                Renderer.SolidRect(GumpColors.ControlDark, X + 1, Y + 0x15, this.Width - 2, this.Height - 0x16, base.m_Clipper);
            }
        }

        public void Reset()
        {
            for (int i = 0; i < this.m_Entries.Length; i++)
            {
                this.m_Entries[i].Reset();
            }
        }

        public void SetClipper(Clipper c)
        {
            base.Clipper = c;
            this.m_Label.Clipper = c;
            for (int i = 0; i < this.m_Entries.Length; i++)
            {
                this.m_Entries[i].SetClipper(c);
            }
        }

        public GPropertyEntry[] Entries
        {
            get
            {
                return this.m_Entries;
            }
            set
            {
                this.m_Entries = value;
            }
        }

        public string Label
        {
            get
            {
                return this.m_Label.Text;
            }
            set
            {
                this.m_Label.Text = value;
            }
        }
    }
}