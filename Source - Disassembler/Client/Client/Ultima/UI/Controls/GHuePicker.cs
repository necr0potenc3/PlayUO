﻿namespace Client
{
    using System.Windows.Forms;

    public class GHuePicker : Gump
    {
        private int[,,] m_ColorTable;
        private Client.OnHueSelect m_OnHueRelease;
        private Client.OnHueSelect m_OnHueSelect;
        private int m_xShade;
        private int m_yShade;
        private int m_zShade;
        private const int xShades = 20;
        private const int xSize = 8;
        private const int yShades = 10;
        private const int ySize = 8;
        private const int zShades = 5;

        public GHuePicker(int X, int Y) : base(X, Y)
        {
            base.m_Tooltip = new Tooltip("");
            this.m_ColorTable = new int[20, 10, 5];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        ushort num4 = Hues.GetData((1 + (((j * 20) + i) * 5)) + k).colors[0x30];
                        int num5 = (num4 >> 10) & 0x1f;
                        int num6 = (num4 >> 5) & 0x1f;
                        int num7 = num4 & 0x1f;
                        num5 = (int)(num5 * 8.225806f);
                        num6 = (int)(num6 * 8.225806f);
                        num7 = (int)(num7 * 8.225806f);
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
            this.m_xShade = X / 8;
            this.m_yShade = Y / 8;
            if ((this.m_xShade != xShade) || (this.m_yShade != yShade))
            {
                if (this.m_OnHueSelect != null)
                {
                    this.m_OnHueSelect(this.Hue, this);
                }
                if (Engine.GMPrivs)
                {
                    ((Tooltip)base.m_Tooltip).Text = string.Format("0x{0:X}", this.Hue);
                }
                Engine.Redraw();
            }
        }

        public int Color(int Index)
        {
            return this.m_ColorTable[this.m_xShade, this.m_yShade, Index];
        }

        private int ColorAt(int X, int Y)
        {
            if (X >= 20)
            {
                X = 0x13;
            }
            if (Y >= 10)
            {
                Y = 9;
            }
            return this.m_ColorTable[X, Y, this.m_zShade];
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            Renderer.AlphaTestEnable = false;
            int width = 4;
            int num2 = 8 - width;
            int height = 4;
            int num4 = 8 - height;
            Renderer.SolidRect(this.ColorAt(0, 0), X, Y, width, height);
            Renderer.SolidRect(this.ColorAt(0, 10), X, (Y + 80) - num4, width, num4);
            Renderer.SolidRect(this.ColorAt(20, 0), (X + 160) - num2, Y, num2, height);
            Renderer.SolidRect(this.ColorAt(20, 10), (X + 160) - num2, (Y + 80) - num4, num2, num4);
            for (int i = 0; i < 0x13; i++)
            {
                int num7 = this.ColorAt(i, 0);
                int num8 = this.ColorAt(i + 1, 0);
                Renderer.GradientRect4(num7, num8, num8, num7, (X + (i * 8)) + width, Y, 8, height);
                num7 = this.ColorAt(i, 10);
                num8 = this.ColorAt(i + 1, 10);
                Renderer.GradientRect4(num7, num8, num8, num7, (X + (i * 8)) + width, (Y + 80) - num4, 8, num4);
                for (int k = 0; k < 9; k++)
                {
                    int num10 = this.ColorAt(i, k);
                    int num11 = this.ColorAt(i + 1, k);
                    int num12 = this.ColorAt(i + 1, k + 1);
                    int num13 = this.ColorAt(i, k + 1);
                    int num14 = X + (i * 8);
                    int num15 = Y + (k * 8);
                    Renderer.GradientRect4(num10, num11, num12, num13, num14 + width, num15 + height, 8, 8);
                }
            }
            for (int j = 0; j < 9; j++)
            {
                int num17 = this.ColorAt(0, j);
                int num18 = this.ColorAt(0, j + 1);
                Renderer.GradientRect4(num17, num17, num18, num18, X, (Y + (j * 8)) + height, width, 8);
                num17 = this.ColorAt(20, j);
                num18 = this.ColorAt(20, j + 1);
                Renderer.GradientRect4(num17, num17, num18, num18, (X + 160) - num2, (Y + (j * 8)) + height, num2, 8);
            }
            Renderer.SetAlphaEnable(true);
            Renderer.SetAlpha(0.5f);
            Renderer.SolidRect(0x80ffff, (X + (this.m_xShade * 8)) + 2, (Y + (this.m_yShade * 8)) + 3, 3, 1);
            Renderer.SolidRect(0x80ffff, (X + (this.m_xShade * 8)) + 3, (Y + (this.m_yShade * 8)) + 2, 1, 3);
            Renderer.SetAlphaEnable(false);
            Renderer.SolidRect(0x80ffff, (X + (this.m_xShade * 8)) + 3, (Y + (this.m_yShade * 8)) + 3, 1, 1);
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

        public int Brightness
        {
            get
            {
                return this.m_zShade;
            }
            set
            {
                this.m_zShade = value;
                if (this.m_zShade < 0)
                {
                    this.m_zShade = 0;
                }
                if (this.m_zShade >= 5)
                {
                    this.m_zShade = 4;
                }
                if (this.m_OnHueSelect != null)
                {
                    this.m_OnHueSelect(this.Hue, this);
                }
                if (Engine.GMPrivs)
                {
                    ((Tooltip)base.m_Tooltip).Text = string.Format("0x{0:X}", this.Hue);
                }
                Engine.Redraw();
            }
        }

        public override int Height
        {
            get
            {
                return 80;
            }
        }

        public int Hue
        {
            get
            {
                return ((2 + (((this.m_yShade * 20) + this.m_xShade) * 5)) + this.m_zShade);
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

        public int ShadeX
        {
            set
            {
                this.m_xShade = value;
                if (this.m_xShade < 0)
                {
                    this.m_xShade = 0;
                }
                if (this.m_xShade >= 20)
                {
                    this.m_xShade = 0x13;
                }
                if (this.m_OnHueSelect != null)
                {
                    this.m_OnHueSelect(this.Hue, this);
                }
                if (Engine.GMPrivs)
                {
                    ((Tooltip)base.m_Tooltip).Text = string.Format("0x{0:X}", this.Hue);
                }
                Engine.Redraw();
            }
        }

        public int ShadeY
        {
            set
            {
                this.m_yShade = value;
                if (this.m_yShade < 0)
                {
                    this.m_yShade = 0;
                }
                if (this.m_yShade >= 10)
                {
                    this.m_yShade = 9;
                }
                if (this.m_OnHueSelect != null)
                {
                    this.m_OnHueSelect(this.Hue, this);
                }
                if (Engine.GMPrivs)
                {
                    ((Tooltip)base.m_Tooltip).Text = string.Format("0x{0:X}", this.Hue);
                }
                Engine.Redraw();
            }
        }

        public override int Width
        {
            get
            {
                return 160;
            }
        }
    }
}