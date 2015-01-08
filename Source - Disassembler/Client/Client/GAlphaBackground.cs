namespace Client
{
    using System;

    public class GAlphaBackground : Gump
    {
        protected int m_BorderColor;
        protected Client.Clipper m_Clipper;
        protected bool m_DrawBorder;
        protected float m_FillAlpha;
        protected int m_FillColor;
        protected int m_Height;
        protected Client.OnDispose m_OnDispose;
        protected int m_RightColor;
        protected bool m_ShouldHitTest;
        protected int m_Width;

        public GAlphaBackground(int X, int Y, int Width, int Height) : base(X, Y)
        {
            this.m_FillAlpha = 0.4f;
            this.m_ShouldHitTest = true;
            this.m_DrawBorder = true;
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            this.m_Width = Width;
            this.m_Height = Height;
        }

        protected internal override void Draw(int X, int Y)
        {
            ClipType inside = ClipType.Inside;
            if (this.m_Clipper != null)
            {
                inside = this.m_Clipper.Evaluate(X, Y, this.m_Width, this.m_Height);
            }
            if (inside == ClipType.Inside)
            {
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                if (this.m_FillAlpha == 1f)
                {
                    Renderer.SetAlphaEnable(false);
                    if (this.m_FillColor == this.m_RightColor)
                    {
                        Renderer.SolidRect(this.m_FillColor, X + 1, Y + 1, this.m_Width - 2, this.m_Height - 2);
                    }
                    else
                    {
                        Renderer.GradientRectLR(this.m_FillColor, this.m_RightColor, X + 1, Y + 1, this.m_Width - 2, this.m_Height - 2);
                    }
                }
                else if (this.m_FillAlpha > 0f)
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(this.m_FillAlpha);
                    if (this.m_FillColor == this.m_RightColor)
                    {
                        Renderer.SolidRect(this.m_FillColor, X + 1, Y + 1, this.m_Width - 2, this.m_Height - 2);
                    }
                    else
                    {
                        Renderer.GradientRectLR(this.m_FillColor, this.m_RightColor, X + 1, Y + 1, this.m_Width - 2, this.m_Height - 2);
                    }
                }
                Renderer.SetAlphaEnable(false);
                if (this.m_DrawBorder)
                {
                    Renderer.TransparentRect(this.m_BorderColor, X, Y, this.m_Width, this.m_Height);
                }
                Renderer.AlphaTestEnable = true;
            }
            else if (inside == ClipType.Partial)
            {
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                if (this.m_FillAlpha == 1f)
                {
                    Renderer.SetAlphaEnable(false);
                    Renderer.SolidRect(this.m_FillColor, X + 1, Y + 1, this.m_Width - 2, this.m_Height - 2, this.m_Clipper);
                }
                else if (this.m_FillAlpha > 0f)
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(this.m_FillAlpha);
                    Renderer.SolidRect(this.m_FillColor, X + 1, Y + 1, this.m_Width - 2, this.m_Height - 2, this.m_Clipper);
                }
                Renderer.SetAlphaEnable(false);
                if (this.m_DrawBorder)
                {
                    Renderer.TransparentRect(this.m_BorderColor, X, Y, this.m_Width, this.m_Height, this.m_Clipper);
                }
                Renderer.AlphaTestEnable = true;
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            if (!this.m_ShouldHitTest)
            {
                return false;
            }
            return (!Engine.amMoving && (Engine.TargetHandler == null));
        }

        protected internal override void OnDispose()
        {
            if (this.m_OnDispose != null)
            {
                this.m_OnDispose(this);
            }
        }

        public int BorderColor
        {
            get
            {
                return this.m_BorderColor;
            }
            set
            {
                this.m_BorderColor = value;
            }
        }

        public Client.Clipper Clipper
        {
            get
            {
                return this.m_Clipper;
            }
            set
            {
                this.m_Clipper = value;
            }
        }

        public Client.OnDispose Disposer
        {
            get
            {
                return this.m_OnDispose;
            }
            set
            {
                this.m_OnDispose = value;
            }
        }

        public bool DrawBorder
        {
            get
            {
                return this.m_DrawBorder;
            }
            set
            {
                this.m_DrawBorder = value;
            }
        }

        public float FillAlpha
        {
            get
            {
                return this.m_FillAlpha;
            }
            set
            {
                this.m_FillAlpha = value;
            }
        }

        public int FillColor
        {
            get
            {
                return this.m_FillColor;
            }
            set
            {
                this.m_FillColor = value;
                this.m_RightColor = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public int RightColor
        {
            get
            {
                return this.m_RightColor;
            }
            set
            {
                this.m_RightColor = value;
            }
        }

        public bool ShouldHitTest
        {
            get
            {
                return this.m_ShouldHitTest;
            }
            set
            {
                this.m_ShouldHitTest = value;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }
    }
}

