namespace Client
{
    using System;

    public class GItemCounter : GAlphaBackground
    {
        private GItemArt m_Image;
        private GLabel m_Label;
        private int m_LastAmount;
        private IItemValidator m_Validator;

        public GItemCounter(ItemIDValidator v) : base(0, 0, 100, 20)
        {
            this.m_Validator = v;
            this.m_Image = new GItemArt(3, 3, v.List[0]);
            this.m_Label = new GLabel("", Engine.DefaultFont, Hues.Bright, 0, 0);
            this.m_Image.X -= this.m_Image.Image.xMin;
            this.m_Image.Y -= this.m_Image.Image.yMin;
            base.m_Children.Add(this.m_Image);
            base.m_Children.Add(this.m_Label);
            this.m_LastAmount = -2147483648;
        }

        private void Size()
        {
            int num = (this.m_Label.Image.xMax - this.m_Label.Image.xMin) + 1;
            int num2 = (this.m_Image.Image.yMax - this.m_Image.Image.yMin) + 1;
            int num3 = (this.m_Label.Image.yMax - this.m_Label.Image.yMin) + 1;
            if (num3 > num2)
            {
                num2 = num3;
            }
            this.Height = num2 + 6;
            this.Width = num + 0x25;
            this.m_Label.X = 0x20 - this.m_Label.Image.xMin;
            this.m_Label.Y = ((this.Height - num3) / 2) - this.m_Label.Image.yMin;
            this.m_Image.Y = ((this.Height - ((this.m_Image.Image.yMax - this.m_Image.Image.yMin) + 1)) / 2) - this.m_Image.Image.yMin;
        }

        public void Update(Item pack)
        {
            Item[] itemArray = pack.FindItems(this.m_Validator);
            int num = 0;
            for (int i = 0; i < itemArray.Length; i++)
            {
                num += Math.Max((ushort)itemArray[i].Amount, (ushort)1);
            }
            if (this.m_LastAmount != num)
            {
                this.m_LastAmount = num;
                this.m_Label.Text = num.ToString();
                this.m_Label.Hue = (num < 5) ? Hues.Load(0x22) : Hues.Bright;
                this.Size();
            }
        }
    }
}