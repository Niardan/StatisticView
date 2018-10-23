using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StatisticsViewer.XamlElement
{
    class ContextDataGridTextCollumn : DataGridTextColumn
    {
       public string Context { set; get; }
    }
}
