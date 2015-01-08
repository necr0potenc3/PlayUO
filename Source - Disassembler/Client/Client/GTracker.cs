namespace Client
{
    using System;

    public abstract class GTracker : GLabel
    {
        private static string[] m_DirectionStrings = new string[] { "north-west", "north", "north-east", "east", "south-east", "south", "south-west", "west" };
        private static IHue[] m_Hues = new IHue[] { Hues.Load(0x44), Hues.Load(0x3f), Hues.Load(0x3a), Hues.Load(0x35), Hues.Load(0x30), Hues.Load(0x2b), Hues.Load(0x26) };
        private int m_xLast;
        private int m_yLast;

        public GTracker() : base("", Engine.DefaultFont, Engine.DefaultHue, 4, 4)
        {
            this.m_xLast = this.m_yLast = -2147483648;
        }

        protected abstract string GetPluralString(string direction, int distance);
        protected abstract string GetSingularString(string direction);
        protected void Render(int X, int Y, int xTarget, int yTarget)
        {
            Mobile player = World.Player;
            if (player != null)
            {
                if ((this.m_xLast != xTarget) || (this.m_yLast != yTarget))
                {
                    int num3;
                    Direction direction = Engine.GetDirection(player.X, player.Y, xTarget, yTarget);
                    string str = m_DirectionStrings[(int) (Direction.West & direction)];
                    int num = Math.Abs((int) (xTarget - player.X));
                    int num2 = Math.Abs((int) (yTarget - player.Y));
                    if (num > num2)
                    {
                        num3 = num2 + (num - num2);
                    }
                    else
                    {
                        num3 = num + (num2 - num);
                    }
                    int index = (num3 - 2) / 2;
                    if (index < 0)
                    {
                        index = 0;
                    }
                    else if (index > 6)
                    {
                        index = 6;
                    }
                    string str2 = (num3 == 1) ? this.GetSingularString(str) : this.GetPluralString(str, num3);
                    this.Hue = m_Hues[index];
                    this.Text = str2;
                }
                base.Render(X, Y);
                Stats.Add(this);
            }
        }

        public override int Y
        {
            get
            {
                return Stats.yOffset;
            }
        }
    }
}

