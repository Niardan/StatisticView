using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StatisticsViewer.XamlElement
{
    /// <summary>
    /// Логика взаимодействия для BlockControl.xaml
    /// </summary>
    public partial class BlockControl : UserControl
    {
        private readonly BlockControl _baseBlock;
        public BlockControl(BlockControl baseBlock)
        {
            InitializeComponent();
            _baseBlock = baseBlock;
        }

        public BlockControl BaseBlock
        {
            get { return _baseBlock; }
        }

        public void AddElement(UIElement element, bool parse)
        {
            if (!parse && BlockPanel.Children.Count > 0)
            {
                BlockPanel.Children.Add(new ComboControll());
            }
            BlockPanel.Children.Add(element);
        }

        public string GetFilter()
        {
            string str = "( ";
            foreach (var item in BlockPanel.Children)
            {
                switch (item)
                {
                    case BlockControl block:
                        str += block.GetFilter() + " ";
                        break;
                    case ComboControll block:
                        str += block.GetFilter() + " ";
                        break;
                    case Condition block:
                        str += block.GetFilter() + " ";
                        break;
                }
            }

            str += ")";
            return str;
        }
    }
}
