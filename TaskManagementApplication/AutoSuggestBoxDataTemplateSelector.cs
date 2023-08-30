using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace TaskManagementCleanArchitecture
{
    public class AutoSuggestBoxDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UsersListTemplate { get; set; }
        public DataTemplate NoItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is UserBO)
            {
                return UsersListTemplate;
            }
            else { return NoItemTemplate; }
        }
    }
}
