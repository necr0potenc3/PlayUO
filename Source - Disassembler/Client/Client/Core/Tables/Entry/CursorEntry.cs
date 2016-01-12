namespace Client
{
    using Client.Targeting;
    using Microsoft.DirectX.Direct3D;

    public class CursorEntry
    {
        private bool m_Draw;
        public int m_Graphic;
        public Client.Texture m_Image;
        public int m_Type;
        private Client.VertexCache m_vCache;
        private static CustomVertex.TransformedColoredTextured[] m_vTargetPool = VertexConstructor.Create();
        public int m_xOffset;
        public int m_yOffset;

        public CursorEntry(int graphic, int type, int xOffset, int yOffset, Client.Texture image)
        {
            this.m_Graphic = graphic;
            this.m_Type = type;
            this.m_xOffset = xOffset;
            this.m_yOffset = yOffset;
            this.m_Image = image;
            this.m_Draw = (this.m_Image != null) && !this.m_Image.IsEmpty();
            this.m_vCache = new Client.VertexCache();
        }

        public void Draw(int xMouse, int yMouse)
        {
            if (this.m_Draw)
            {
                this.m_vCache.Draw(this.m_Image, xMouse - this.m_xOffset, yMouse - this.m_yOffset);
                if (this.m_Graphic == 12)
                {
                    int color = 0;
                    ServerTargetHandler targetHandler = Engine.TargetHandler as ServerTargetHandler;
                    if (targetHandler != null)
                    {
                        if ((targetHandler.Flags & ServerTargetFlags.Harmful) != ServerTargetFlags.None)
                        {
                            if (targetHandler.Action == TargetAction.Poison)
                            {
                                color = 0xff00;
                            }
                            else
                            {
                                color = 0xcc0000;
                            }
                        }
                        else if ((targetHandler.Flags & ServerTargetFlags.Beneficial) != ServerTargetFlags.None)
                        {
                            if (targetHandler.Action == TargetAction.Cure)
                            {
                                color = 0xffaa;
                            }
                            else
                            {
                                color = 0xffff;
                            }
                        }
                    }
                    if (color > 0)
                    {
                        if (!Renderer.m_AlphaEnable)
                        {
                            Renderer.SetAlphaEnablePrecalc(true);
                        }
                        if (Renderer.m_FilterEnable)
                        {
                            Renderer.SetFilterEnablePrecalc(false);
                        }
                        Renderer.SetAlpha(1f);
                        Renderer.ColorAlphaEnable = true;
                        Engine.m_TargetCursorImage.Draw(xMouse - this.m_xOffset, yMouse - this.m_yOffset, color, m_vTargetPool);
                        Renderer.ColorAlphaEnable = false;
                        Renderer.SetAlphaEnablePrecalc(false);
                    }
                }
            }
        }
    }
}