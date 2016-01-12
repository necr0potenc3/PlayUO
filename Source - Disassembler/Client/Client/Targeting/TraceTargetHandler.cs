namespace Client.Targeting
{
    using Client;
    using System.Windows.Forms;

    public class TraceTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Trace request canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is StaticTarget)
            {
                Engine.AddTextMessage(((StaticTarget)o).Hue.ToString());
            }
            else if (o is Item)
            {
                Item item = (Item)o;
                Engine.AddTextMessage(string.Format("Hue: {0}", item.Hue));
                Clipboard.SetDataObject(string.Format("new Point3D( {0}, {1}, {2} ),", item.X, item.Y, item.Z));
                Debug.Trace(string.Format("new Point3D( {0}, {1}, {2} ),", item.X, item.Y, item.Z));
            }
            else if (o is Mobile)
            {
                ((Mobile)o).Trace();
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}