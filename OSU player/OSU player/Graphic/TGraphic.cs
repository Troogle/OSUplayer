using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OSUplayer.OsuFiles.StoryBoard;
using Device = Microsoft.Xna.Framework.Graphics.GraphicsDevice;

namespace OSUplayer.Graphic
{
    /// <summary>
    ///     MDX静态图像基类
    /// </summary>
    internal class StaticGraphic
    {
        //protected Texture2D texture;
        protected byte Alpha;
        protected Color Color;
        protected int CurrentFrameIndex; //当前材质行索引
        protected int FrameCount; //材质行总数
        protected float Layer;
        //protected Matrix scaleMatrix, transformMatrix, rotateMatrix;

        protected Vector2 Origin; //坐标轴位置

        /// <summary>
        ///     0x0=none,1=H,2=V,4=A
        /// </summary>
        public byte Parameter;

        protected Vector2 Position; //显示位置
        protected float Rotate; //旋转角度
        protected Vector2 Scale; //缩放比例
        protected Texture2D[] Texturearray;

        //protected Rectangle rectangle;


        public StaticGraphic(Device graphicDevice, string source, Vector2 position,
            float rotate, float scale, Color color, byte alpha, byte parameter)
        {
            Texturearray = new[] {Texture2D.FromFile(graphicDevice, source)};
            Origin = new Vector2(0, 0);
            Position = position;
            Rotate = rotate;
            Scale = new Vector2(scale, scale);
            Layer = 0;
            Color = color;
            Alpha = alpha;
            Parameter = parameter;
            CurrentFrameIndex = 0;
        }

        protected StaticGraphic()
        {
        }

        /// <summary>
        ///     绘制图像
        /// </summary>
        public void Draw(SpriteBatch sprite)
        {
            var tmp = new SpriteEffects();
            if ((Parameter & 1) == 1)
            {
                tmp = SpriteEffects.FlipHorizontally;
            }
            if ((Parameter & 2) == 2)
            {
                tmp = tmp | SpriteEffects.FlipVertically;
            }
            Color = new Color(Color.R, Color.G, Color.B, Alpha);
            sprite.Draw(Texturearray[CurrentFrameIndex], Position, null, Color, Rotate, Origin, Scale, tmp, Layer);
        }
    }


    /// <summary>
    ///     MDX带有动作列表的图像基类
    /// </summary>
    internal class TGraphic : StaticGraphic
    {
        private readonly ElementLoopType _loop;
        private readonly int _mSPerFrame; // 帧动画的速率
        private readonly ElementOrigin _origin;
        private TSpriteAction _alphaAction;
        private TSpriteAction _colorAction;
        private int _msLastFrame; //上次记录的时间
        private TSpriteAction _parameterAction;
        private TSpriteAction _rotateAction;
        private TSpriteAction _scaleXAction;
        private TSpriteAction _scaleYAction;
        private TSpriteAction _xAction;
        private TSpriteAction _yAction;

        public TGraphic(SBelement element, Texture2D[] file)
        {
            switch (element.Type)
            {
                case ElementType.Sprite:
                {
                    Texturearray = file;
                    FrameCount = 1;
                    CurrentFrameIndex = 0;
                    _msLastFrame = 0;
                    _mSPerFrame = 16;
                    Position = new Vector2(element.X0, element.Y0);
                    Origin = Getorigin(Texturearray[0], element.Origin);
                    break;
                }
                case ElementType.Animation:
                {
                    Texturearray = file;
                    FrameCount = element.FrameCount;
                    _mSPerFrame = element.Framedelay;
                    _loop = element.Looptype;
                    CurrentFrameIndex = 0;
                    _msLastFrame = 0;
                    Position = new Vector2(element.X0, element.Y0);
                    //this.texture = texturearray[0];
                    Origin = Getorigin(Texturearray[0], element.Origin);
                    break;
                }
                default:
                    throw (new FormatException("Failed to resolve .osb file"));
            }
            _origin = element.Origin;
            Color = Color.White;
            Alpha = 0;
            Parameter = 0;
            Rotate = 0f;
            if (element.Layers == ElementLayer.Background)
            {
                Layer = 0.9f - element.Z*0.000001f;
            }
            else
            {
                Layer = 0.5f - element.Z*0.000001f;
            }
            Scale = new Vector2(1f, 1f);
            //  this.InitSpriteAction();
        }

        /*   /// <summary>
           /// 创建一个多帧TGraphic实例
           /// </summary>
           /// <param name="graphicDevice">显示设备</param>
           /// <param name="bitmap">要绘制的图像</param>
           /// <param name="pPosition">绘制坐标</param>
           /// <param name="pCenter">旋转中心</param>
           /// <param name="pFrameCount">总帧数</param>
           /// <param name="pFPS">FPS</param>
           public TGraphic(Device graphicDevice, Bitmap bitmap, Vector3 pPosition, Vector3 pCenter,
                                    int pFrameCount, int pFPS)
               : base(graphicDevice, bitmap, pPosition, 0f, 1f, Color.White, 255, 0)
           {
               this.frameCount = pFrameCount;
               this.currentFrameIndex = 0;
               this.mSPerFrame = (int)(1f / pFPS * 1000);
               this.msLastFrame = 0;
               this.center = pCenter;
               this.InitSpriteAction();
           }

           /// <summary>
           /// 创建一个单帧TGraphic实例
           /// </summary>
           /// <param name="graphicDevice">显示设备</param>
           /// <param name="bitmap">要绘制的图像</param>
           /// <param name="pPosition">绘制坐标</param>
           /// <param name="pCenter">旋转中心</param>
           public TGraphic(Device graphicDevice, Bitmap bitmap, Vector3 pPosition, Vector3 pCenter)
               : base(graphicDevice, bitmap, pPosition, 0f, 1f, Color.White, 255, 0)
           {
               this.center = pCenter;
               this.frameCount = 1;
               this.currentFrameIndex = 0;
               this.msLastFrame = 0;
               this.mSPerFrame = 16;
               this.InitSpriteAction();
           }*/

