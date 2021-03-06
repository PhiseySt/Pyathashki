﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Fifteen
{
    public class Cell : Control
    {
        static Cell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Cell), new FrameworkPropertyMetadata(typeof(Cell)));
        }

        #region Text...

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Cell), new UIPropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region Top...
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top", typeof(double), typeof(Cell), new UIPropertyMetadata(default(double)));
        public double Top
        {
            get => (double)GetValue(TopProperty);
            set => SetValue(TopProperty, value);
        }
        #endregion
        #region Left...
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left", typeof(double), typeof(Cell), new UIPropertyMetadata(default(double)));
        public double Left
        {
            get => (double)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        #endregion

        #region Row...
        public static readonly DependencyProperty RowProperty = DependencyProperty.Register("Row", typeof(int), typeof(Cell), new FrameworkPropertyMetadata(default(int), OnRowChanged));
        public int Row
        {
            get => (int)GetValue(RowProperty);
            set => SetValue(RowProperty, value);
        }
        private static void OnRowChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source is Cell instance)) return;

            instance.AnimateMove(TopProperty, (int)e.NewValue);
        }

        #endregion
        #region Column...
        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register("Column", typeof(int), typeof(Cell), new FrameworkPropertyMetadata(default(int), OnColumnChanged));
        public int Column
        {
            get => (int)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }
        private static void OnColumnChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (!(source is Cell instance)) return;

            instance.AnimateMove(LeftProperty, (int)e.NewValue);
        }
        #endregion

        private void AnimateMove(DependencyProperty property, double value, double duration = 500)
        {
            var doubleAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
                To = value * 80
            };
            BeginAnimation(property, doubleAnimation);
        }
    }
}
