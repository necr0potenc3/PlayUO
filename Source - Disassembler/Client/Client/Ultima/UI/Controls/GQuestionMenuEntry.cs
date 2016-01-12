namespace Client
{
    public class GQuestionMenuEntry : Gump
    {
        private AnswerEntry m_Answer;
        private int m_Height;
        private GLabel m_Label;
        private GRadioButton m_Radio;
        private int m_Width;
        private int m_yBase;

        public GQuestionMenuEntry(int x, int y, int xWidth, AnswerEntry a) : base(x, y)
        {
            this.m_yBase = y;
            this.m_Answer = a;
            this.m_Radio = new GRadioButton(210, 0xd3, false, 0, 0);
            this.m_Label = new GWrappedLabel(a.Text, Engine.GetFont(1), Hues.Load(0x455), this.m_Radio.Width + 4, 5, (xWidth - this.m_Radio.Width) - 4);
            this.m_Width = xWidth;
            this.m_Height = this.m_Radio.Height;
            if ((this.m_Label.Y + this.m_Label.Height) > this.m_Height)
            {
                this.m_Height = this.m_Label.Y + this.m_Label.Height;
            }
            base.m_Children.Add(this.m_Radio);
            base.m_Children.Add(this.m_Label);
        }

        public AnswerEntry Answer
        {
            get
            {
                return this.m_Answer;
            }
        }

        public Client.Clipper Clipper
        {
            set
            {
                this.m_Radio.Clipper = value;
                this.m_Label.Clipper = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public GRadioButton Radio
        {
            get
            {
                return this.m_Radio;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }

        public int yBase
        {
            get
            {
                return this.m_yBase;
            }
        }
    }
}