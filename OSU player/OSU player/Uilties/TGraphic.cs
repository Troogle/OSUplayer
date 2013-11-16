using System;
using System.Drawing;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using OSU_player.OSUFiles.StoryBoard;
namespace OSU_player.Graphic
{
    /// <summary>
    /// MDX静态图像基类
    /// </summary>
    class TStaticGraphic
    {
        protected Texture texture;
        protected Matrix scaleMatrix, transformMatrix, rotateMatrix;
        protected Vector3 center;                                         //旋转中心
        protected Vector3 position;                                       //显示位置
        protected float rotate;                                               //旋转角度
        protected float scaleX;                                                //缩放比例
        protected float scaleY;
        protected Color color;
        protected byte alpha;
        /// <summary>
        /// 0=none,1=H,2=V,3=A
        /// </summary>
        public byte parameter;
        protected Rectangle rectangle;


        public TStaticGraphic(Device graphicDevice, Bitmap bitmap, Vector3 position,
                                          float rotate, float scale, Color color, byte alpha, byte parameter)
        {
            this.texture = Texture.FromBitmap(graphicDevice, bitmap, Usage.Dynamic, Pool.Default);
            this.center = new Vector3(0, 0, 0);
            this.transformMatrix = new Matrix();
            this.rotateMatrix = new Matrix();
            this.scaleMatrix = new Matrix();
            this.position = position;
            this.rotate = rotate;
            this.scaleX = scale;
            this.scaleY = scale;
            this.color = color;
            this.alpha = alpha;
            this.parameter = parameter;
            this.rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        }
        public TStaticGraphic(Device graphicDevice)
        {
            this.center = new Vector3(0, 0, 0);
            this.transformMatrix = new Matrix();
            this.rotateMatrix = new Matrix();
            this.scaleMatrix = new Matrix();
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        public virtual void Draw(Sprite sprite)
        {
            this.rotateMatrix.RotateZ(this.rotate);
            if (this.parameter == 1) { this.scaleMatrix.Scale(-this.scaleX, this.scaleY, 0); }
            else if (this.parameter == 2) { this.scaleMatrix.Scale(this.scaleX, -this.scaleY, 0); }
            else { this.scaleMatrix.Scale(this.scaleX, this.scaleY, 0); }
            this.transformMatrix.Translate(this.position);
            sprite.Transform = this.rotateMatrix * this.scaleMatrix * this.transformMatrix;
            sprite.Draw(this.texture, this.rectangle, this.center, Vector3.Empty, Color.FromArgb(this.alpha, this.color));
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
        protected Texture[] texturearray;
        protected Rectangle[] rectarray;
        protected ElementOrigin Origin;

        public TGraphic(Device graphicDevice, SBelement Element, string Location)
            : base(graphicDevice)
        {
            Bitmap bitmap;
            switch (Element.Type)
            {
                case ElementType.Sprite:
                    {
                        if (File.Exists(Path.Combine(Location, Element.path)))
                        {
                            bitmap = new Bitmap(Path.Combine(Location, Element.path));
                        }
                        else { bitmap = new Bitmap(Properties.Resources.BlackBase, 1, 1); }
                        this.frameCount = 1;
                        this.currentFrameIndex = 0;
                        this.msLastFrame = 0;
                        this.mSPerFrame = 16;
                        this.Origin = Element.Origin;
                        this.texture = Texture.FromBitmap(graphicDevice, bitmap, Usage.Dynamic, Pool.Default);
                        this.rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        this.position = new Vector3(Element.x, Element.y, 0);
                        this.center = Getcenter(-1);
                        break;
                    }
                case ElementType.Animation:
                    {
                        string prefix = Path.Combine(Location, Element.path);
                        string ext = prefix.Substring(prefix.LastIndexOf(".") + 1);
                        prefix = prefix.Substring(0, prefix.LastIndexOf("."));
                        texturearray = new Texture[Element.frameCount];
                        rectarray = new Rectangle[Element.frameCount];
                        for (int i = 0; i < Element.frameCount; i++)
                        {
                            if (File.Exists(prefix + i.ToString() + "." + ext))
                            {
                                bitmap = new Bitmap(prefix + i.ToString() + "." + ext);
                            }
                            else { bitmap = new Bitmap(Properties.Resources.BlackBase, 1, 1); }
                            texturearray[i] = Texture.FromBitmap(graphicDevice, bitmap, Usage.Dynamic, Pool.Default);
                            rectarray[i] = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                        }
                        this.frameCount = Element.frameCount;
                        this.mSPerFrame = Element.framedelay;
                        this.Loop = Element.Looptype;
                        this.currentFrameIndex = 0;
                        this.msLastFrame = 0;
                        this.Origin = Element.Origin;
                        this.position = new Vector3(Element.x, Element.y, 0);
                        this.rectangle = rectarray[0];
                        this.texture = texturearray[0];
                        this.center = Getcenter(0);
                        break;
                    }
                default:
                    throw (new FormatException("Failed to resolve .osb file"));
            }
            this.color = Color.White;
            this.alpha = 0;
            this.parameter = 0;
            this.rotate = 0f;
            this.scaleX = 1f;
            this.scaleY = 1f;
            //  this.InitSpriteAction();
        }
        /// <summary>
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
        }
        private Vector3 Getcenter(int count)
        {
            Rectangle tmp;
            if (count == -1)
            {
                tmp = this.rectangle;
            }
            else
            {
                tmp = this.rectarray[count];
            }
            int id = (int)this.Origin;
            float x = (float)(id % 3) / 2 * tmp.Width;
            float y = (float)(id - id % 3) / 6 * tmp.Height;
            return new Vector3(x, y, 0);
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
                this.color = Color.FromArgb((int)colorAction.CurrentValue);
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
                this.scaleX = this.scaleXAction.CurrentValue;
            }
            if (this.scaleYAction.Enable)
            {
                this.scaleYAction.Update(CurrentTime);
                this.scaleY = this.scaleYAction.CurrentValue;
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
                    if (this.Loop == ElementLoopType.LoopForever && this.currentFrameIndex == this.frameCount)
                    {
                        this.currentFrameIndex = 0;
                    }
                    else
                    {
                        this.currentFrameIndex = this.frameCount - 1;
                    }
                    this.texture = texturearray[currentFrameIndex];
                    this.rectangle = rectarray[currentFrameIndex];
                    this.center = Getcenter(currentFrameIndex);
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
