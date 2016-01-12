namespace Client
{
    using Microsoft.DirectX;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public class Debug
    {
        public static Benchmark m_Benchmark = new Benchmark(7);
        private static int m_Indent;
        private static StreamWriter m_Logger;
        private static bool m_Time;

        public static void Block(string Name)
        {
            Trace("{0}..", Name);
            Indent++;
        }

        [DebuggerHidden]
        public static void Break()
        {
            Debugger.Break();
        }

        public static void Dispose()
        {
            if (m_Logger != null)
            {
                m_Logger.Flush();
                m_Logger.Close();
                m_Logger = null;
            }
            if (m_Benchmark != null)
            {
                m_Benchmark = null;
            }
        }

        public static void EndBlock()
        {
            if (m_Time)
            {
                m_Benchmark.StopNoLog();
                m_Time = false;
                EndTry("( {0} )", Benchmark.Format(m_Benchmark.Elapsed));
            }
            else
            {
                Indent--;
                Trace("Done");
            }
        }

        public static void EndTry()
        {
            GetLogger();
            m_Logger.WriteLine("done");
        }

        public static void EndTry(string msg)
        {
            GetLogger();
            m_Logger.WriteLine("done {0}", msg);
        }

        public static void EndTry(string Format, object Obj0)
        {
            EndTry(string.Format(Format, Obj0));
        }

        public static void EndTry(string Format, params object[] Params)
        {
            EndTry(string.Format(Format, Params));
        }

        public static void EndTry(string Format, object Obj0, object Obj1)
        {
            EndTry(string.Format(Format, Obj0, Obj1));
        }

        public static void EndTry(string Format, object Obj0, object Obj1, object Obj2)
        {
            EndTry(string.Format(Format, Obj0, Obj1, Obj2));
        }

        public static void Error(Exception ex)
        {
            if (ex is DirectXException)
            {
                DirectXException exception = (DirectXException)ex;
                Trace("Error Code -> {0}", exception.ErrorCode);
                Trace("Error String -> {0}", exception.ErrorString);
            }
            Trace("Type -> {0}", ex.GetType());
            Trace("Message -> {0}", ex.Message);
            Trace("Source -> {0}", ex.Source);
            Trace("Target -> {0}", ex.TargetSite);
            Trace("Inner -> {0}", ex.InnerException);
            Trace("Stack ->");
            Trace(ex.StackTrace);
        }

        public static void Error(string Message)
        {
            StackTrace trace = new StackTrace(true);
            bool flag = false;
            MethodBase base2 = null;
            int frameCount = trace.FrameCount;
            for (int i = 0; i < frameCount; i++)
            {
                MethodBase method = trace.GetFrame(i).GetMethod();
                if ((method.DeclaringType == typeof(Client.Debug)) && (method.Name == "Error"))
                {
                    flag = true;
                }
                else if (flag)
                {
                    base2 = method;
                    break;
                }
            }
            if (base2 == null)
            {
                Print("Error in unknown module:");
                Print(" - {0}", Message.Replace("\n", "\r\n - "));
                Print(" - Stack Trace ->");
                Print(trace.ToString());
                Print();
            }
            else
            {
                Print("Error in '{0}.{1}':", base2.DeclaringType.Name, base2.Name);
                Print(" - {0}", Message.Replace("\n", "\r\n - "));
                Print(" - Stack Trace ->");
                Print(trace.ToString());
                Print();
            }
        }

        public static void FailBlock()
        {
            if (m_Time)
            {
                m_Benchmark.StopNoLog();
                m_Time = false;
                EndTry("( {0} )", Benchmark.Format(m_Benchmark.Elapsed));
            }
            else
            {
                Indent--;
                Trace("Failed");
            }
        }

        public static void FailTry()
        {
            GetLogger();
            m_Logger.WriteLine("failed");
        }

        public static void FailTry(string msg)
        {
            GetLogger();
            m_Logger.WriteLine("failed {0}", msg);
        }

        private static void GetLogger()
        {
            if (m_Logger == null)
            {
                m_Logger = new StreamWriter(Engine.FileManager.CreateUnique("Debug", ".log"));
                m_Logger.AutoFlush = true;
            }
        }

        private static void Print()
        {
            m_Logger.WriteLine();
        }

        private static void Print(string ToWrite)
        {
            GetLogger();
            m_Logger.WriteLine(ToWrite);
        }

        private static void Print(string Format, object Obj0)
        {
            Print(string.Format(Format, Obj0));
        }

        private static void Print(string Format, params object[] Params)
        {
            Print(string.Format(Format, Params));
        }

        private static void Print(string Format, object Obj0, object Obj1)
        {
            Print(string.Format(Format, Obj0, Obj1));
        }

        private static void Print(string Format, object Obj0, object Obj1, object Obj2)
        {
            Print(string.Format(Format, Obj0, Obj1, Obj2));
        }

        public static void TimeBlock(string Name)
        {
            Try(Name);
            m_Time = true;
            m_Benchmark.Start();
        }

        public static void Trace(string Message)
        {
            GetLogger();
            m_Logger.WriteLine(new string(' ', Indent * 3) + Message);
        }

        public static void Trace(string Format, object Obj0)
        {
            Trace(string.Format(Format, Obj0));
        }

        public static void Trace(string Format, params object[] Params)
        {
            Trace(string.Format(Format, Params));
        }

        public static void Trace(string Format, object Obj0, object Obj1)
        {
            Trace(string.Format(Format, Obj0, Obj1));
        }

        public static void Trace(string Format, object Obj0, object Obj1, object Obj2)
        {
            Trace(string.Format(Format, Obj0, Obj1, Obj2));
        }

        public static void Try(string Name)
        {
            GetLogger();
            m_Logger.Write("{0}{1}...", new string(' ', Indent * 3), Name);
        }

        public static void Try(string Format, object Obj0)
        {
            Try(string.Format(Format, Obj0));
        }

        public static void Try(string Format, params object[] Params)
        {
            Try(string.Format(Format, Params));
        }

        public static void Try(string Format, object Obj0, object Obj1)
        {
            Try(string.Format(Format, Obj0, Obj1));
        }

        public static void Try(string Format, object Obj0, object Obj1, object Obj2)
        {
            Try(string.Format(Format, Obj0, Obj1, Obj2));
        }

        public static int Indent
        {
            get
            {
                return m_Indent;
            }
            set
            {
                m_Indent = value;
            }
        }
    }
}