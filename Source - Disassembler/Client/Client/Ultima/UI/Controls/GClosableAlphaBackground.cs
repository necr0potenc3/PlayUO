namespace Client
{
    using System.Windows.Forms;

    public class GClosableAlphaBackground : GAlphaBackground
    {
        public GClosableAlphaBackground(int X, int Y, int Width, int Height) : base(X, Y, Width, Height)
        {
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Gumps.Destroy(this);
            }
        }
    }
}