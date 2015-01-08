namespace Client
{
    using System;

    public class SwingSpeedInfoNode : Table2DInfoNode
    {
        private int[] m_Speeds;

        public SwingSpeedInfoNode(string name, string[] cols, string[] rows, int[] speeds) : base(name, new TableDescriptor2D[] { new TableDescriptor2D(cols, rows, null) })
        {
            this.m_Speeds = speeds;
            base.Descriptors[0].Function = new TableComputeFunction(this.SwingSpeedCompute);
        }

        private object SwingSpeedCompute(int row, int col)
        {
            string active = base.Tree.Provider.Inputs[1].Active as string;
            int num = 0;
            if ((active != null) && (active.Length > 0))
            {
                try
                {
                    num = int.Parse(active);
                }
                catch
                {
                }
            }
            int num2 = this.m_Speeds[row] * (100 + num);
            num2 /= 100;
            int num3 = 1 + ((int) (((20000.0 / ((2 + col) * 0.5)) - (100 * num2)) / ((double) num2)));
            if (num3 < 0)
            {
                num3 = 0;
            }
            return num3;
        }
    }
}

