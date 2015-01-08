namespace Client
{
    using System;
    using System.Drawing;

    public interface IExposed
    {
        void Ability(string type);
        void AddTextMessage(string TextMessage);
        void AddTextMessage(string TextMessage, TimeSpan Length);
        void AddTextMessage(string TextMessage, TimeSpan Length, int WhichFont);
        void AllNames();
        bool Attack(string what);
        void AutoUse();
        bool BandageSelf();
        void CastSpell(int SpellID);
        void CastSpell(string Name);
        void ClearLastTarget();
        void ClearScreen();
        void ClearTargetQueue();
        void Count(string what);
        bool DelayMacro(string args);
        void Dequip();
        void Dismount();
        void DoAction(string Action);
        void Equip(int index);
        Bitmap GetGump(int GumpID, int HueUID);
        Bitmap GetItem(int ItemID, int HueUID);
        Bitmap GetLand(int LandID, int HueUID);
        void Last(string what);
        void Open(string What);
        void Paste();
        void Paste(string ToPaste);
        void Quit();
        void RegisterAsMacro(string action);
        void RegisterAsMacro(string action, params ParamNode[] options);
        void RegisterAsMacro(string action, params string[] list);
        void RegisterAsMacro(string action, string[,] list);
        void Remount();
        void RepeatMacro();
        void RepeatSpeech();
        bool Resync();
        void Say(string Text);
        void SetAutoUse();
        void SetEquip(int index);
        void SetText(string text);
        bool SmartPotion();
        void StopMacros();
        void Target(string What);
        void UseItemByType(int[] itemIDs);
        bool UsePotion(PotionType type);
        void UseSkill(string Name);
        bool WrestleMove(WrestleType type);

        bool CanTargetLast { get; }

        bool ContainerGrid { get; set; }

        bool FPS { get; set; }

        bool Grid { get; set; }

        bool Halos { get; set; }

        bool HasTarget { get; }

        bool LastTargetIsDynamic { get; }

        bool LastTargetIsLand { get; }

        bool LastTargetIsMobile { get; }

        bool LastTargetIsSelf { get; }

        bool LastTargetIsStatic { get; }

        bool MiniHealth { get; set; }

        bool ParticleCount { get; set; }

        bool Ping { get; set; }

        int Player { get; }

        bool PumpFPS { get; set; }

        bool RegCounter { get; set; }

        bool Screenshots { get; set; }

        bool Temperature { get; set; }

        bool Transparency { get; set; }

        bool Warmode { get; set; }
    }
}

