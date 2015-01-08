namespace Client
{
    using System;

    public class CorpseCell : IAnimatedCell, ICell, IDisposable, IEntity
    {
        private int m_Action;
        private int m_Body;
        private int m_Direction;
        private int m_Frame;
        private int m_Hue;
        private int m_Serial;
        private int m_X;
        private int m_Y;
        private sbyte m_Z;
        private static Type MyType = typeof(CorpseCell);

        public CorpseCell(Item src)
        {
            this.m_Serial = src.Serial;
            this.m_Z = (sbyte) src.Z;
            this.m_Body = src.Amount;
            this.m_Action = Engine.m_Animations.ConvertAction(this.m_Body, src.CorpseSerial, this.m_X, this.m_Y, src.Direction, GenericAction.Die, null);
            this.m_Direction = Engine.GetAnimDirection(src.Direction);
            this.m_Frame = Engine.m_Animations.GetFrameCount(this.m_Body, this.m_Action, this.m_Direction) - 1;
            if (this.m_Frame < 0)
            {
                this.m_Frame = 0;
            }
            this.m_X = src.X;
            this.m_Y = src.Y;
            this.m_Hue = src.Hue;
            this.m_Hue ^= 0x8000;
        }

        public void GetPackage(ref int Body, ref int Action, ref int Direction, ref int Frame, ref int Hue)
        {
            Body = this.m_Body;
            Action = this.m_Action;
            Direction = this.m_Direction;
            Frame = this.m_Frame;
            Hue = this.m_Hue;
        }

        void IDisposable.Dispose()
        {
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
                return this.m_Serial;
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

