namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System;
    using System.Collections;

    public class Effects
    {
        private bool m_DrawTemperature;
        private ArrayList m_Fades = new ArrayList();
        private int m_GlobalLight;
        private bool m_Invalidated;
        private ArrayList m_List = new ArrayList();
        private ArrayList m_Lock = new ArrayList();
        private bool m_Locked;
        private ArrayList m_Particles = new ArrayList();
        private CustomVertex.TransformedColoredTextured[] m_Screen = new CustomVertex.TransformedColoredTextured[4];
        private int m_Temperature = 10;

        public Effects()
        {
            this.m_Screen[3].X = -0.5f;
            this.m_Screen[3].Y = -0.5f;
            this.m_Screen[1].X = -0.5f + Engine.ScreenWidth;
            this.m_Screen[1].Y = -0.5f;
            this.m_Screen[0].X = -0.5f + Engine.ScreenWidth;
            this.m_Screen[0].Y = -0.5f + Engine.ScreenHeight;
            this.m_Screen[2].X = -0.5f;
            this.m_Screen[2].Y = -0.5f + Engine.ScreenHeight;
        }

        public void Add(Effect e)
        {
            this.m_List.Add(e);
            e.OnStart();
        }

        public void Add(Fade f)
        {
            if (this.m_Locked)
            {
                this.m_Lock.Add(f);
            }
            else
            {
                this.m_Fades.Add(f);
            }
        }

        public void Add(IParticle p)
        {
            this.m_Particles.Add(p);
            this.m_Invalidated = true;
        }

        public void ClearParticle()
        {
            if (this.m_Particles.Count > 0)
            {
                ((IParticle) this.m_Particles[0]).Invalidate();
            }
        }

        public void ClearParticles()
        {
            int count = this.m_Particles.Count;
            for (int i = 0; i < count; i++)
            {
                ((IParticle) this.m_Particles[i]).Invalidate();
            }
        }

        public void Draw()
        {
            int count = this.m_List.Count;
            int index = 0;
            if (count > 0)
            {
                Renderer.CullEnable = false;
                Renderer.SetFilterEnable(true);
                while (index < count)
                {
                    Effect effect = (Effect) this.m_List[index];
                    if (!effect.Slice())
                    {
                        effect.OnStop();
                        this.m_List.RemoveAt(index);
                        EffectList children = effect.Children;
                        int num3 = children.Count;
                        for (int i = 0; i < num3; i++)
                        {
                            this.m_List.Add(children[i]);
                            children[i].OnStart();
                        }
                        count = this.m_List.Count;
                    }
                    else
                    {
                        index++;
                    }
                }
                Renderer.SetFilterEnable(false);
                Renderer.CullEnable = true;
            }
            count = this.m_Particles.Count;
            index = 0;
            if (count > 0)
            {
                Renderer.CullEnable = false;
                Renderer.SetFilterEnable(true);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(1f);
                Renderer.ColorAlphaEnable = true;
                Random random = new Random();
                while (index < count)
                {
                    IParticle particle = (IParticle) this.m_Particles[index];
                    if (!particle.Slice())
                    {
                        this.m_Particles.RemoveAt(index);
                        particle.Destroy();
                        count = this.m_Particles.Count;
                        this.m_Invalidated = false;
                    }
                    else
                    {
                        if (this.m_Invalidated)
                        {
                            count = this.m_Particles.Count;
                            this.m_Invalidated = false;
                        }
                        index++;
                    }
                }
                Renderer.ColorAlphaEnable = false;
                Renderer.SetAlphaEnable(false);
                Renderer.SetFilterEnable(false);
                Renderer.CullEnable = true;
            }
            if (this.m_DrawTemperature)
            {
                if (this.Temperature > 0x19)
                {
                    Renderer.SetTexture(null);
                    Renderer.SetAlphaEnable(true);
                    Renderer.AlphaTestEnable = false;
                    Renderer.SetAlpha((this.m_Temperature - 25f) / 102f);
                    this.m_Screen[0].Color = this.m_Screen[1].Color = this.m_Screen[2].Color = this.m_Screen[3].Color = Renderer.GetQuadColor(0xff4020);
                    Renderer.DrawQuadPrecalc(this.m_Screen);
                    Renderer.SetAlphaEnable(false);
                    Renderer.AlphaTestEnable = true;
                }
                else if (this.Temperature < 10)
                {
                    Renderer.SetTexture(null);
                    Renderer.SetAlphaEnable(true);
                    Renderer.AlphaTestEnable = false;
                    Renderer.SetAlpha(((float) Math.Abs((int) (this.m_Temperature - 10))) / 118f);
                    this.m_Screen[0].Color = this.m_Screen[1].Color = this.m_Screen[2].Color = this.m_Screen[3].Color = Renderer.GetQuadColor(0x40c0ff);
                    Renderer.DrawQuadPrecalc(this.m_Screen);
                    Renderer.SetAlphaEnable(false);
                    Renderer.AlphaTestEnable = true;
                }
            }
            int globalLight = this.m_GlobalLight;
            Mobile player = World.Player;
            if (player != null)
            {
                globalLight -= player.LightLevel;
            }
            if (globalLight < 0)
            {
                globalLight = 0;
            }
            else if (globalLight > 0x1f)
            {
                globalLight = 0x1f;
            }
            if (globalLight != 0)
            {
                Renderer.SetTexture(null);
                Renderer.SetAlphaEnable(true);
                Renderer.AlphaTestEnable = false;
                Renderer.SetAlpha((float) (((double) globalLight) / 31.0));
                this.m_Screen[0].Color = this.m_Screen[1].Color = this.m_Screen[2].Color = this.m_Screen[3].Color = Renderer.GetQuadColor(0);
                Renderer.DrawQuadPrecalc(this.m_Screen);
            }
            count = this.m_Fades.Count;
            index = 0;
            if (count > 0)
            {
                Renderer.SetTexture(null);
                Renderer.SetAlphaEnable(true);
                Renderer.AlphaTestEnable = false;
                double alpha = 0.0;
                while (index < count)
                {
                    Fade fade = (Fade) this.m_Fades[index];
                    if (fade.Evaluate(ref alpha))
                    {
                        Renderer.SetAlpha((float) alpha);
                        this.m_Screen[0].Color = this.m_Screen[1].Color = this.m_Screen[2].Color = this.m_Screen[3].Color = Renderer.GetQuadColor(fade.Color);
                        Renderer.DrawQuadPrecalc(this.m_Screen);
                    }
                    else
                    {
                        fade.OnFadeComplete();
                        this.m_Fades.RemoveAt(index);
                        count = this.m_Fades.Count;
                        continue;
                    }
                    index++;
                }
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
        }

        public void Lock()
        {
            this.m_Locked = true;
        }

        public void Offset(int xDelta, int yDelta)
        {
            int count = this.m_Particles.Count;
            int index = 0;
            while (index < count)
            {
                IParticle particle = (IParticle) this.m_Particles[index];
                if (!particle.Offset(xDelta, yDelta))
                {
                    this.m_Particles.RemoveAt(index);
                    particle.Destroy();
                    count = this.m_Particles.Count;
                }
                else
                {
                    index++;
                }
            }
        }

        public static float RandomRainAngle(Random rnd)
        {
            double num = rnd.NextDouble();
            double num2 = 1.5707963267948966 + (((3.1415926535897931 * num) * 0.5) - 0.78539816339744828);
            return (float) num2;
        }

        public static float RandomSnowAngle(Random rnd)
        {
            double num = rnd.NextDouble();
            double num2 = 3.1415926535897931 * num;
            return (float) num2;
        }

        public void Unlock()
        {
            this.m_Locked = false;
            for (int i = 0; i < this.m_Lock.Count; i++)
            {
                this.m_Fades.Add(this.m_Lock[i]);
            }
            this.m_Fades.Clear();
        }

        public bool DrawTemperature
        {
            get
            {
                return this.m_DrawTemperature;
            }
            set
            {
                this.m_DrawTemperature = value;
            }
        }

        public int GlobalLight
        {
            get
            {
                return this.m_GlobalLight;
            }
            set
            {
                this.m_GlobalLight = value;
            }
        }

        public bool Locked
        {
            get
            {
                return this.m_Locked;
            }
        }

        public int ParticleCount
        {
            get
            {
                return this.m_Particles.Count;
            }
        }

        public int Temperature
        {
            get
            {
                return this.m_Temperature;
            }
            set
            {
                this.m_Temperature = value;
            }
        }
    }
}

