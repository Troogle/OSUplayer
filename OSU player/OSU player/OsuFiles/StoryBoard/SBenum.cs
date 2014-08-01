namespace OSUplayer.OsuFiles.StoryBoard
{
    public enum ElementType
    {
        Background,
        Video,
        Break,
        Colour,
        Sprite,
        Sample,
        Animation
    }
    public enum ElementLayer
    {
        Background,
        Fail,
        Pass,
        Foreground
    }
    public enum ElementOrigin
    {
        TopLeft,
        TopCentre,
        TopRight,
        CentreLeft,
        Centre,
        CentreRight,
        BottomLeft,
        BottomCentre,
        BottomRight
    }
    public enum ElementLoopType
    {
        LoopOnce,
        LoopForever
    }
    public enum EventType
    {
        //F - fade【隐藏(淡入淡出)】
        //M - move【移动】
        //S - scale【缩放】
        //V - vector scale (width and height separately)【矢量缩放(宽高分别变动)】
        //R - rotate【旋转】
        //C - colour【颜色】
        //L - loop【循环】
        //T - Event-triggered loop【事件触发循环】
        //P - Parameters【参数】
        //Play - 播放sample
        F,
        MX,
        MY,
        M,
        S,
        V,
        R,
        C,
        L,
        T,
        P,
        Play
    }
    public enum Triggertype
    {
        HitSoundClap,
        HitSoundFinish,
        HitSoundWhistle,
        Passing,
        Failing
    }
}
