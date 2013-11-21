using System;
using System.IO;
using OSUplayer.OsuFiles.StoryBoard;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Device = Microsoft.Xna.Framework.Graphics.GraphicsDevice;
using Color = Microsoft.Xna.Framework.Graphics.Color;
namespace OSUplayer.Graphic
{
    /// <summary>
    /// MDX静态图像基类
    /// </summary>
    class TStaticGraphic
    {
        protected Texture2D texture;
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


        public TStaticGraphic(Device graphicDevice, string source, Vector2 position,
                                          float rotate, float scale, Color color, byte alpha, byte parameter)
        {
            this.texture = Texture2D.FromFile(graphicDevice, source);
            this.origin = new Vector2(0, 0);
            this.position = position;
            this.rotate = rotate;
            this.scale = new Vector2(scale, scale);
            this.layer = 0;
            this.color = color;
            this.alpha = alpha;
            this.parameter = parameter;
            //this.rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
        }
        public TStaticGraphic() { }

        /// <summary>
        /// 绘制图像
        /// </summary>
        public virtual void Draw(SpriteBatch sprite)
        {
            SpriteEffects tmp = new SpriteEffects();
            if ((this.parameter & 1) == 1) { tmp = SpriteEffects.FlipHorizontally; }
            if ((this.parameter & 2) == 2) { tmp = tmp | SpriteEffects.FlipVertically; }
            this.color = new Color(color.R, color.G, color.B, this.alpha);
            sprite.Draw(this.texture, this.position, null, this.color, this.rotate, this.origin, this.scale, tmp, this.layer);
        }
    }


    /// <summary>
    /// MDX带有动作列表的图像基类
    /// </summary>
    class TGraphic : TStaticGraphic
    {
        protected int frameCount;  //材质行总数
        protected int currentFrameIndex; //当前材质行索引
        protected int mSPerFrame; // 帧动画的速率
        protected int msLastFrame; //上次记录的时间
        protected ElementLoopType Loop;
        protected TSpriteAction xAction;
        protected TSpriteAction yAction;
        protected TSpriteAction scaleXAction;
        protected TSpriteAction scaleYAction;
        protected TSpriteAction rotateAction;
        protected TSpriteAction alphaAction;
        protected TSpriteAction colorAction;
        protected TSpriteAction parameterAction;
        protected Texture2D[] texturearray;
        protected ElementOrigin Origin;

        public TGraphic(Device graphicDevice, SBelement Element, string Location,int layerdelta)
            : base()
        {
            switch (Element.Type)
            {
                case ElementType.Sprite:
                    {
                        if (File.Exists(Path.Combine(Location, Element.Path)))
                        {
                            using (FileStream s = new FileStream(Path.Combine(Location, Element.Path), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                this.texture = Texture2D.FromFile(graphicDevice, s);
                            }
                        }
                        else
                        {
                            this.texture = new Texture2D(graphicDevice, 1, 1, 0, 0, SurfaceFormat.Bgr32);
                        }
                        this.frameCount = 1;
                        this.currentFrameIndex = 0;
                        this.msLastFrame = 0;
                        this.mSPerFrame = 16;
                        this.position = new Vector2(Element.x, Element.y);
                        this.origin = Getorigin(this.texture, Element.Origin);
                        break;
                    }
                case ElementType.Animation:
                    {
                        string prefix = Path.Combine(Location, Element.Path);
                        string ext = prefix.Substring(prefix.LastIndexOf(".") + 1);
                        prefix = prefix.Substring(0, prefix.LastIndexOf("."));
                        texturearray = new Texture2D[Element.FrameCount];
                        for (int i = 0; i < Element.FrameCount; i++)
                        {
                            if (File.Exists(prefix + i.ToString() + "." + ext))
                            {
                                using (FileStream s = new FileStream(prefix + i.ToString() + "." + ext, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    this.texturearray[i] = Texture2D.FromFile(graphicDevice, s);
                                }
                            }
                            else
                            {
                                this.texturearray[i] = new Texture2D(graphicDevice, 1, 1, 0, 0, SurfaceFormat.Bgr32);
                            }
                        }
                        this.frameCount = Element.FrameCount;
                        this.mSPerFrame = Element.Framedelay;
                        this.Loop = Element.Looptype;
                        this.currentFrameIndex = 0;
                        this.msLastFrame = 0;
                        this.position = new Vector2(Element.x, Element.y);
                        this.texture = texturearray[0];
                        this.origin = Getorigin(this.texture, Element.Origin);
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
            if (Element.Layers == ElementLayer.Background) { this.layer = 0.9f - layerdelta*0.001f; }
            else { this.layer = 0.5f - layerdelta * 0.001f; }
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
        private Vector2 Getorigin(Texture2D texture, ElementOrigin Origin)
        {
            int id = (int)Origin;
            float x = (float)(id % 3) / 2 * texture.Width;
            float y = (float)(id - id % 3) / 6 * texture.Height;
            return new Vector2(x, y);
        }

        /// <summary>
        /// 更新图像
        /// </summary>
        public virtual void Update(int CurrentTime)
        {
            //处理动作更新
            //
            if (this.alphaAction.Enable)
            {
                this.alphaAction.Update(CurrentTime);
                this.alpha = (byte)alphaAction.CurrentValue;
            }
            if (this.colorAction.Enable)
            {
                this.colorAction.Update(CurrentTime);
                int colorv = (int)colorAction.CurrentValue;
                this.color = new Color((byte)(colorv >> 0x10), (byte)(colorv >> 8), (byte)colorv);
            }
            if (this.xAction.Enable)
            {
                this.xAction.Update(CurrentTime);
                this.position.X = this.xAction.CurrentValue;
            }
            if (this.yAction.Enable)
            {
                this.yAction.Update(CurrentTime);
                this.position.Y = this.yAction.CurrentValue;
            }
            if (this.scaleXAction.Enable)
            {
                this.scaleXAction.Update(CurrentTime);
                this.scale.X = this.scaleXAction.CurrentValue;
            }
            if (this.scaleYAction.Enable)
            {
                this.scaleYAction.Update(CurrentTime);
                this.scale.Y = this.scaleYAction.CurrentValue;
            }
            if (this.rotateAction.Enable)
            {
                this.rotateAction.Update(CurrentTime);
                this.rotate = this.rotateAction.CurrentValue;
            }
            if (this.parameterAction.Enable)
            {
                this.parameterAction.Update(CurrentTime);
                this.parameter = (byte)this.parameterAction.CurrentValue;
            }

            // 处理帧更新
            //
            if (this.frameCount > 1)
            {
                if (CurrentTime - this.msLastFrame >= this.mSPerFrame)
                {
                    this.msLastFrame = CurrentTime;
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
                    this.texture = texturearray[currentFrameIndex];
                    this.origin = Getorigin(this.texture, this.Origin);
                }
            }
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
