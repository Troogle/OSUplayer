using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OSUplayer.OsuFiles.StoryBoard;
using Device = Microsoft.Xna.Framework.Graphics.GraphicsDevice;
using Color = Microsoft.Xna.Framework.Graphics.Color;
namespace OSUplayer.Graphic
{
    /// <summary>
    /// MDX静态图像基类
    /// </summary>
    class StaticGraphic
    {
        //protected Texture2D texture;
        protected Texture2D[] texturearray;
        protected int currentFrameIndex; //当前材质行索引
        protected int frameCount;  //材质行总数
        //protected Matrix scaleMatrix, transformMatrix, rotateMatrix;

        protected Vector2 origin;                                         //坐标轴位置
        protected Vector2 position;                                       //显示位置
        protected float rotate;                                               //旋转角度
        protected Vector2 scale;                                                 //缩放比例
        protected float layer;
        protected Color color;
        protected byte alpha;
        /// <summary>
        /// 0x0=none,1=H,2=V,4=A
        /// </summary>
        public byte parameter;
        //protected Rectangle rectangle;


        public StaticGraphic(Device graphicDevice, string source, Vector2 position,
                                          float rotate, float scale, Color color, byte alpha, byte parameter)
        {
            this.texturearray = new Texture2D[] { Texture2D.FromFile(graphicDevice, source) };
            this.origin = new Vector2(0, 0);
            this.position = position;
            this.rotate = rotate;
            this.scale = new Vector2(scale, scale);
            this.layer = 0;
            this.color = color;
            this.alpha = alpha;
            this.parameter = parameter;
            this.currentFrameIndex = 0;
        }
        protected StaticGraphic() { }

        /// <summary>
        /// 绘制图像
        /// </summary>
        public virtual void Draw(SpriteBatch sprite)
        {
            var tmp = new SpriteEffects();
            if ((this.parameter & 1) == 1) { tmp = SpriteEffects.FlipHorizontally; }
            if ((this.parameter & 2) == 2) { tmp = tmp | SpriteEffects.FlipVertically; }
            this.color = new Color(color.R, color.G, color.B, this.alpha);
            sprite.Draw(this.texturearray[currentFrameIndex], this.position, null, this.color, this.rotate, this.origin, this.scale, tmp, this.layer);
        }
    }


    /// <summary>
    /// MDX带有动作列表的图像基类
    /// </summary>
    class TGraphic : StaticGraphic
    {
        private int mSPerFrame; // 帧动画的速率
        private int msLastFrame; //上次记录的时间
        private ElementLoopType Loop;
        private TSpriteAction xAction;
        private TSpriteAction yAction;
        private TSpriteAction scaleXAction;
        private TSpriteAction scaleYAction;
        private TSpriteAction rotateAction;
        private TSpriteAction alphaAction;
        private TSpriteAction colorAction;
        private TSpriteAction parameterAction;
        private ElementOrigin Origin;

