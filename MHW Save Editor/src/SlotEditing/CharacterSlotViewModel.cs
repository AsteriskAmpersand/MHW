using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using MHW_Save_Editor.SaveSlot;

namespace MHW_Save_Editor.SlotEditing
{
    public enum GenderEnum {
        Male = 0, 
        Female = 1
    }
    
    public class CharacterSlotViewModel
    {
        private CharacterSlot charslot;
        public CharacterSlotViewModel(CharacterSlot charslot)
        {
            this.charslot = charslot;
        }

        public string HunterName{ get => charslot.HunterName; set => charslot.HunterName = value; }
        public string PalicoName { get => charslot.PalicoName; set => charslot.PalicoName = value; }
        public UInt32 HunterRank { get => charslot.HunterRank; set => charslot.HunterRank = value; }
        public UInt32 Zenny  { get => charslot.Zenny; set => charslot.Zenny = value; }
        public UInt32 ResearchPoints { get => charslot.ResearchPoints; set => charslot.ResearchPoints = value; }
        public UInt32 HunterXP { get => charslot.HunterXP; set => charslot.HunterXP = value; }
        public GenderEnum Gender{ get => charslot.Gender==0?GenderEnum.Male:GenderEnum.Female; set => charslot.Gender = (UInt32) (value==GenderEnum.Male?0:1);}
        
        public Int32 seconds { 
                get => (Int32) (charslot.PlayTime) % 60; 
                set => charslot.PlayTime = charslot.PlayTime - (charslot.PlayTime % 60 - (UInt32) value);
        }
        public Int32 minutes
        {
            get => (Int32) (charslot.PlayTime / (60)) % 60; 
            set => charslot.PlayTime = charslot.PlayTime - (charslot.PlayTime / (60) % 60 - (UInt32) value)*60;
        }
        public Int32 hours
        {
            get => (Int32) (charslot.PlayTime / (60 * 60)); 
            set => charslot.PlayTime = charslot.PlayTime - (charslot.PlayTime / (60*60) - (UInt32) value)*60*60;
        }

        
        //public HunterAppearance HunterAppearance;
        //public PalicoAppearance PalicoAppearance;
        //public GuildCard GuildCard;
        //public List<GuildCard> SharedGC;
        //public List<ItemLoadout> ItemLoadouts;
        //public List<InventoryArea> ItemBox;
        //public List<Investigation> InvestigationList;
        //public List<EquipLoadout> EquipmentLoadouts;
        public List<CharacterSlot.DLC> DLCClaimed;

        public void Commit(byte[] savefile, int offset)
        {
            charslot.Serialize().CopyTo(savefile,offset);
            BitConverter.GetBytes(charslot.Gender).CopyTo(savefile, offset + 0xb0);

            // Save Palico name
            charslot.SerializePalico().CopyTo(savefile, offset + 0xDA671);
        }
    }
    
    public class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }
}