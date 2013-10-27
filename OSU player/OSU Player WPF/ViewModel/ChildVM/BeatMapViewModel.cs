using GalaSoft.MvvmLight;
using OSU_Player_WPF.Model;

namespace OSU_Player_WPF.ViewModel.ChildVM
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BeatMapViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the BeatMapViewModel class.
        /// </summary>
        public BeatMapViewModel()
        {
            InitializeProp();  //初始化自动更新属性
        }

        private void InitializeProp()
        {
            this.ABeatMap = new BeatMapInfo()
            {
                Title = ""
            };
        }

        #region 自动更新属性

        private BeatMapInfo _ABeatMap;
        /// <summary>
        /// BeatMap实例
        /// </summary>
        public BeatMapInfo ABeatMap
        {
            get { return _ABeatMap; }
            set
            {
                _ABeatMap = value;
                this.RaisePropertyChanged("ABeatMap");
            }
        }


        #endregion
    }
}