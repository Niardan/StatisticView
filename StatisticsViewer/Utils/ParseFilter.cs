using System.Threading;
using StatisticsViewer.XamlElement;

namespace StatisticsViewer.Utils
{
    public class ParseFilter
    {
        private BlockControl _currentBlock;

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