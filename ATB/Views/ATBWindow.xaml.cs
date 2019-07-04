using ATB.Models;
using ATB.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application = System.Windows.Application;

namespace ATB.Views
{
    public partial class ATBWindow : Window
    {
        public ATBWindow()
        {
            InitializeComponent();

            SelectTheme();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        #region Theme Switch

        private void SelectTheme()
        {
            switch (MainSettingsModel.Instance.Theme)
            {
                case SelectedTheme.Pink:
                    Pink();
                    break;

                case SelectedTheme.Blue:
                    Blue();
                    break;

                case SelectedTheme.Green:
                    Green();
                    break;

                case SelectedTheme.Red:
                    Red();
                    break;

                case SelectedTheme.Yellow:
                    Yellow();
                    break;

                default:
                    Pink();
                    break;
            }
        }

        private void Pink()
        {
            Resources.MergedDictionaries.Clear();
            AddResourceDictionary("/ATB;component/Views/Styles/ATBStyles.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/BaseColors.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/PinkTheme.xaml");
        }

        private void Blue()
        {
            Resources.MergedDictionaries.Clear();
            AddResourceDictionary("/ATB;component/Views/Styles/ATBStyles.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/BaseColors.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/BlueTheme.xaml");
        }

        private void Yellow()
        {
            Resources.MergedDictionaries.Clear();
            AddResourceDictionary("/ATB;component/Views/Styles/ATBStyles.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/BaseColors.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/YellowTheme.xaml");
        }

        private void Red()
        {
            Resources.MergedDictionaries.Clear();
            AddResourceDictionary("/ATB;component/Views/Styles/ATBStyles.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/BaseColors.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/RedTheme.xaml");
        }

        private void Green()
        {
            Resources.MergedDictionaries.Clear();
            AddResourceDictionary("/ATB;component/Views/Styles/ATBStyles.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/BaseColors.xaml");
            AddResourceDictionary("/ATB;component/Views/Styles/GreenTheme.xaml");
        }

        #endregion Theme Switch

        private void AddResourceDictionary(string source)
        {
            var resourceDictionary = Application.LoadComponent(new Uri(source, UriKind.Relative)) as ResourceDictionary;
            Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            FormManager.SaveFormInstances();

            Close();
        }

        private void CmbSwitchTheme(object sender, SelectionChangedEventArgs e)
        {
            SelectTheme();
        }
    }
}