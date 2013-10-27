using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OSU_Player_WPF.Model;
using OSU_Player_WPF.Service;
using System.Collections.Generic;

namespace OSU_Player_WPF.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            //值初始化
            InitializeMainAppInfo();  //程序信息

        }

        LibService zLibService = new LibService();

        #region 自动更新属性

        private MainAppInfo _MainAppInfos;
        /// <summary>
        /// 主程序信息
        /// </summary>
        public MainAppInfo MainAppInfos
        {
            get { return _MainAppInfos; }
            set
            {
                _MainAppInfos = value;
                this.RaisePropertyChanged("MainAppInfos");
            }
        }

        private List<BeatMapInfo> _BeatMapListing;
        /// <summary>
        /// BeatMap列表
        /// </summary>
        public List<BeatMapInfo> BeatMapListing
        {
            get { return _BeatMapListing; }
            set
            {
                _BeatMapListing = value;
                this.RaisePropertyChanged("BeatMapListing");
            }
        }


        #endregion

        #region 事件命令

        private RelayCommand _ShowBeatMapWindow;
        /// <summary>
        /// 界面传过来的命令（按钮按下实例）
        /// </summary>
        public RelayCommand ShowBeatMapWindow
        {
            get
            {
                return _ShowBeatMapWindow
                    ?? (_ShowBeatMapWindow = new RelayCommand(() =>
                                          {
                                              Ex_ShowBeatMapWindow();  //这个命令要干什么
                                          }));
            }
        }



        #endregion

        #region 命令方法

        private void Ex_ShowBeatMapWindow()
        {
            throw new System.NotImplementedException();
        }

        private void GetBeatMapList()
        {
            //
        }

        #endregion

        #region 普通方法逻辑

        private void InitializeMainAppInfo()
        {
            this.MainAppInfos = zLibService.GetInitialize().InitializeMainAppinfo();  //初始化主程序信息
        }

        #endregion
    }
}