namespace Client
{
    using System;

    public class CommandInfoProvider : InfoProvider
    {
        public CommandInfoProvider() : base("Commands")
        {
        }

        public override Gump CreateGump()
        {
            return new GHtmlLabel(this.GetText(), Engine.GetUniFont(1), GumpColors.WindowText, 0, 0, 380);
        }

        public override InfoInput[] CreateInputs()
        {
            InfoNode[] nodes = new InfoNode[4];
            nodes[0] = new InfoNode("Actions", new InfoNode[] { new CommandInfoNode("AttackReds", "This command attacks all non-human reds on screen."), new CommandInfoNode("Disarm", "This command performs the disarm wrestling move."), new CommandInfoNode("IBanThee", "This command will quickly identify and ban players or pets from your house."), new CommandInfoNode("Remove", "This command removes a targeted item or player from the client screen. Use the Resynchronize macro to refresh the screen of any removed objects."), new CommandInfoNode("Stun", "This command performs the stun wrestling move."), new CommandInfoNode("TurnTo", "This command turns the avatar to face what is targeted."), new CommandInfoNode("UpTarg", "This command targets the highest object above you. It can be used to easily teleport onto building roofs.") });
            InfoNode[] children = new InfoNode[2];
            InfoNode[] nodeArray3 = new InfoNode[5];
            nodeArray3[0] = new CommandInfoNode("DefaultRegs", "This command changes the default number of reagents used in the MakeRegs command for your character.", new string[,] { { "Amount", "Amount of desired reagents." } });
            string[,] parms = new string[2, 2];
            parms[0, 1] = "The default amount, specified by the DefaultRegs command (or, if that's unset, 125), is used.";
            parms[1, 0] = "Amount";
            parms[1, 1] = "That amount of reagents is used.";
            nodeArray3[1] = new CommandInfoNode("MakeRegs", "This command makes your total number of reagents a specified value. It does this by either dropping extra reagents to or getting extra reagents from a stock container. In the process, all reagents are stacked in your reagent container.", parms);
            nodeArray3[2] = new CommandInfoNode("RegBag", "This command changes the destination reagent container used in the MakeRegs command for your character.");
            nodeArray3[3] = new CommandInfoNode("RegDrop", "This command moves all reagents in your backpack to a targeted container.");
            nodeArray3[4] = new CommandInfoNode("RegStock", "This command changes the source reagent container used in the MakeRegs command for your character.");
            children[0] = new InfoNode("Reagents", nodeArray3);
            nodeArray3 = new InfoNode[7];
            parms = new string[2, 2];
            parms[0, 1] = "No offsets are used.";
            parms[1, 0] = "X Y";
            parms[1, 1] = "The specified offsets are used.";
            nodeArray3[0] = new CommandInfoNode("BringTo", "This command brings all matching items in your backpack to a targeted item. Optional offset values can be used to give a better view of quantity.", parms);
            nodeArray3[1] = new CommandInfoNode("ClearMoves", "This command clears the item movement queue which is used in commands like DragToBag, Move, and RegDrop.");
            nodeArray3[2] = new CommandInfoNode("DragToBag", "This command attempts to drag a targeted item to your backpack.");
            nodeArray3[3] = new CommandInfoNode("Leapfrog", "This command will automatically move the currently dragged item as your avatar moves.");
            nodeArray3[4] = new CommandInfoNode("Loot", "This command searches nearby corpses and attempts to loot the best items.");
            nodeArray3[5] = new CommandInfoNode("Move", "This command moves all items matching one targeted item to a targeted container. For example, it can be used to move potions into potion kegs.");
            nodeArray3[6] = new CommandInfoNode("Stack", "This command stacks all matching items in your backpack to a targeted item.");
            children[1] = new InfoNode("Generic", nodeArray3);
            nodes[1] = new InfoNode("Item Movers", children);
            children = new InfoNode[13];
            children[0] = new CommandInfoNode("Ack", "This command sets the maximum number of outstanding movement requests. If exceeded the client will stop and wait for a server response.", new string[,] { { "Value", "Maximum number to set." } });
            children[1] = new CommandInfoNode("AltFont", "This command enables or disables an alternate speech font.");
            children[2] = new CommandInfoNode("Archery", "This command enables or disables picking up or looting arrows.");
            children[3] = new CommandInfoNode("HouseLevel", "This command changes the level to which houses are collapsed when you are not inside them.", new string[,] { { "0, 5, or off", "Houses are not collapsed." }, { "1..4", "Houses are collapsed so that the specified floor is shown." }, { "Up", "The next floor higher is shown." }, { "Down", "The next floor lower is shown." } });
            children[4] = new CommandInfoNode("LootGold", "This command enables or disables looting gold from corpses.");
            parms = new string[4, 2];
            parms[0, 1] = "Music is toggled on or off.";
            parms[1, 0] = "On";
            parms[1, 1] = "Music is enabled.";
            parms[2, 0] = "Off";
            parms[2, 1] = "Music is disabled.";
            parms[3, 0] = "Stop";
            parms[3, 1] = "The currently playing music is stopped.";
            children[5] = new CommandInfoNode("Music", "This command controls music.", parms);
            parms = new string[4, 2];
            parms[0, 1] = "Notoriety query is toggled on or off.";
            parms[1, 0] = "On";
            parms[1, 1] = "Notoriety query is enabled.";
            parms[2, 0] = "Off";
            parms[2, 1] = "Notoriety query is disabled.";
            parms[3, 0] = "Smart";
            parms[3, 1] = "Notoriety query is changed to 'smart' mode. In this mode notoriety query is invoked only if attempting to harm an innocent when either they or you are in town. Notoriety query is disabled when out of town.";
            children[6] = new CommandInfoNode("Noto", "This command enables or disables notoriety query.", parms);
            parms = new string[3, 2];
            parms[0, 1] = "Target queueing is toggled on or off.";
            parms[1, 0] = "On";
            parms[1, 1] = "Target queueing is enabled.";
            parms[2, 0] = "Off";
            parms[2, 1] = "Target queueing is disabled.";
            children[7] = new CommandInfoNode("QueueTargets", "This command enables or disables target queueing.", parms);
            children[8] = new CommandInfoNode("RunSpeed", "This command sets the base speed of running.", new string[,] { { "Value", "Delay to set, in milliseconds." } });
            parms = new string[3, 2];
            parms[0, 1] = "Smooth movement is toggled on or off.";
            parms[1, 0] = "On";
            parms[1, 1] = "Smooth movement is enabled.";
            parms[2, 0] = "Off";
            parms[2, 1] = "Smooth movement is disabled.";
            children[9] = new CommandInfoNode("SmoothWalk", "This command enables or disables smooth movement.", parms);
            children[10] = new CommandInfoNode("Volume", "This command opens a menu allowing sound and music volume to be adjusted.");
            children[11] = new CommandInfoNode("WalkSpeed", "This command sets the base speed of walking.", new string[,] { { "Value", "Delay to set, in milliseconds." } });
            parms = new string[3, 2];
            parms[0, 1] = "Weather is toggled on or off.";
            parms[1, 0] = "On";
            parms[1, 1] = "Weather is enabled.";
            parms[2, 0] = "Off";
            parms[2, 1] = "Weather is disabled.";
            children[12] = new CommandInfoNode("Weather", "This command enables or disables weather.", parms);
            nodes[2] = new InfoNode("Options", children);
            children = new InfoNode[13];
            parms = new string[3, 2];
            parms[0, 1] = "Always run is toggled on or off.";
            parms[1, 0] = "On";
            parms[1, 1] = "Always run is enabled.";
            parms[2, 0] = "Off";
            parms[2, 1] = "Always run is disabled.";
            children[0] = new CommandInfoNode("AlwaysRun", "This command enables, disables, or toggles the 'always run' option.", parms);
            children[1] = new CommandInfoNode("BuyHorse", "This command buys a horse from the nearest animal trainer.");
            children[2] = new CommandInfoNode("CharMacros", "This command creates a macro file which is unique to the current character.");
            children[3] = new CommandInfoNode("Export", "This command generates a text file detailing all items in a targeted area.");
            children[4] = new CommandInfoNode("Friend", "This command toggles whether a targeted player is considered a friend. Players who are considered a friend will have a green icon above their head. Macros can be made to target non-friended players.");
            children[5] = new CommandInfoNode("GuardHP", "This command opens the health bars of all faction guards on screen. It is useful when attacking or defending faction guard turrets.");
            children[6] = new CommandInfoNode("Ignore", "This command toggles whether a targeted player is ignored. Ignored players will appear highlighted yellow.");
            children[7] = new CommandInfoNode("Path", "This command controls movement path recording and playback.", new string[,] { { "Record", "Begins recording the path of your movement." }, { "Stop", "Stops recording the path of your movement." }, { "Run", "Tries to follow the set movement path." }, { "Save", "Saves the last recorded movement path." }, { "Load", "Loads the last saved movement path." } });
            children[8] = new CommandInfoNode("Target", "This command creates a new target which has no effect other than to set the last targeted object.");
            parms = new string[2, 2];
            parms[0, 1] = "Renders 100 frames.";
            parms[1, 0] = "Count";
            parms[1, 1] = "Renders that many frames. Higher numbers can be used to give a more accurate FPS.";
            children[9] = new CommandInfoNode("TimeRefresh", "This command renders a sequence of frames and records the time taken to do so. That time is used to calculate FPS, which is then displayed.", parms);
            children[10] = new CommandInfoNode("Trace", "This command outputs details of a targeted item or mobile to Debug.log.");
            children[11] = new CommandInfoNode("UOAM", "This command connects to a specified UOAM server.", new string[,] { { "Name", "Your username." }, { "Password", "The server password." }, { "Address", "The server IP address." }, { "Port", "The server port." } });
            children[12] = new CommandInfoNode("UseGate", "This command uses the nearest moongate.");
            nodes[3] = new InfoNode("Other", children);
            InfoInputTree tree = new InfoInputTree("Commands", nodes);
            return new InfoInput[] { tree };
        }

        private string GetText()
        {
            CommandInfoNode active = base.Inputs[0].Active as CommandInfoNode;
            if (active == null)
            {
                return "Select a command from the list above.";
            }
            return string.Format("<center><u>{0}</u></center><br><br>{1}", active.Name, active.Description);
        }

        public override void UpdateGump(Gump g)
        {
            GHtmlLabel label = (GHtmlLabel) g;
            label.Children.Clear();
            label.Update(this.GetText(), Engine.GetUniFont(1), GumpColors.WindowText);
        }
    }
}

