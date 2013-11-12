namespace OSU_player.Properties {
    // 通过此类可以处理设置类的特定事件:
    //  在更改某个设置的值之前将引发 SettingChanging 事件。
    //  在更改某个设置的值之后将引发 PropertyChanged 事件。
    //  在加载设置值之后将引发 SettingsLoaded 事件。
    //  在保存设置值之前将引发 SettingsSaving 事件。
    internal sealed partial class Settings {
        public Settings() {
            // // 若要为保存和更改设置添加事件处理程序，请取消注释下列行:
            //
            this.PropertyChanged += Settings_PropertyChanged;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }

        void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.Save();
        }
    }
}
