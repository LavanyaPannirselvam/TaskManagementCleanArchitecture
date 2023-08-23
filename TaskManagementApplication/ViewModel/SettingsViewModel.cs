using Microsoft.UI.Xaml.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using TaskManagementCleanArchitecture.Converter;
using Windows.Globalization;
using Windows.UI.Xaml.Media;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class SettingsViewModel : NotifyPropertyBase
    {
        
        public SettingsViewModel() 
        {
            InitializeColors();
            Languages = new ObservableCollection<Language>
            {
                new Language("en-US"),
                new Language("ru-RU")
            };

        }
        private void InitializeColors()
        {
            AccentColors = new List<SolidColorBrush>(){
                ColorsHelper.GetSolidColorBrush("FF6A3F73"),
                ColorsHelper.GetSolidColorBrush("FF0078D4"),
                ColorsHelper.GetSolidColorBrush("FF3666CD"),
                ColorsHelper.GetSolidColorBrush("FFBA4273"),
                ColorsHelper.GetSolidColorBrush("FF6E6FD8"),
                ColorsHelper.GetSolidColorBrush("FF488FA5"),
            };
        }

        public IEnumerable<SolidColorBrush> AccentColors { get; set; }

        public IReadOnlyList<string> userLanguages = Windows.Globalization.ApplicationLanguages.ManifestLanguages;
        
        private ObservableCollection<Language> _languages;
        public ObservableCollection<Language> Languages 
        { 
            get { return _languages; }
            set 
            { 
                _languages = value; 
                OnPropertyChanged(nameof(Languages));
            }
        }
    }
}
