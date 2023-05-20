using FontAwesome6;
using FontAwesome6.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileNumRename.Control
{
    internal class KeyBorder : Border
    {
        public string Text { get; set; }

        public EFontAwesomeIcon Icon { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (this.Text != null)
            {
                this.Child = GetText();
            }
            else if (this.Icon != EFontAwesomeIcon.None)
            {
                this.Child = GetIcon();
            }

            this.Child = GetText();
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
            var icon = new FontAwesome();

            return new FontAwesome()
            {
                Icon = this.Icon,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 14,
            };
        }
    }
}
