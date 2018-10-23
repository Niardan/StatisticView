using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using StatisticsViewer.Utils;
using StatisticsViewer.XamlElement;
using Condition = StatisticsViewer.XamlElement.Condition;

namespace StatisticsViewer
{
    public class FilterController
    {
        private BlockControl _currentBlock;
        private readonly Parametr _parametr;
        private readonly StackPanel _basePanel;
        private readonly AsyncBaseLoader _baseLoader;
        private readonly FieldsValue _fieldsValue;
        private IDictionary<string, ICollection<string>> _filterFields = new Dictionary<string, ICollection<string>>();
        private string _tableName;

        public event ConditionHandler SelectCondition;
        public event ConditionHandler DeselectCondition;

        public FilterController(Parametr parametr, StackPanel basePanel, AsyncBaseLoader baseLoader, string tableName)
        {
            _tableName = tableName;
            _basePanel = basePanel;
            _baseLoader = baseLoader;
            _parametr = parametr;
            _fieldsValue = new FieldsValue(_baseLoader, tableName);
            _currentBlock = Parse(_parametr.Filter);

            if (_currentBlock != null)
            {
                basePanel.Children.Add(_currentBlock);
            }

        }

        public void AddCondition()
        {
            var element = AddConditionElement(_currentBlock);
            if (element != null)
            {
                if (_currentBlock == null)
                {
                    _currentBlock = GetBlock(null);

                    _basePanel.Children.Add(_currentBlock);
                }

                element.Select();
                _currentBlock.AddElement(element, false);
            }
        }

        public void OpenBlock()
        {
            var block = GetBlock(_currentBlock);
            var element = AddConditionElement(block);
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
                element.ParentBlock = _currentBlock;
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

        public void ApplyFilter()
        {
            string str = GetFilter();
            _parametr.Filter = str;           
        }

        public BlockControl Parse(string filter)
        {
            if (filter != null)
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
                            _currentBlock?.AddElement(GetCondition(text.Trim(' '), _currentBlock), true);
                            text = "";
                        }

                        var blok = GetBlock(_currentBlock);
                        _currentBlock?.AddElement(blok, true);
                        _currentBlock = blok;
                    }
                    else if (item == ")")
                    {
                        if (text != "")
                        {
                            _currentBlock?.AddElement(GetCondition(text.Trim(' '), _currentBlock), true);
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
                            _currentBlock?.AddElement(GetCondition(text.Trim(' '), _currentBlock), true);
                            text = "";
                        }

                        _currentBlock?.AddElement(new ComboControll(item), true);
                    }
                    else
                    {
                        text += item + " ";
                    }
                }
            }
           

            return _currentBlock;
        }

        private Condition AddConditionElement(BlockControl block)
        {
            string text;
            var addCondition = new XamlElement.AddConditionWindow(_fieldsValue);
            if (addCondition.ShowDialogCondition(out text))
            {
                return GetCondition(text, block);
            }

            return null;
        }

        private void ConditionOnDeleted(Condition sender)
        {
            var parent = sender.ParentBlock;
            sender.Selected -= ConditionOnSelected;
            sender.Deselected -= ConditionOnDeselected;
            sender.Deleted -= ConditionOnDeleted;
            sender.Changed -= ConditionOnChanged;
            parent.DeleteElement(sender);
            FilterChanged();
        }

        private Condition GetCondition(string text, BlockControl block)
        {
            var condition = new Condition(text, _fieldsValue);
            condition.ParentBlock = block;
            condition.Selected += ConditionOnSelected;
            condition.Deselected += ConditionOnDeselected;
            condition.Deleted += ConditionOnDeleted;
            condition.Changed += ConditionOnChanged;
            return condition;
        }

        private void ConditionOnChanged(Condition sender)
        {
            FilterChanged();
        }

        private BlockControl GetBlock(BlockControl baseBlock)
        {
            var block = new BlockControl(baseBlock);
            block.Deleted += BlockOnDeleted;
            block.Changed += BlockOnChanged;
            return block;
        }

        private void BlockOnChanged(BlockControl sender)
        {
            FilterChanged();
        }

        private void BlockOnDeleted(BlockControl sender)
        {
            sender.Deleted -= BlockOnDeleted;
            sender.Changed -= BlockOnChanged;
            var baseBlock = sender.BaseBlock;
            if (baseBlock != null)
            {
                baseBlock.DeleteElement(sender);
            }
            else
            {
                var parent = (StackPanel)sender.Parent;
                parent.Children.Remove(sender);
            }
        }

        private void ConditionOnDeselected(Condition sender)
        {
            DeselectCondition?.Invoke(sender);
        }

        private void ConditionOnSelected(Condition sender)
        {
            SelectCondition?.Invoke(sender);
        }

        private string GetFilter()
        {
            var block = _currentBlock;
            while (block != null && block.BaseBlock != null)
            {
                block = block.BaseBlock;
            }

            return block?.GetFilter();
        }

        public void FilterChanged()
        {
            _parametr.Filter = GetFilter();
        }
    }
}