using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MHW_Save_Editor.InventoryEditing;
using MHW_Save_Editor.InvestigationEditing;

namespace MHW_Save_Editor.SaveSlot
{
    public class CharacterSlot
    {
        public static readonly Int32 SaveSlotOffset = 0x003004DC;
        public static readonly Int32 SaveSize = 0xF6110;

        public CharacterSlot(byte[] charslot)
        {
            GetSlotData(charslot);
            DLCClaimed = GetDLC(charslot);
        }

        private byte[] _HunterName { get; set; }
        public string HunterName { get => _HunterName.DecodeUTF8(); set => _HunterName = value.ToFixedSizeCharArray(64); }

        private byte[] _PalicoName { get; set; }
        public string PalicoName { get => _PalicoName.DecodeUTF8(); set => _PalicoName = value.ToFixedSizeCharArray(16); }

        public UInt32 HunterRank;
        public UInt32 Zenny;
        public UInt32 ResearchPoints;
        public UInt32 HunterXP;
        public UInt32 PlayTime;
        public UInt32 Gender;
        //public HunterAppearance HunterAppearance;
        //public PalicoAppearance PalicoAppearance;
        //public GuildCard GuildCard;
        //public List<GuildCard> SharedGC;
        //public List<ItemLoadout> ItemLoadouts;
        //public List<InventoryArea> ItemBox;
        //public List<Investigation> InvestigationList;
        //public List<EquipLoadout> EquipmentLoadouts;
        public List<DLC> DLCClaimed;

        private void GetSlotData(byte[] newdata)
        {
            int i = 0;
            int pIndex = 0xDA671;
            _HunterName = newdata.Slice(0, 64); i+=64;
            _PalicoName = newdata.Slice(pIndex, pIndex + 16);
            HunterRank = BitConverter.ToUInt32(newdata,i);i+=4;
            Zenny = BitConverter.ToUInt32(newdata,i);i+=4;
            ResearchPoints = BitConverter.ToUInt32(newdata,i);i+=4;
            HunterXP = BitConverter.ToUInt32(newdata,i);i+=4;
            PlayTime = BitConverter.ToUInt32(newdata,i);

            Gender = BitConverter.ToUInt32(newdata, 0xb0);
        }

        private List<DLC> GetDLC(byte[] newdata)
        {
            List<DLC>dlc = new List<DLC>();
            for (int i = 0; i < dlccount; i++)
            {
                dlc.Add((DLC)BitConverter.ToUInt16(newdata,dlclocaloffset+i*2));
            }
            return dlc;
        }

        public byte[] SerializePalico()
        {
            return _PalicoName;
        }
        public byte[] Serialize()
        {
            return
                _HunterName.
                Concat(BitConverter.GetBytes(HunterRank)).
                Concat(BitConverter.GetBytes(Zenny)).
                Concat(BitConverter.GetBytes(ResearchPoints)).
                Concat(BitConverter.GetBytes(HunterXP)).
                Concat(BitConverter.GetBytes(PlayTime)).ToArray();
        }

        public static readonly Int32 dlclocaloffset = 0xf34b3;
        public static readonly Int32 dlccount = 256;
        public enum DLC{
            None                                      = 0,
            Origin_Armor_Set                          = 1,
            Fair_Wind_Charm                           = 2,
            Samurai_Set                               = 3,
            The_Handlers_Guildmarm_Costume            = 4,
            The_Handlers_Astera_3_Star_Chef_Coat      = 5,
            The_Handlers_Busy_Bee_Dress               = 7,
            The_Handlers_Sunshine_Pareo               = 8,
            The_Handlers_Mischievous_Dress            = 9,
            Beginner_Commendation_Pack                = 12,
            Intermediate_Commendation_Pack            = 13,
            Expert_Commendation_Pack                  = 14,
            Champion_Commendation_Pack                = 15,
            Character_Edit_Voucher                    = 20,
            Face_Paint_Camouflage                     = 21,
            Face_Paint_Wyvern                         = 22,
            Face_Paint_Shade_Pattern                  = 23,
            Face_Paint_Heart_Shape                    = 24,
            Face_Paint_Eye_Shadow                     = 25,
            Hairstyle_Topknot                         = 26,
            Hairstyle_Provisions_Manager              = 27,
            Hairstyle_Field_Team_Leader               = 28,
            Hairstyle_The_Handler                     = 29,
            Hairstyle_The_Admiral                     = 30,
            Gesture_Zen                               = 31,
            Gesture_Ninja_Star                        = 32,
            Gesture_Sumo_Slap                         = 33,
            Gesture_Passionate                        = 34,
            Gesture_SpinORama                         = 35,
            Gesture_Air_Splits                        = 36,
            Gesture_Feverish_Dance                    = 37,
            Gesture_Gallivanting_Dance                = 38,
            Gesture_Interpretive_Dance                = 39,
            Gesture_Play_Possum                       = 40,
            Gesture_Kowtow                            = 41,
            Gesture_Sleep                             = 42,
            Gesture_Kneel                             = 43,
            Classic_Gesture_Dance                     = 44,
            Classic_Gesture_Prance                    = 45,
            Classic_Gesture_Rant                      = 46,
            Classic_Gesture_Clap                      = 47,
            Gesture_Devil_May_Cry_Dual_Guns           = 50,
            Gesture_Spirit_Fingers                    = 52,
            Gesture_Windmill_Whirlwind                = 53,
            Gesture_Disco_Fever                       = 54,
            Gesture_Squat_Day                         = 55,
            Sticker_Set_MH_All_Stars_Set              = 56,
            Sticker_Set_Sir_Loin_Set                  = 57,
            Sticker_Set_Poogie_Set                    = 58,
            Sticker_Set_Guild_Lasses_Set              = 59,
            Sticker_Set_Endemic_Life_Set              = 60,
            Sticker_Set_Classic_Monsters_Set          = 61,
            Sticker_Set_Research_Commission_Set       = 62,
            Sticker_Set_Mega_Man_Set                  = 63,
            Sticker_Set_Devil_May_Cry_Set             = 65,
            Character_Edit_Voucher_Single_Voucher     = 99,
            Character_Edit_Voucher_Two_Voucher_Pack   = 100,
            Character_Edit_Voucher_Three_Voucher_Pack = 101,
        };
    }
}