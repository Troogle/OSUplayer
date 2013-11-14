using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
namespace OSU_player
{
    class TGraphics
    {        
        protected Texture texture;
        protected Matrix scaleMatrix, transformMatrix, rotateMatrix;

        protected Vector3 center;                                         //旋转中心
        protected Vector3 position;                                       //显示位置
        protected Vector3 zero;
        protected float rotate;                                               //旋转角度
        protected float scale;                                                //缩放比例
        protected Color color;
        protected Rectangle rectangle;


        public TGraphics(Device graphicDevice, Bitmap bitmap, Vector3 position,
                                          float rotate, float scale, byte alpha)
        {
            this.texture = Texture.FromBitmap(graphicDevice, bitmap, Usage.Dynamic, Pool.Default);
            this.center = new Vector3(0, 0, 0);
            this.transformMatrix = new Matrix();
            this.rotateMatrix = new Matrix();
            this.scaleMatrix = new Matrix();
            this.position = position;
            this.zero = new Vector3();
            this.rotate = rotate;
            this.scale = scale;
            this.color = Color.FromArgb(alpha, 255, 255, 255);
            this.rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
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
            sprite.Draw(this.texture, this.rectangle, this.center, this.zero, this.color);
        }
    }
}
