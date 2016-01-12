namespace Client
{
    public class BBMessageHeader
    {
        private Item m_Board;
        private string m_Poster;
        private string m_Subject;
        private Item m_Thread;
        private string m_Time;

        public BBMessageHeader(Item board, Item thread, string poster, string subject, string time)
        {
            this.m_Board = board;
            this.m_Thread = thread;
            this.m_Poster = poster;
            this.m_Subject = subject;
            this.m_Time = time;
        }
    }
}