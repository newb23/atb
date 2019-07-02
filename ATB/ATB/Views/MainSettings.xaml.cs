using ff14bot;
using ff14bot.Managers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ATB.Views
{
    public partial class MainSettings : UserControl
    {
        public MainSettings()
        {
            InitializeComponent();
        }

        private static string _bot = "ATB";

        private void LaunchRoutineSelect(object sender, RoutedEventArgs e)
        {
            RoutineManager.PreferedRoutine = "";
            RoutineManager.PickRoutine();
            System.Threading.Tasks.Task.Factory.StartNew(async () =>
            {
                await TreeRoot.StopGently(" " + "Preparing to switch Combat Routine.");
                BotManager.SetCurrent(BotManager.Bots.FirstOrDefault(r => r.Name == _bot));
                TreeRoot.Start();
            });
        }
    }
}