using System.Windows.Controls;

namespace MHW_Save_Editor.InventoryEditing
{
    public partial class InventoryAreas : UserControl
    {
        public InventoryAreas()
        {
            InitializeComponent();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = new InventoryAreasViewModel();
            }
        }
    }
}
