﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiceMaster3600.View.Controls
{
    public partial class DiceControl : UserControl
    {
        #region Fields
        #endregion

        #region Properties
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int), typeof(DiceControl), new PropertyMetadata(0, OnValueChanged));

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            nameof(IsSelected), typeof(bool), typeof(DiceControl), new PropertyMetadata(false, OnIsSelectedChanged));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        #endregion


        #region Constructors
        public DiceControl()
        {
            InitializeComponent();
        }
        #endregion
        

        #region Methods
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DiceControl control)
            {
                int newValue = (int)e.NewValue;

                control.HideAllGroups();
                control.ShowGroup(newValue);
            }
        }
        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DiceControl control)
            {
                control.Background = (bool)e.NewValue 
                    ? new SolidColorBrush(Color.FromRgb(255, 204, 203)) 
                    : Brushes.Transparent;
            }
        }

        private void ShowGroup(int value)
        {
            switch (value)
            {
                case 1: One.Visibility = Visibility.Visible; break;
                case 2: Two.Visibility = Visibility.Visible; break;
                case 3: Three.Visibility = Visibility.Visible; break;
                case 4: Four.Visibility = Visibility.Visible; break;
                case 5: Five.Visibility = Visibility.Visible; break;
                case 6: Six.Visibility = Visibility.Visible; break;
                default: break;
            }
        }

        private void HideAllGroups()
        {
            One.Visibility = Visibility.Collapsed;
            Two.Visibility = Visibility.Collapsed;
            Three.Visibility = Visibility.Collapsed;
            Four.Visibility = Visibility.Collapsed;
            Five.Visibility = Visibility.Collapsed;
            Six.Visibility = Visibility.Collapsed;
        }
        #endregion


        #region Events
        #endregion
    }
}