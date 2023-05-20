using System.Windows;
using System.Windows.Input;

namespace FileNumRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Item.Collection;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            switch (e.Key)
            {
                case Key.Escape:
                    Application.Current.Shutdown();
                    break;
                case Key.Enter:
                    Item.Collection.ChangeFileName();
                    break;
                case Key.Up:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        //  increase の値を最大値に
                        Item.Collection.ToMaxIncrease();
                    }
                    else
                    {
                        //  Increase の値を一つ上に
                        Item.Collection.UpdateIncrease(1);
                    }
                    break;
                case Key.Down:
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        //  increase の値を最小値に
                        Item.Collection.ToMinIncrease();
                    }
                    else
                    {
                        //  Increase の値を一つ下に
                        Item.Collection.UpdateIncrease(-1);
                    }
                    break;
                case Key.Left:
                    //  Cursor の値を一つ下げる (左に移動)
                    Item.Collection.UpdateCursor(-1);
                    break;
                case Key.Right:
                    //  Cursor の値を一つ上げる (右に移動)
                    Item.Collection.UpdateCursor(1);
                    break;
            }
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!FileList.IsMouseOver)
            {
                this.DragMove();
            }
        }
    }
}
