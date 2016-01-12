namespace Client
{
    using System;
    using System.Drawing;

    public class RectangleList
    {
        private int m_Count;
        private Rectangle[] m_Rects = new Rectangle[8];

        public void Add(Rectangle rect)
        {
            for (int i = 0; i < this.m_Count; i++)
            {
                Rectangle rectangle = this.m_Rects[i];
                if (rect.IntersectsWith(rectangle))
                {
                    Rectangle[] rectangleArray = Punch(rect, rectangle);
                    for (int j = 0; j < rectangleArray.Length; j++)
                    {
                        this.Add(rectangleArray[j]);
                    }
                    return;
                }
            }
            this.InternalAdd(rect);
        }

        public void Clear()
        {
            this.m_Count = 0;
        }

        private void InternalAdd(Rectangle rect)
        {
            if (this.m_Count >= this.m_Rects.Length)
            {
                Rectangle[] rects = this.m_Rects;
                this.m_Rects = new Rectangle[rects.Length * 2];
                for (int i = 0; i < rects.Length; i++)
                {
                    this.m_Rects[i] = rects[i];
                }
            }
            this.m_Rects[this.m_Count++] = rect;
        }

        private void InternalRemove(int index)
        {
            this.m_Count--;
            for (int i = index; i < this.m_Count; i++)
            {
                this.m_Rects[i] = this.m_Rects[i + 1];
            }
        }

        private static Rectangle[] Punch(Rectangle cookie, Rectangle cutter)
        {
            if (!cookie.IntersectsWith(cutter))
            {
                return new Rectangle[] { cookie };
            }
            int width = cutter.X - cookie.X;
            int height = cutter.Y - cookie.Y;
            int num3 = (cookie.X + cookie.Width) - (cutter.X + cutter.Width);
            int num4 = (cookie.Y + cookie.Height) - (cutter.Y + cutter.Height);
            int num5 = 0;
            if (width > 0)
            {
                num5++;
            }
            else
            {
                width = 0;
            }
            if (height > 0)
            {
                num5++;
            }
            else
            {
                height = 0;
            }
            if (num3 > 0)
            {
                num5++;
            }
            else
            {
                num3 = 0;
            }
            if (num4 > 0)
            {
                num5++;
            }
            else
            {
                num4 = 0;
            }
            Rectangle[] rectangleArray = new Rectangle[num5];
            num5 = 0;
            if (width > 0)
            {
                rectangleArray[num5++] = new Rectangle(cookie.X, cookie.Y, width, cookie.Height);
            }
            if (height > 0)
            {
                rectangleArray[num5++] = new Rectangle(cookie.X + width, cookie.Y, (cookie.Width - width) - num3, height);
            }
            if (num3 > 0)
            {
                rectangleArray[num5++] = new Rectangle(cutter.X + cutter.Width, cookie.Y, num3, cookie.Height);
            }
            if (num4 > 0)
            {
                rectangleArray[num5++] = new Rectangle(cookie.X + width, cutter.Y + cutter.Height, (cookie.Width - width) - num3, num4);
            }
            return rectangleArray;
        }

        public void Remove(Rectangle rect)
        {
            for (int i = this.m_Count - 1; i >= 0; i--)
            {
                Rectangle rectangle = this.m_Rects[i];
                if (rect.IntersectsWith(rectangle))
                {
                    this.InternalRemove(i);
                    Rectangle[] rectangleArray = Punch(rectangle, rect);
                    for (int j = 0; j < rectangleArray.Length; j++)
                    {
                        this.InternalAdd(rectangleArray[j]);
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return this.m_Count;
            }
        }

        public Rectangle this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.m_Count))
                {
                    throw new IndexOutOfRangeException();
                }
                return this.m_Rects[index];
            }
        }
    }
}