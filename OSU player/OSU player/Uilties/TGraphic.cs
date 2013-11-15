using System;
using System.Drawing;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using OSU_player.StoryBoard;
namespace OSU_player
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
        protected float scale;                                                //缩放比例
        protected Color color;
        protected Rectangle rectangle;


        public TStaticGraphic(Device graphicDevice, Bitmap bitmap, Vector3 position,
                                          float rotate, float scale, Color color)
        {
            this.texture = Texture.FromBitmap(graphicDevice, bitmap, Usage.Dynamic, Pool.Default);
            this.center = new Vector3(0, 0, 0);
            this.transformMatrix = new Matrix();
            this.rotateMatrix = new Matrix();
            this.scaleMatrix = new Matrix();
            this.position = position;
            this.rotate = rotate;
            this.scale = scale;
            this.color = color;
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
            this.scaleMatrix.Scale(this.scale, this.scale, 0);
            this.transformMatrix.Translate(this.position);
            sprite.Transform = this.rotateMatrix * this.scaleMatrix * this.transformMatrix;
            sprite.Draw(this.texture, this.rectangle, this.center, Vector3.Empty, this.color);
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
        protected int msSinceLastFrame; //上一帧到现在的时间
        protected ElementLoopType Loop;
        protected TSpriteAction xAction;
        protected TSpriteAction yAction;
        protected TSpriteAction scaleAction;
        protected TSpriteAction rotateAction;
        protected TSpriteAction alphaAction;


        public TGraphic(Device graphicDevice, SBelement Element, string Location)
            : base(graphicDevice)
        {
            Bitmap bitmap;
            switch (Element.Type)
            {
                case ElementType.Sprite:
                    {
                        bitmap = new Bitmap(45 * 4, 60);
                        break;
                    }
                case ElementType.Animation:
                    {
                        Graphics resultGraphics;    //用来绘图的实例
                        bitmap = new Bitmap(45 * 4, 60);    //总高宽
                        resultGraphics = Graphics.FromImage(bitmap);
                        string[] numberImgPath = { "0.jpg", "3.jpg", "1.jpg", "7.jpg" };
                        for (int i = 0; i < numberImgPath.Length; i++)
                        {
                            resultGraphics.DrawImage(Image.FromFile(numberImgPath[i]), 45 * i, 60);
                        }
                        resultGraphics.Dispose();
                        break;
                    }
                default:
                    throw (new FormatException("Failed to resolve .osb file"));
            }
            this.texture = Texture.FromBitmap(graphicDevice, bitmap, Usage.Dynamic, Pool.Default);
            this.rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        }
        /// <summary>
        /// 创建一个多帧WGraphic实例
        /// </summary>
        /// <param name="graphicDevice">显示设备</param>
        /// <param name="bitmap">要绘制的图像</param>
        /// <param name="pPosition">绘制坐标</param>
        /// <param name="pCenter">旋转中心</param>
        /// <param name="pFrameCount">总帧数</param>
        /// <param name="pFPS">FPS</param>
        public TGraphic(Device graphicDevice, Bitmap bitmap, Vector3 pPosition, Vector3 pCenter,
                                 int pFrameCount, int pFPS)
            : base(graphicDevice, bitmap, pPosition, 0f, 1f, Color.White)
        {
            this.frameCount = pFrameCount;
            this.currentFrameIndex = 0;
            this.mSPerFrame = (int)(1f / pFPS * 1000);
            this.msSinceLastFrame = 0;
            this.center = pCenter;
            this.InitSpriteAction();
        }

        /// <summary>
        /// 创建一个单帧WGraphic实例
        /// </summary>
        /// <param name="graphicDevice">显示设备</param>
        /// <param name="bitmap">要绘制的图像</param>
        /// <param name="pPosition">绘制坐标</param>
        /// <param name="pCenter">旋转中心</param>
        public TGraphic(Device graphicDevice, Bitmap bitmap, Vector3 pPosition, Vector3 pCenter)
            : base(graphicDevice, bitmap, pPosition, 0f, 1f, Color.White)
        {
            this.center = pCenter;
            this.frameCount = 1;
            this.currentFrameIndex = 0;
            this.msSinceLastFrame = 0;
            this.mSPerFrame = 16;
            this.InitSpriteAction();
        }


        /// <summary>
        /// 更新图像
        /// </summary>
        public virtual void Update(int pElapsedMS)
        {
            //处理动作更新
            //
            if (this.alphaAction.Enable)
            {
                this.alphaAction.Update(pElapsedMS);
                this.color = Color.FromArgb((byte)alphaAction.CurrentValue, 255, 255, 255);
            }
            if (this.xAction.Enable)
            {
                this.xAction.Update(pElapsedMS);
                this.position.X = this.xAction.CurrentValue;
            }
            if (this.yAction.Enable)
            {
                this.yAction.Update(pElapsedMS);
                this.position.Y = this.yAction.CurrentValue;
            }
            if (this.scaleAction.Enable)
            {
                this.scaleAction.Update(pElapsedMS);
                this.scale = this.scaleAction.CurrentValue;
            }
            if (this.rotateAction.Enable)
            {
                this.rotateAction.Update(pElapsedMS);
                this.rotate = this.rotateAction.CurrentValue;
            }

            // 处理帧更新
            //
            if (this.frameCount > 1)
            {
                if (this.msSinceLastFrame + pElapsedMS >= this.mSPerFrame)
                {
                    this.msSinceLastFrame = 0;
                    this.rectangle.X = this.currentFrameIndex * this.rectangle.Width;
                    this.currentFrameIndex++;

                    if (this.currentFrameIndex == this.frameCount)
                    {
                        this.currentFrameIndex = 0;
                    }
                }
                else
                {
                    this.msSinceLastFrame += pElapsedMS;
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
            this.scaleAction = new TSpriteAction();
            this.rotateAction = new TSpriteAction();
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
        /// 设置Scale动作
        /// </summary>
        public void SetScaleAction(TSpriteAction pScaleAction)
        {
            this.scaleAction = pScaleAction;
        }
        /// <summary>
        /// 设置Rotate动作
        /// </summary>
        public void SetRotateAction(TSpriteAction pRotateAction)
        {
            this.rotateAction = pRotateAction;
        }
        #endregion
    }
}
