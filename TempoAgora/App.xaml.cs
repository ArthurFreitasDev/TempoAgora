using TempoAgora.Helpers;

namespace TempoAgora
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        static HelpersTempo _dbTempo;
        public static HelpersTempo DbTempo
        {
            get
            {
                if (_dbTempo == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_tempo.db3");
                    _dbTempo = new HelpersTempo(path);
                }
                return _dbTempo;
            }
        }
    }
}
