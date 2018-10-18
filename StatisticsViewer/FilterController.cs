using System;
using System.Windows;
using System.Windows.Controls;
using StatisticsViewer.XamlElement;
using Condition = StatisticsViewer.XamlElement.Condition;

namespace StatisticsViewer
{
    public class FilterController
    {
       
        private BlockControl _currentBlock;
        private Parametr _parametr;
        private StackPanel _basePanel;

        public FilterController(Parametr parametr, StackPanel basePanel)
        {
            _basePanel = basePanel;
            _parametr = parametr;
            _currentBlock = Parse(_parametr.Filter);

            if (_currentBlock != null)
            {
                basePanel.Children.Add(_currentBlock);
            }
        }

        public void AddCondition()
        {
            var element = AddConditionElement();
            if (element != null)
            {
                if (_currentBlock == null)
                {
                    _currentBlock = new BlockControl(null);

                    _basePanel.Children.Add(_currentBlock);
                }
                _currentBlock.AddElement(element, false);
            }
        }

        public void OpenBlock()
        {
            var block = new BlockControl(this._currentBlock);
            var element = AddConditionElement();
            if (element != null)
            {
                if (_currentBlock == null)
                {
                    _basePanel.Children.Add(block);
                }
                else
                {
                    _currentBlock.AddElement(block, false);
                }
                _currentBlock = block;
                _currentBlock.AddElement(element, false);
            }
        }

        public void CloseBlock()
        {
            var block = _currentBlock.BaseBlock;
            if (block != null)
            {
                _currentBlock = block;
            }
        }

        private UIElement AddConditionElement()
        {
            string text;
            var addCondition = new XamlElement.AddConditionWindow();
            if (addCondition.ShowDialogCondition(out text))
            {
                var condition = new Condition(text);
                return condition;
            }

            return null;
        }

        public void ApplyFilter()
        {
            string str = GetFilter();
            _parametr.Filter = str;
            MessageBox.Show(str);
        }

        private string GetFilter()
        {
            var block = _currentBlock;
            while (block.BaseBlock != null)
            {
                block = block.BaseBlock;
            }

            return block.GetFilter();
        }


        public BlockControl Parse(string filter)
        {
            var result = filter.Split(' ');
            _currentBlock = null;
            string text = "";
            foreach (var item in result)
            {
                if (item == "(")
                {
                    if (text != "")
                    {
                        _currentBlock?.AddElement(new Condition(text.Trim(' ')), true);
                        text = "";
                    }
                    var blok = new BlockControl(_currentBlock);
                    _currentBlock?.AddElement(blok, true);
                    _currentBlock = blok;
                }
                else if (item == ")")
                {
                    if (text != "")
                    {
                        _currentBlock?.AddElement(new Condition(text.Trim(' ')), true);
                        text = "";
                    }
                    if (_currentBlock?.BaseBlock != null)
                    {
                        _currentBlock = _currentBlock?.BaseBlock;
                    }
                }
                else if (item == "OR" || item == "AND")
                {
                    if (text != "")
                    {
                        _currentBlock?.AddElement(new Condition(text.Trim(' ')), true);
                        text = "";
                    }
                    _currentBlock?.AddElement(new ComboControll(item), true);
                }
                else
                {
                    text += item + " ";
                }
            }

            return _currentBlock;
        }

    }
}