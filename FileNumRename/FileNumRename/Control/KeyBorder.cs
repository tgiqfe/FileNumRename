using FontAwesome6;
using FontAwesome6.Fonts;
using FontAwesome6.Fonts.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileNumRename.Control
{
    internal class KeyBorder : Border
    {
        public string Text { get; set; }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                "Icon", 
                typeof(EFontAwesomeIcon), 
                typeof(ImageAwesome), 
                new PropertyMetadata(EFontAwesomeIcon.None, OnIconPropertyChanged));

        public EFontAwesomeIcon Icon
        {
            get { return (EFontAwesomeIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!string.IsNullOrEmpty(this.Text))
            {
                this.Child = GetText();
            }
            else if (this.Icon != EFontAwesomeIcon.None)
            {
                this.Child = GetIcon();
            }
        }

        private TextBlock GetText()
        {
            return new TextBlock()
            {
                Text = this.Text,
                FontFamily = new System.Windows.Media.FontFamily("Noto Sans JP"),
                FontSize = 11,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(0, 3, 0, 3),
            };
        }

        private FontAwesome GetIcon()
        {
            return new FontAwesome()
            {
                Icon = this.Icon,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 14,
            };
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ImageAwesome control)
            {
                return;
            }

            if (control.Icon == EFontAwesomeIcon.None)
            {
                control.Source = null;
            }
            else
            {
                control.Source = control.Icon.CreateImageSource(control.PrimaryColor);
            }
        }
    }
}
