namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GSkinHuePickerAll : Gump
    {
        private int[,,] m_ColorTable;
        private Client.OnHueSelect m_OnHueRelease;
        private Client.OnHueSelect m_OnHueSelect;
        private int m_xShade;
        private int m_yShade;
        private int m_zShade;
        private const int xShades = 7;
        private int xSize;
        private const int yShades = 1;
        private int ySize;
        private const int zShades = 8;

        public GSkinHuePickerAll(int X, int Y, int Width, int Height) : base(X, Y)
        {
            do
            {
                this.xSize++;
            }
            while ((this.xSize * 7) <= Width);
            this.xSize--;
            do
            {
                this.ySize++;
            }
            while ((this.ySize * 8) <= Height);
            this.ySize--;
            this.m_ColorTable = new int[7, 1, 8];
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        ushort num4 = Hues.GetData((0x3e9 + (((j * 7) + i) * 8)) + k).colors[0x30];
                        int num5 = (num4 >> 10) & 0x1f;
                        int num6 = (num4 >> 5) & 0x1f;
                        int num7 = num4 & 0x1f;
                        num5 = (int) (num5 * 8.225806f);
                        num6 = (int) (num6 * 8.225806f);
                        num7 = (int) (num7 * 8.225806f);
                        int num8 = ((num5 << 0x10) | (num6 << 8)) | num7;
                        this.m_ColorTable[i, j, k] = num8;
                    }
                }
            }
        }

        private void ChangeShade(int X, int Y)
        {
            int xShade = this.m_xShade;
            int yShade = this.m_yShade;
            int zShade = this.m_zShade;
            this.m_xShade = X / this.xSize;
            this.m_yShade = Y / this.ySize;
            this.m_zShade = this.m_yShade / 1;
            this.m_yShade = this.m_yShade % 1;
            if (((this.m_xShade != xShade) || (this.m_yShade != yShade)) || (this.m_zShade != zShade))
            {
                if (this.m_OnHueSelect != null)
                {
                    this.m_OnHueSelect(this.Hue, this);
                }
                Engine.Redraw();
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            Renderer.AlphaTestEnable = false;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        Renderer.SolidRect(this.m_ColorTable[i, j, k], X + (i * this.xSize), Y + ((k + j) * this.ySize), this.xSize, this.ySize);
                    }
                }
            }
            Renderer.SolidRect(0x80ffff, (X + (this.m_xShade * this.xSize)) + ((this.xSize - 3) / 2), (Y + ((this.m_zShade + this.m_yShade) * this.ySize)) + ((this.ySize - 1) / 2), 3, 1);
            Renderer.SolidRect(0x80ffff, (X + (this.m_xShade * this.xSize)) + ((this.xSize - 1) / 2), (Y + ((this.m_zShade + this.m_yShade) * this.ySize)) + ((this.ySize - 3) / 2), 1, 3);
            Renderer.AlphaTestEnable = true;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return true;
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            this.ChangeShade(X, Y);
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (mb != MouseButtons.None)
            {
                this.ChangeShade(X, Y);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            this.ChangeShade(X, Y);
            if (this.m_OnHueRelease != null)
            {
                this.m_OnHueRelease(this.Hue, this);
            }
        }

        public override int Height
        {
            get
            {
                return (this.ySize * 8);
            }
        }

        public int Hue
        {
            get
            {
                return ((0x3ea + (((this.m_yShade * 7) + this.m_xShade) * 8)) + this.m_zShade);
            }
        }

        public Client.OnHueSelect OnHueRelease
        {
            get
            {
                return this.m_OnHueRelease;
            }
            set
            {
                this.m_OnHueRelease = value;
            }
        }

        public Client.OnHueSelect OnHueSelect
        {
            get
            {
                return this.m_OnHueSelect;
            }
            set
            {
                this.m_OnHueSelect = value;
            }
        }

        public override int Width
        {
            get
            {
                return (this.xSize * 7);
            }
        }
    }
}

