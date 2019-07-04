using ff14bot.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ATB.Models
{
    public abstract class BaseModel : JsonSettings, INotifyPropertyChanged
    {
        #region Constructor

        protected BaseModel(string path) : base(path)
        {
        }

        #endregion Constructor

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged
    }
}