        public TGraphic(Device graphicDevice, SBelement Element, Texture2D[] File)
            : base()
        {
            switch (Element.Type)
            {
                case ElementType.Sprite:
                    {
                        this.texturearray = File;
                        this.frameCount = 1;
                        this.currentFrameIndex = 0;
                        this.msLastFrame = 0;
                        this.mSPerFrame = 16;
                        this.position = new Vector2(Element.X0, Element.Y0);
                        this.origin = Getorigin(this.texturearray[0], Element.Origin);
                        break;
                    }
                case ElementType.Animation:
                    {
                        this.texturearray = File;
                        this.frameCount = Element.FrameCount;
                        this.mSPerFrame = Element.Framedelay;
                        this.Loop = Element.Looptype;
                        this.currentFrameIndex = 0;
                        this.msLastFrame = 0;
                        this.position = new Vector2(Element.X0, Element.Y0);
                        //this.texture = texturearray[0];
                        this.origin = Getorigin(this.texturearray[0], Element.Origin);
                        break;
                    }
                default:
                    throw (new FormatException("Failed to resolve .osb file"));
            }
            this.Origin = Element.Origin;
            this.color = Color.White;
            this.alpha = 0;
            this.parameter = 0;
            this.rotate = 0f;
            if (Element.Layers == ElementLayer.Background) { this.layer = 0.9f - Element.Z * 0.000001f; } else { this.layer = 0.5f - Element.Z * 0.000001f; }
            this.scale = new Vector2(1f, 1f);
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
        private static Vector2 Getorigin(Texture2D texture, ElementOrigin Origin)
        {
            var id = (int)Origin;
            var x = (float)(id % 3) / 2 * texture.Width;
            var y = (float)(id - id % 3) / 6 * texture.Height;
            return new Vector2(x, y);
        }

        /// <summary>
        /// 更新图像
        /// </summary>
        public virtual void Update(int currentTime)
        {
            //处理动作更新
            //
            if (this.alphaAction.Enable)
            {
                this.alphaAction.Update(currentTime);
                this.alpha = (byte)alphaAction.CurrentValue;//默认就是0,ok~
            }
            if (this.colorAction.Enable)
            {
                this.colorAction.Update(currentTime);
                int colorv = (int)colorAction.CurrentValue;
                if (this.colorAction.isActive)
                {
                    this.color = new Color((byte)(colorv >> 0x10), (byte)(colorv >> 8), (byte)colorv);
                }
            }
            if (this.xAction.Enable)
            {
                this.xAction.Update(currentTime);
                if (this.xAction.isActive) { this.position.X = this.xAction.CurrentValue; }
            }
            if (this.yAction.Enable)
            {
                this.yAction.Update(currentTime);
                if (this.yAction.isActive) { this.position.Y = this.yAction.CurrentValue; }
            }
            if (this.scaleXAction.Enable)
            {
                this.scaleXAction.Update(currentTime);
                if (this.scaleXAction.isActive) { this.scale.X = this.scaleXAction.CurrentValue; }
            }
            if (this.scaleYAction.Enable)
            {
                this.scaleYAction.Update(currentTime);
                if (this.scaleYAction.isActive) { this.scale.Y = this.scaleYAction.CurrentValue; }
            }
            if (this.rotateAction.Enable)
            {
                this.rotateAction.Update(currentTime);
                this.rotate = this.rotateAction.CurrentValue;//默认就是0,ok~
            }
            if (this.parameterAction.Enable)
            {
                this.parameterAction.Update(currentTime);
                this.parameter = (byte)this.parameterAction.CurrentValue;//默认就是0,ok~
            }

            // 处理帧更新
            //
            if (this.frameCount <= 1) return;
            if (currentTime - this.msLastFrame < this.mSPerFrame) return;
            this.msLastFrame = currentTime;
            this.currentFrameIndex++;
            if (this.currentFrameIndex == this.frameCount)
            {
                if (this.Loop == ElementLoopType.LoopForever)
                {
                    this.currentFrameIndex = 0;
                }
                else
                {
                    this.currentFrameIndex = this.frameCount - 1;
                }
            }
            //this.texture = texturearray[currentFrameIndex];
            this.origin = Getorigin(this.texturearray[currentFrameIndex], this.Origin);
        }


        #region 设置动作
        /// <summary>
        /// 初始化精灵动作
        /// </summary>
        private void InitSpriteAction()
        {
            this.alphaAction = new TSpriteAction();
            this.xAction = new TSpriteAction();
            this.yAction = new TSpriteAction();
            this.scaleXAction = new TSpriteAction();
            this.scaleYAction = new TSpriteAction();
            this.rotateAction = new TSpriteAction();
            this.colorAction = new TSpriteAction();
            this.parameterAction = new TSpriteAction();
        }
        /// <summary>
        /// 设置Alpha动作
        /// </summary>
        public void SetAlphaAction(TSpriteAction pAlphaAction)
        {
            this.alphaAction = pAlphaAction;
        }
        /// <summary>
        /// 设置X轴动作
        /// </summary>
        public void SetXAction(TSpriteAction pXAction)
        {
            this.xAction = pXAction;
        }
        /// <summary>
        /// 设置Y轴动作
        /// </summary>
        public void SetYAction(TSpriteAction pYAction)
        {
            this.yAction = pYAction;
        }
        /// <summary>
        /// 设置ScaleX动作
        /// </summary>
        public void SetScaleXAction(TSpriteAction pScaleAction)
        {
            this.scaleXAction = pScaleAction;
        }
        /// <summary>
        /// 设置ScaleY动作
        /// </summary>
        public void SetScaleYAction(TSpriteAction pScaleAction)
        {
            this.scaleYAction = pScaleAction;
        }
        /// <summary>
        /// 设置Rotate动作
        /// </summary>
        public void SetRotateAction(TSpriteAction pRotateAction)
        {
            this.rotateAction = pRotateAction;
        }
        /// <summary>
        /// 设置Color动作
        /// </summary>
        public void SetColorAction(TSpriteAction pColorAction)
        {
            this.colorAction = pColorAction;
        }
        /// <summary>
        /// 设置Parameter动作
        /// </summary>
        public void SetParameterAction(TSpriteAction pParameterAction)
        {
            this.parameterAction = pParameterAction;
        }
        #endregion
    }
}
