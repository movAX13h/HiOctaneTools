using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelEditor.Games.HiOctane.Resources
{
    public class EntityItem : TableItem
    {
        public byte Type { get { return Bytes[0]; } }
        public byte SubType { get { return Bytes[1]; } }
        public int NextID { get { return (Bytes[13] << 8) | Bytes[12]; } }
        public float X { get { return Bytes[16] / 256f + Bytes[17]; } }
        public float Y = 0; // set by level loader
        public float Z { get { return Bytes[18] / 256f + Bytes[19]; } }

        public Vector3 Pos { get { return new Vector3(X, Y, Z); } }
        public Vector3 Center { get { return Pos + 0.5f * Vector3.One; } }


        public int Group { get { return Bytes[2] + Bytes[3] * 256; } }
        public int TargetGroup { get { return Bytes[4] + Bytes[5] * 256; } }
        public int Value { get { return Bytes[14] + Bytes[15] * 256; } }
        public float OffsetX { get { return Bytes[20] + Bytes[21] * 256f; } }
        public float OffsetY { get { return Bytes[22] + Bytes[23] * 256f; } }


        public enum EntityType
        {
            Unknown, UnknownShieldItem, UnknownItem,
            ExtraShield, ShieldFull, DoubleShield,
            ExtraAmmo, AmmoFull, DoubleAmmo,
            ExtraFuel, FuelFull, DoubleFuel,
            MinigunUpgrade, MissileUpgrade, BoosterUpgrade,
            WallSegment,
            WaypointFuel, WaypointAmmo, WaypointShield, WaypointSpecial1, WaypointSpecial2, WaypointSpecial3, WaypointFast, WaypointSlow, WaypointShortcut,
            RecoveryTruck,
            SteamStrong, SteamLight, Cone, Checkpoint,
            MorphSource1, MorphSource2, MorphOnce, MorphPermanent,
            TriggerCraft, TriggerTimed, TriggerRocket,
            DamageCraft,
            Explosion, ExplosionParticles
        }
        public EntityType GameType { get; private set; }

        public EntityItem(int id, int offset, byte[] bytes)
        {
            ID = id;
            Bytes = bytes;
            Offset = offset;

            UpdateGameType();
        }

        public void UpdateGameType()
        {
            GameType = identify();
        }


        private EntityType identify()
        {
            if (Type == 1 && SubType == 5) return EntityType.Checkpoint;

            if (Type == 2 && SubType == 1) return EntityType.ExplosionParticles; // see level 4
            if (Type == 2 && SubType == 2) return EntityType.DamageCraft; // see level 8
            if (Type == 2 && SubType == 3) return EntityType.Explosion;
            if (Type == 2 && SubType == 5) return EntityType.SteamStrong;
            if (Type == 2 && SubType == 7) return EntityType.MorphSource2;
            if (Type == 2 && SubType == 8) return EntityType.SteamLight;
            if (Type == 2 && SubType == 9) return EntityType.MorphSource1;
            if (Type == 2 && SubType == 16) return EntityType.MorphOnce;
            if (Type == 2 && SubType == 23) return EntityType.MorphPermanent;

            if (Type == 3 && SubType == 6) return EntityType.Cone;

            if (Type == 5 && SubType == 0) return EntityType.UnknownShieldItem;
            if (Type == 5 && SubType == 1) return EntityType.UnknownItem;

            if (Type == 5 && SubType == 2) return EntityType.ExtraShield;
            if (Type == 5 && SubType == 3) return EntityType.ShieldFull;
            if (Type == 5 && SubType == 4) return EntityType.DoubleShield;
            if (Type == 5 && SubType == 5) return EntityType.ExtraAmmo;
            if (Type == 5 && SubType == 6) return EntityType.AmmoFull;
            if (Type == 5 && SubType == 7) return EntityType.DoubleAmmo;
            if (Type == 5 && SubType == 8) return EntityType.ExtraFuel;
            if (Type == 5 && SubType == 9) return EntityType.FuelFull;
            if (Type == 5 && SubType == 10) return EntityType.DoubleFuel;
            if (Type == 5 && SubType == 11) return EntityType.MinigunUpgrade;
            if (Type == 5 && SubType == 12) return EntityType.MissileUpgrade;
            if (Type == 5 && SubType == 13) return EntityType.BoosterUpgrade;

            if (Type == 8 && SubType == 0) return EntityType.TriggerCraft;
            if (Type == 8 && SubType == 1) return EntityType.TriggerTimed;
            if (Type == 8 && SubType == 3) return EntityType.TriggerRocket;

            if (Type == 9 && SubType == 0) return EntityType.WallSegment;

            if (Type == 9 && SubType == 1) return EntityType.WaypointSlow;
            if (Type == 9 && SubType == 2) return EntityType.WaypointFuel;
            if (Type == 9 && SubType == 3) return EntityType.WaypointAmmo;
            if (Type == 9 && SubType == 4) return EntityType.WaypointShield;
            if (Type == 9 && SubType == 6) return EntityType.WaypointShortcut;
            if (Type == 9 && SubType == 7) return EntityType.WaypointSpecial1;
            if (Type == 9 && SubType == 8) return EntityType.WaypointSpecial2;
            if (Type == 9 && SubType == 9) return EntityType.WaypointFast;
            if (Type == 9 && SubType == 10) return EntityType.WaypointSpecial3;

            if (Type == 10 && SubType == 9) return EntityType.RecoveryTruck;

            return EntityType.Unknown;
        }
        public override bool WriteChanges()
        {
            return false;
        }

        public string ToJSON()
        {
            return "{\"i\":" + ID + "," + 
                    "\"t\":\"" + GameType.ToString() + "\"," +
                    "\"n\":" + NextID + "," +
                    "\"x\":" + X + "," +
                    "\"z\":" + Z + "," +
                    "\"g\":" + Group + "," +
                    "\"l\":" + TargetGroup + "," +
                    "\"v\":" + Value + "," +
                    "\"ox\":" + OffsetX + "," +
                    "\"oy\":" + OffsetY +
                   "}";
        }
    }
}
