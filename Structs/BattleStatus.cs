using System.Runtime.InteropServices;
using Reloaded.Memory.Pointers;

namespace VelvetControl.Structs;

[StructLayout(LayoutKind.Explicit)]
internal struct BattleStatus
{
    [FieldOffset(0x0)] internal ushort HP; 
    [FieldOffset(0x4)] internal ushort SP; 
    [FieldOffset(0xC)] internal ushort Level; 
    [FieldOffset(0x8)] internal uint Ailments; 
    [FieldOffset(0x10)] internal ushort Exp; 
    [FieldOffset(0x18)] internal uint BuffStatus; 
    [FieldOffset(0x1C)] internal BuffDirectionDuration BuffDirection; 
    [FieldOffset(0x26)] internal BuffDirectionDuration BuffDuration; 

    [FieldOffset(0x38)]
    internal PersonaStruct Persona; 
    
    [FieldOffset(0x278)] internal ushort MeleeWeapon; 
    [FieldOffset(0x27A)] internal ushort Armor; 
    [FieldOffset(0x27C)] internal ushort Accessory; 
    [FieldOffset(0x27E)] internal ushort Outfit; 
    [FieldOffset(0x280)] internal ushort RangedWeapon; 
    
    [FieldOffset(0x290)] internal ushort BonusHP; 
    [FieldOffset(0x292)] internal ushort BonusSP; 
    [FieldOffset(0x288)] internal byte Bullets;
    [FieldOffset(0x29F)] internal byte LastByte;
}

internal struct BuffDirectionDuration
{
    internal byte AtkAcc;
    internal byte DefEva;
    internal byte Crit;
    internal byte SuscAffFire;
    internal byte AffIceWind;
    internal byte AffElecNuke;
    internal byte AffPsyResFire;
    internal byte ResIceElec;
    internal byte ResWindNuke;
    internal byte ResPsy;
}



internal struct Ailments
{
    internal bool Burn;
    internal bool Freeze;
    internal bool Shock;
    internal bool Dizzy;
    internal bool Confuse;
    internal bool Fear;
    internal bool Forget;
    internal bool Hunger;
    internal bool Sleep;
    internal bool Rage;
    internal bool Despair;
    internal bool Brainwash;
    internal bool Desperation;
    internal bool Panic;
    internal bool Lust;
    internal bool Wrath;
    internal bool Envy;
    internal bool Susceptible;
    internal bool Dead;
    internal bool Down;
    internal bool Rattled;
}

internal struct BuffStatus
{
    internal bool Attack;
    internal bool Accuracy;
    internal bool Defense;
    internal bool Evasion;
    internal bool Critical;
    internal bool Critical2;
    internal bool Susceptibility;
    internal bool AfftyFire;
    internal bool AfftyIce;
    internal bool AfftyWind;
    internal bool AfftyElec;
    internal bool AfftyNuke;
    internal bool AfftyPsy;
    internal bool ResistFire;
    internal bool ResistIce;
    internal bool ResistElec;
    internal bool ResistWind;
    internal bool ResistNuke;
    internal bool ResistPsy;
    internal bool ResistPhysical;
    internal bool ResistMagic;
    internal bool Charge;
    internal bool Concentrate;
    internal bool ResistInstakill;
}