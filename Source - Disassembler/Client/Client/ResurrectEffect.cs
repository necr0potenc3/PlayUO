namespace Client
{
    using System;

    public class ResurrectEffect : Fade
    {
        public ResurrectEffect() : base(0xffffff, 0f, 1f, 0.5f)
        {
            Renderer.m_DeathOverride = true;
        }

        protected internal override void OnFadeComplete()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                player.Animation = null;
            }
            Renderer.m_DeathOverride = false;
            Engine.Effects.Add(new Fade(0xffffff, 1f, 0f, 1f));
        }
    }
}

