using System.Windows;
using System.Windows.Controls;

namespace StatisticsViewer.XamlElement
{
    /// <summary>
    /// Логика взаимодействия для BlockControl.xaml
    /// </summary>
    public delegate void BlockControlHandler(BlockControl sender);

    public partial class BlockControl : UserControl
    {
        private readonly BlockControl _baseBlock;

        public event BlockControlHandler Deleted;
        public event BlockControlHandler Changed;

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
            Changed?.Invoke(this);
        }

        public void DeleteElement(UIElement element)
        {
            int index = BlockPanel.Children.IndexOf(element);
            if (index > 0)
            {
                var combo = BlockPanel.Children[index - 1];
                BlockPanel.Children.Remove(combo);
                BlockPanel.Children.Remove(element);
            }
            else if (index == 0)
            {
                if (BlockPanel.Children.Count == 1)
                {
                    Deleted?.Invoke(this);
                }
                else
                {
                    var combo = BlockPanel.Children[index + 1];
                    BlockPanel.Children.Remove(combo);
                    BlockPanel.Children.Remove(element);
                }
            }
            Changed?.Invoke(this);
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
