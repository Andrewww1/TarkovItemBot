﻿using System.ComponentModel;
using System.Text.Json.Serialization;

namespace TarkovItemBot.Services
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ItemKind
    {
        [Description("None")]
        None,
        [KindType(typeof(AmmunitionItem))]
        Ammunition,
        [KindType(typeof(ArmorItem))]
        Armor,
        [KindType(typeof(BackpackItem))]
        Backpack,
        [KindType(typeof(BarterItem))]
        Barter,
        [KindType(typeof(ClothingItem))]
        Clothing,
        [KindType(typeof(BaseItem))]
        Common,
        [KindType(typeof(ContainerItem))]
        Container,
        [KindType(typeof(FirearmItem))]
        Firearm,
        [KindType(typeof(FoodItem))]
        Food,
        [KindType(typeof(GrenadeItem))]
        Grenade,
        [KindType(typeof(HeadphoneItem))]
        Headphone,
        [KindType(typeof(KeyItem))]
        Key,
        [KindType(typeof(MagazineItem))]
        Magazine,
        [KindType(typeof(MapItem))]
        Map,
        [KindType(typeof(MedicalItem))]
        Medical,
        [KindType(typeof(MeleeItem))]
        Melee,
        [KindType(typeof(ModificationItem))]
        Modification,
        [KindType(typeof(BarrelItem))]
        [Description("Barrel")]
        ModificationBarrel,
        [KindType(typeof(BipodItem))]
        [Description("Bipod")]
        ModificationBipod,
        [KindType(typeof(ChargeItem))]
        [Description("Charging Handle")]
        ModificationCharge,
        [KindType(typeof(DeviceItem))]
        [Description("Tactical Device")]
        ModificationDevice,
        [KindType(typeof(ForegripItem))]
        [Description("Foregrip")]
        ModificationForegrip,
        [KindType(typeof(GasblockItem))]
        [Description("Gasblock")]
        ModificationGasblock,
        [KindType(typeof(GogglesItem))]
        [Description("Goggles")]
        ModificationGoggles,
        [KindType(typeof(HandguardItem))]
        [Description("Handguard")]
        ModificationHandguard,
        [KindType(typeof(LauncherItem))]
        [Description("Launcher")]
        ModificationLauncher,
        [KindType(typeof(MountItem))]
        [Description("Mount")]
        ModificationMount,
        [KindType(typeof(MuzzleItem))]
        [Description("Muzzle")]
        ModificationMuzzle,
        [KindType(typeof(PistolgripItem))]
        [Description("Pistolgrip")]
        ModificationPistolgrip,
        [KindType(typeof(ReceiverItem))]
        [Description("Receiver")]
        ModificationReceiver,
        [KindType(typeof(SightItem))]
        [Description("Sight")]
        ModificationSight,
        [KindType(typeof(SightSpecialItem))]
        [Description("Special Sight")]
        ModificationSightSpecial,
        [KindType(typeof(StockItem))]
        [Description("Stock")]
        ModificationStock,
        [KindType(typeof(MoneyItem))]
        Money,
        [KindType(typeof(TacticalrigItem))]
        Tacticalrig
    }
}
