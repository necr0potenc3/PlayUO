namespace Client
{
    public class DeathEffect : Fade
    {
        public DeathEffect() : base(0, 1f, 1f, 2f)
        {
            Cursor.Visible = false;
        }

        protected internal override void OnFadeComplete()
        {
            Engine.Effects.Add(new MiddleDeathEffect());
        }
    }
}