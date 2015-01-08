namespace Client.Targeting
{
    using Client;
    using System;

    public class MultiTargetHandler : ITargetHandler
    {
        private int m_TargetID;

        public MultiTargetHandler(int targetID)
        {
            this.m_TargetID = targetID;
        }

        public void OnCancel(TargetCancelType type)
        {
            Engine.m_MultiPreview = false;
            if (type == TargetCancelType.UserCancel)
            {
                Network.Send(new PMultiTarget_Cancel(this.m_TargetID));
            }
        }

        public void OnTarget(object o)
        {
            if (o is LandTarget)
            {
                Engine.m_MultiPreview = false;
                LandTarget target = (LandTarget) o;
                Network.Send(new PMultiTarget_Response(this.m_TargetID, target.X, target.Y, target.Z, 0));
            }
            else if (o is StaticTarget)
            {
                Engine.m_MultiPreview = false;
                StaticTarget target2 = (StaticTarget) o;
                Network.Send(new PMultiTarget_Response(this.m_TargetID, target2.X, target2.Y, target2.Z + Map.GetHeight(target2.ID), target2.ID & 0x3fff));
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}

