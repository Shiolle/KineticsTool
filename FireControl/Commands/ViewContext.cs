using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FireControl.Commands
{
    internal class ViewContext
    {
        public string ViewTypeName { get; set; }
        public Window ParentWindow { get; set; }
    }
}
