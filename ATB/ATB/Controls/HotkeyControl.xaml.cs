using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace ATB.Controls
{
    /// <summary>
    /// Interaction logic for HotkeyControl.xaml
    /// </summary>
    public partial class HotkeyControl : UserControl

    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(HotkeyControl), new UIPropertyMetadata("Hotkey"));
        public static readonly DependencyProperty KeySettingProperty = DependencyProperty.Register("KeySetting", typeof(Keys), typeof(HotkeyControl), new PropertyMetadata(Keys.None, OnKeyChanged));
        public static readonly DependencyProperty ModKeySettingProperty = DependencyProperty.Register("ModKeySetting", typeof(ModifierKeys), typeof(HotkeyControl), new PropertyMetadata(ModifierKeys.None, OnModKeyChanged));

        private static void OnKeyChanged(DependencyObject keySetting, DependencyPropertyChangedEventArgs eventArgs)
        {
        }

        private static void OnModKeyChanged(DependencyObject keySetting, DependencyPropertyChangedEventArgs eventArgs)
        {
        }

        public HotkeyControl()

        {
            InitializeComponent();
            PreviewKeyDown += OnPreviewKeyDown;
            LostFocus += OnLostFocus;
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)

        {
            // Re - Registering Code
        }

        public string Text

        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string HkText => ModKeySetting + " + " + KeySetting;

        public Keys KeySetting

        {
            get { return (Keys)GetValue(KeySettingProperty); }
            set { SetValue(KeySettingProperty, value); }
        }

        public ModifierKeys ModKeySetting

        {
            get { return (ModifierKeys)GetValue(ModKeySettingProperty); }
            set { SetValue(ModKeySettingProperty, value); }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // The text box grabs all input.
            e.Handled = true;

            // Fetch the actual shortcut key.
            var key = (e.Key == Key.System ? e.SystemKey : e.Key);

            if (key == Key.Escape)
            {
                TxtHk.Text = "None + None";
                ModKeySetting = ModifierKeys.None;
                KeySetting = Keys.None;
                return;
            }

            // Ignore modifier keys.
            if (key == Key.LeftShift || key == Key.RightShift
                || key == Key.LeftCtrl || key == Key.RightCtrl
                || key == Key.LeftAlt || key == Key.RightAlt
                || key == Key.LWin || key == Key.RWin)
            {
                return;
            }

            // Build the shortcut key name.
            var shortcutText = new StringBuilder();
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                shortcutText.Append("Ctrl + ");
                ModKeySetting = ModifierKeys.Control;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                shortcutText.Append("Shift + ");
                ModKeySetting = ModifierKeys.Shift;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                shortcutText.Append("Alt + ");
                ModKeySetting = ModifierKeys.Alt;
            }

            if (Keyboard.Modifiers == 0)
            {
                shortcutText.Append("None + ");
                ModKeySetting = ModifierKeys.None;
            }

            shortcutText.Append(key);

            var newKey = (Keys)KeyInterop.VirtualKeyFromKey(key);
            KeySetting = newKey;
            // Update the text box.
            TxtHk.Text = shortcutText.ToString();
        }
    }
}