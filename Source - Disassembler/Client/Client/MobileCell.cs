namespace Client
{
    using System;
    using System.Collections;

    public class MobileCell : IAnimatedCell, ICell, IDisposable, IEntity
    {
        private static Queue m_InstancePool = new Queue();
        public Mobile m_Mobile;
        private sbyte m_Z;
        private static Type MyType = typeof(MobileCell);

        private MobileCell(Mobile m)
        {
            this.m_Mobile = m;
            this.m_Z = (sbyte) m.Z;
        }

        public void GetPackage(ref int Body, ref int Action, ref int Direction, ref int Frame, ref int Hue)
        {
            Mobile m = this.m_Mobile;
            Body = m.Body;
            if (m.Ghost)
            {
                Body = 970;
            }
            Hue = m.Hue;
            Hue ^= 0x8000;
            ArrayList equip = m.Equip;
            if (m.Walking.Count > 0)
            {
                GenericAction action;
                Direction = Engine.GetAnimDirection((byte) ((WalkAnimation) m.Walking.Peek()).NewDir);
                int num = 0;
                if ((equip.Count > 0) && (((EquipEntry) equip[0]).m_Layer == Layer.Mount))
                {
                    action = ((Direction & 0x80) == 0) ? GenericAction.MountedWalk : GenericAction.MountedRun;
                    num = ((Direction & 0x80) == 0) ? 2 : 1;
                }
                else
                {
                    action = ((Direction & 0x80) == 0) ? GenericAction.Walk : GenericAction.Run;
                    num = ((Direction & 0x80) == 0) ? 4 : 2;
                }
                Action = Engine.m_Animations.ConvertAction(Body, m.Serial, m.X, m.Y, Direction, action, m);
                int num2 = Engine.m_Animations.GetFrameCount(Body, Action, Direction & 7);
                if (num2 == 0)
                {
                    Frame = 0;
                }
                else
                {
                    Frame = (m.MovedTiles * num) % num2;
                }
            }
            else
            {
                Direction = Engine.GetAnimDirection(m.Direction);
                if ((m.Animation == null) || !m.Animation.Running)
                {
                    GenericAction mountedStand;
                    if ((equip.Count > 0) && (((EquipEntry) equip[0]).m_Layer == Layer.Mount))
                    {
                        mountedStand = GenericAction.MountedStand;
                    }
                    else
                    {
                        mountedStand = GenericAction.Stand;
                    }
                    Action = Engine.m_Animations.ConvertAction(Body, m.Serial, m.X, m.Y, Direction, mountedStand, m);
                    Frame = 0;
                }
                else
                {
                    int frames = Renderer.m_Frames;
                    Action = m.Animation.Action;
                    Direction = Engine.GetAnimDirection((byte) (m.Direction & 7));
                    Action = Action % 0x23;
                    Direction &= 7;
                    int num4 = Engine.m_Animations.GetFrameCount(Body, Action, Direction);
                    if (num4 == 0)
                    {
                        num4 = 1;
                    }
                    int num5 = (m.Animation.Delay * 2) + 4;
                    if (num5 < 1)
                    {
                        num5 = 1;
                    }
                    Frame = ((frames - m.Animation.Start) / num5) % num4;
                    if (!m.Animation.Forward)
                    {
                        Frame = (num4 - 1) - Frame;
                    }
                    if ((m.Animation.Repeat && (m.Animation.RepeatCount != 0)) && (frames >= ((m.Animation.Start + ((m.Animation.RepeatCount * num4) * num5)) - 1)))
                    {
                        if (m.Animation.OnAnimationEnd != null)
                        {
                            m.Animation.OnAnimationEnd(m.Animation, m);
                        }
                    }
                    else if ((!m.Animation.Repeat && (frames >= ((m.Animation.Start + (num4 * num5)) - 1))) && (m.Animation.OnAnimationEnd != null))
                    {
                        m.Animation.OnAnimationEnd(m.Animation, m);
                    }
                    if ((m.Animation.Repeat && (m.Animation.RepeatCount != 0)) && (frames >= (m.Animation.Start + ((m.Animation.RepeatCount * num4) * num5))))
                    {
                        GenericAction stand;
                        m.Animation.Stop();
                        if ((equip.Count > 0) && (((EquipEntry) equip[0]).m_Layer == Layer.Mount))
                        {
                            stand = GenericAction.MountedStand;
                        }
                        else
                        {
                            stand = GenericAction.Stand;
                        }
                        Action = Engine.m_Animations.ConvertAction(Body, m.Serial, m.X, m.Y, Direction, stand, m);
                        Frame = 0;
                        Direction = Engine.GetAnimDirection(m.Direction);
                        Direction = Direction % 8;
                        m.Animation = null;
                    }
                    else if (!m.Animation.Repeat && (frames >= (m.Animation.Start + (num4 * num5))))
                    {
                        GenericAction action4;
                        m.Animation.Stop();
                        if ((equip.Count > 0) && (((EquipEntry) equip[0]).m_Layer == Layer.Mount))
                        {
                            action4 = GenericAction.MountedStand;
                        }
                        else
                        {
                            action4 = GenericAction.Stand;
                        }
                        Action = Engine.m_Animations.ConvertAction(Body, m.Serial, m.X, m.Y, Direction, action4, m);
                        Frame = 0;
                        Direction = Engine.GetAnimDirection(m.Direction);
                        Direction = Direction % 8;
                        m.Animation = null;
                    }
                }
            }
        }

        public static MobileCell Instantiate(Mobile m)
        {
            if (m_InstancePool.Count > 0)
            {
                MobileCell cell = (MobileCell) m_InstancePool.Dequeue();
                cell.m_Mobile = m;
                cell.m_Z = (sbyte) m.Z;
                return cell;
            }
            return new MobileCell(m);
        }

        void IDisposable.Dispose()
        {
            m_InstancePool.Enqueue(this);
        }

        public Type CellType
        {
            get
            {
                return MyType;
            }
        }

        public byte Height
        {
            get
            {
                return 15;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Mobile.Serial;
            }
        }

        public sbyte SortZ
        {
            get
            {
                return this.m_Z;
            }
            set
            {
            }
        }

        public sbyte Z
        {
            get
            {
                return this.m_Z;
            }
        }
    }
}

