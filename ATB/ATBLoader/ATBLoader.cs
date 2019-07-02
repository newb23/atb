using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using ff14bot.AClasses;
using ff14bot.Behavior;
using ff14bot.Helpers;
using TreeSharp;
using Action = TreeSharp.Action;

namespace ATBLoader
{
    public class ATBLoader : BotBase
    {
        // Change this settings to reflect your project!
        private const string ProjectName = "ATB";

        private const string ProjectMainType = "ATB.ATB";
        private const string ProjectAssemblyName = "ATB.dll";
        private static readonly Color LogColor = Colors.LawnGreen;
        public override PulseFlags PulseFlags => PulseFlags.All;
        public override bool IsAutonomous => false;
        public override bool WantButton => true;
        public override bool RequiresProfile => false;

        // Don't touch anything else below from here!
        private static readonly object Locker = new object();

        private static readonly string ProjectAssembly = Path.Combine(Environment.CurrentDirectory, $@"BotBases\{ProjectName}\{ProjectAssemblyName}");
        private static readonly string GreyMagicAssembly = Path.Combine(Environment.CurrentDirectory, @"GreyMagic.dll");
        private static readonly string VersionPath = Path.Combine(Environment.CurrentDirectory, $@"BotBases\{ProjectName}\version.txt");
        private static volatile bool _loaded;

        private static object Product { get; set; }

        private static MethodInfo StartFunc { get; set; }

        private static MethodInfo StopFunc { get; set; }

        private static MethodInfo ButtonFunc { get; set; }

        private static MethodInfo RootFunc { get; set; }
        private static MethodInfo InitFunc { get; set; }

        public override string Name => ProjectName;

        public override Composite Root
        {
            get
            {
                if (!_loaded && Product == null ) { LoadProduct(); }
                return Product != null ? (Composite)RootFunc.Invoke(Product, null) : new Action();
            }
        }

        public override void OnButtonPress()
        {
            if (!_loaded && Product == null ) { LoadProduct(); }
            if (Product != null) { ButtonFunc.Invoke(Product, null); }
        }

        public override void Start()
        {
            if (!_loaded && Product == null ) { LoadProduct(); }
            if (Product != null) { StartFunc.Invoke(Product, null); }
        }

        public override void Stop()
        {
            if (!_loaded && Product == null ) { LoadProduct(); }
            if (Product != null) { StopFunc.Invoke(Product, null); }
        }

        public static void RedirectAssembly()
        {
            ResolveEventHandler handler = (sender, args) =>
            {
                string name = Assembly.GetEntryAssembly().GetName().Name;
                var requestedAssembly = new AssemblyName(args.Name);
                return requestedAssembly.Name != name ? null : Assembly.GetEntryAssembly();
            };

            AppDomain.CurrentDomain.AssemblyResolve += handler;

            ResolveEventHandler greyMagicHandler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name);
                return requestedAssembly.Name != "GreyMagic" ? null : Assembly.LoadFrom(GreyMagicAssembly);
            };

            AppDomain.CurrentDomain.AssemblyResolve += greyMagicHandler;
        }

        private static Assembly LoadAssembly(string path)
        {
            if (!File.Exists(path)) { return null; }

            Assembly assembly = null;
            try { assembly = Assembly.LoadFrom(path); }
            catch (Exception e) { Logging.WriteException(e); }

            return assembly;
        }

        private static object Load()
        {
            RedirectAssembly();

            var assembly = LoadAssembly(ProjectAssembly);
            if (assembly == null) { return null; }

            Type baseType;
            try { baseType = assembly.GetType(ProjectMainType); }
            catch (Exception e)
            {
                Log(e.ToString());
                return null;
            }

            object bb;
            try { bb = Activator.CreateInstance(baseType); }
            catch (Exception e)
            {
                Log(e.ToString());
                return null;
            }

            if (bb != null) { Log(ProjectName + " was loaded successfully."); }
            else { Log("Could not load " + ProjectName + ". This can be due to a new version of Rebornbuddy being released. An update should be ready soon."); }

            return bb;
        }

        private static void LoadProduct()
        {
            lock (Locker)
            {
                if (Product != null) { return; }
                Product = Load();
                _loaded = true;
                if (Product == null) { return; }

                StartFunc = Product.GetType().GetMethod("Start");
                StopFunc = Product.GetType().GetMethod("Stop");
                ButtonFunc = Product.GetType().GetMethod("OnButtonPress");
                RootFunc = Product.GetType().GetMethod("GetRoot");
                InitFunc = Product.GetType().GetMethod("OnInitialize", new[] { typeof(int) });
                if (InitFunc != null)
                {
#if RB_CN
                Log($"{ProjectName}CN loaded.");
                InitFunc.Invoke(Product, new[] {(object)2});
#else
                    Log($"{ProjectName}64 loaded.");
                    InitFunc.Invoke(Product, new[] { (object)1 });
#endif
                }
            }
        }

        private static void Log(string message)
        {
            message = "[Auto-Updater][" + ProjectName + "] " + message;
            Logging.Write(LogColor, message);
        }
    }
}