        private static Vector2 Getorigin(Texture2D texture, ElementOrigin origin)
        {
            var id = (int) origin;
            float x = (float) (id%3)/2*texture.Width;
            float y = (float) (id - id%3)/6*texture.Height;
            return new Vector2(x, y);
        }

        /// <summary>
        ///     更新图像
        /// </summary>
        public virtual void Update(int currentTime)
        {
            //处理动作更新
            //
            if (_alphaAction.Enable)
            {
                _alphaAction.Update(currentTime);
                Alpha = (byte) _alphaAction.CurrentValue; //默认就是0,ok~
            }
            if (_colorAction.Enable)
            {
                _colorAction.Update(currentTime);
                var colorv = (int) _colorAction.CurrentValue;
                if (_colorAction.IsActive)
                {
                    Color = new Color((byte) (colorv >> 0x10), (byte) (colorv >> 8), (byte) colorv);
                }
            }
            if (_xAction.Enable)
            {
                _xAction.Update(currentTime);
                if (_xAction.IsActive)
                {
                    Position.X = _xAction.CurrentValue;
                }
            }
            if (_yAction.Enable)
            {
                _yAction.Update(currentTime);
                if (_yAction.IsActive)
                {
                    Position.Y = _yAction.CurrentValue;
                }
            }
            if (_scaleXAction.Enable)
            {
                _scaleXAction.Update(currentTime);
                if (_scaleXAction.IsActive)
                {
                    Scale.X = _scaleXAction.CurrentValue;
                }
            }
            if (_scaleYAction.Enable)
            {
                _scaleYAction.Update(currentTime);
                if (_scaleYAction.IsActive)
                {
                    Scale.Y = _scaleYAction.CurrentValue;
                }
            }
            if (_rotateAction.Enable)
            {
                _rotateAction.Update(currentTime);
                Rotate = _rotateAction.CurrentValue; //默认就是0,ok~
            }
            if (_parameterAction.Enable)
            {
                _parameterAction.Update(currentTime);
                Parameter = (byte) _parameterAction.CurrentValue; //默认就是0,ok~
            }

            // 处理帧更新
            //
            if (FrameCount <= 1) return;
            if (currentTime - _msLastFrame < _mSPerFrame) return;
            _msLastFrame = currentTime;
            CurrentFrameIndex++;
            if (CurrentFrameIndex == FrameCount)
            {
                if (_loop == ElementLoopType.LoopForever)
                {
                    CurrentFrameIndex = 0;
                }
                else
                {
                    CurrentFrameIndex = FrameCount - 1;
                }
            }
            //this.texture = texturearray[currentFrameIndex];
            Origin = Getorigin(Texturearray[CurrentFrameIndex], _origin);
        }

        #region 设置动作

        /// <summary>
        ///     初始化精灵动作
        /// </summary>
        private void InitSpriteAction()
        {
            _alphaAction = new TSpriteAction();
            _xAction = new TSpriteAction();
            _yAction = new TSpriteAction();
            _scaleXAction = new TSpriteAction();
            _scaleYAction = new TSpriteAction();
            _rotateAction = new TSpriteAction();
            _colorAction = new TSpriteAction();
            _parameterAction = new TSpriteAction();
        }

        /// <summary>
        ///     设置Alpha动作
        /// </summary>
        public void SetAlphaAction(TSpriteAction pAlphaAction)
        {
            _alphaAction = pAlphaAction;
        }

        /// <summary>
        ///     设置X轴动作
        /// </summary>
        public void SetXAction(TSpriteAction pXAction)
        {
            _xAction = pXAction;
        }

        /// <summary>
        ///     设置Y轴动作
        /// </summary>
        public void SetYAction(TSpriteAction pYAction)
        {
            _yAction = pYAction;
        }

        /// <summary>
        ///     设置ScaleX动作
        /// </summary>
        public void SetScaleXAction(TSpriteAction pScaleAction)
        {
            _scaleXAction = pScaleAction;
        }

        /// <summary>
        ///     设置ScaleY动作
        /// </summary>
        public void SetScaleYAction(TSpriteAction pScaleAction)
        {
            _scaleYAction = pScaleAction;
        }

        /// <summary>
        ///     设置Rotate动作
        /// </summary>
        public void SetRotateAction(TSpriteAction pRotateAction)
        {
            _rotateAction = pRotateAction;
        }

        /// <summary>
        ///     设置Color动作
        /// </summary>
        public void SetColorAction(TSpriteAction pColorAction)
        {
            _colorAction = pColorAction;
        }

        /// <summary>
        ///     设置Parameter动作
        /// </summary>
        public void SetParameterAction(TSpriteAction pParameterAction)
        {
            _parameterAction = pParameterAction;
        }

        #endregion
    }
}