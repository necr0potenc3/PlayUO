namespace Client.Targeting
{
    using Client;
    using System;
    using System.IO;

    public class ExportTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
        }

        public void OnTarget(object o)
        {
            if (o is IPoint3D)
            {
                Engine.AddTextMessage("Target another location to complete the bounding box.");
                Engine.TargetHandler = new InternalTargetHandler(new Point3D((IPoint3D) o));
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }

        private class InternalTargetHandler : ITargetHandler
        {
            private Point3D m_Start;

            public InternalTargetHandler(Point3D p)
            {
                this.m_Start = p;
            }

            public static void FixPoints(ref Point3D top, ref Point3D bottom)
            {
                if (bottom.X < top.X)
                {
                    int x = top.X;
                    top.X = bottom.X;
                    bottom.X = x;
                }
                if (bottom.Y < top.Y)
                {
                    int y = top.Y;
                    top.Y = bottom.Y;
                    bottom.Y = y;
                }
                if (bottom.Z < top.Z)
                {
                    int z = top.Z;
                    top.Z = bottom.Z;
                    bottom.Z = z;
                }
            }

            public void OnCancel(TargetCancelType type)
            {
            }

            public void OnTarget(object o)
            {
                if (o is IPoint3D)
                {
                    Point3D start = this.m_Start;
                    Point3D bottom = new Point3D((IPoint3D) o);
                    FixPoints(ref start, ref bottom);
                    using (StreamWriter writer = new StreamWriter("exported.log", true))
                    {
                        writer.WriteLine("Exported on {0}", DateTime.Now);
                        writer.WriteLine();
                        foreach (Item item in World.Items.Values)
                        {
                            if (((item.X >= start.X) && (item.X <= bottom.X)) && ((item.Y >= start.Y) && (item.Y <= bottom.Y)))
                            {
                                writer.WriteLine("0x{0:X4}\t0x{1:X4}\t({2}, {3}, {4})\t\t// {5}", new object[] { item.ID & 0x3fff, item.Hue, item.X, item.Y, item.Z, Localization.GetString(0xf9060 + (item.ID & 0x3fff)) });
                            }
                        }
                        writer.WriteLine("#########################");
                    }
                }
                else
                {
                    Engine.TargetHandler = this;
                }
            }
        }
    }
}

