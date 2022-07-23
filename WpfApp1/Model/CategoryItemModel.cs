using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class CategoryItemModel
    {
        public CategoryItemModel() { }
        public CategoryItemModel(string name, bool state = false)
        {
            CategoryName = name;
            IsSelected = state;
        }
        public bool IsSelected { get; set; }
        public string CategoryName { get; set; }
    }
}
