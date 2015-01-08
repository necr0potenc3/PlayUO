namespace Client
{
    using System;

    public interface IMobileStatus
    {
        void Close();
        void OnArmorChange(int Armor);
        void OnColdChange();
        void OnDamageChange();
        void OnDexChange(int Dex);
        void OnEnergyChange();
        void OnFireChange();
        void OnFlagsChange(MobileFlags Flags);
        void OnFollCurChange(int current);
        void OnFollMaxChange(int maximum);
        void OnGenderChange(int Gender);
        void OnGoldChange(int Gold);
        void OnHPCurChange(int HPCur);
        void OnHPMaxChange(int HPMax);
        void OnIntChange(int Int);
        void OnLuckChange();
        void OnManaCurChange(int ManaCur);
        void OnManaMaxChange(int ManaMax);
        void OnNameChange(string Name);
        void OnNotorietyChange(Notoriety n);
        void OnPoisonChange();
        void OnRefresh();
        void OnStamCurChange(int StamCur);
        void OnStamMaxChange(int StamMax);
        void OnStatCapChange(int statCap);
        void OnStrChange(int Str);
        void OnWeightChange(int Weight);

        Client.Gump Gump { get; }
    }
}

