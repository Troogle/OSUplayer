using System;
using System.Collections.Generic;
namespace OSU_player.Graphic
{
    /// <summary>
    /// 精灵动作
    /// </summary>
    class TSpriteAction
    {
        private float currentValue, k, a, b, prevalue;
        private int currentIndex;
        private TActionNode[] actionList;
        private int timeCount;
        private int easing;
        private bool isLoop;  //动作是否循环
        private bool isEnable;  //动作是否激活
        private bool isKeepValue;  //非循环动作在结束后是否保持值
        private bool isOver;  //动作是否执行完

        #region 访问器
        /// <summary>
        /// 获取动作的值
        /// </summary>
        public float CurrentValue
        {
            get { return this.currentValue; }
        }
        /// <summary>
        /// 获取该动作是否可用（即动作结点数量大于等于1）
        /// </summary>
        public bool Enable
        {
            get { return isEnable; }
        }
        #endregion


        /// <summary>
        /// 创建一个精灵动作实例
        /// </summary>
        /// <param name="pActionList">动作列表</param>
        /// <param name="pEasing">缓冲设定</param>
        /// <param name="pIsLoop">是否循环</param>
        /// <param name="pIsKeepValue">动作结束后是否保持值</param>
        public TSpriteAction(List<TActionNode> pActionList, bool pIsLoop = false, bool pIsKeepValue = true)
        {
            if (pActionList.Count == 0)
            {
                this.actionList = new TActionNode[0];
                this.isLoop = false;
                this.isKeepValue = false;
                this.currentIndex = 0;
                this.currentValue = 0;
                this.k = 0;
                this.b = 0;
                this.timeCount = 0;
                this.isEnable = false;
                this.isOver = true;
            }
            else
            {
                this.actionList = new TActionNode[pActionList.Count];
                for (int i = 0; i < pActionList.Count; i++)
                {
                    this.actionList[i] = new TActionNode(pActionList[i].Time, pActionList[i].Value, pActionList[i].Easing);
                }
                this.isLoop = pIsLoop;
                this.isKeepValue = pIsKeepValue;
                this.Reset();
                this.prevalue = 0;
                this.easing = 0;
            }
        }


        /// <summary>
        /// 创建一个空白精灵动作实例
        /// </summary>
        public TSpriteAction()
        {
            this.actionList = new TActionNode[0];
            this.isLoop = false;
            this.isKeepValue = false;
            this.currentIndex = 0;
            this.currentValue = 0;
            this.k = 0;
            this.b = 0;
            this.timeCount = 0;
            this.isEnable = false;
            this.isOver = true;
        }


        /// <summary>
        /// 更新动作的值
        /// </summary>
        public void Update(int CurrentTime)
        {
            if (!this.isOver)
            {
                this.timeCount = CurrentTime;
            }

            //如果结点列表只有一个元素，则到达时间点时直接赋值
            if (this.actionList.Length == 1)
            {
                if (timeCount >= actionList[0].Time)
                {
                    this.currentValue = actionList[0].Value;
                }
            }
            else
            {
                //否则根据结点设置k和b
                if (timeCount >= actionList[currentIndex].Time)
                {
                    if (this.currentIndex < this.actionList.Length - 1)
                    {
                        this.a = this.actionList[this.currentIndex + 1].Value - this.actionList[this.currentIndex].Value;
                        this.b = this.actionList[this.currentIndex + 1].Time - this.actionList[this.currentIndex].Time;
                        this.k = a / b;
                        this.prevalue = this.actionList[this.currentIndex].Value;
                        this.easing = this.actionList[this.currentIndex].Easing;
                        this.currentIndex++;
                    }
                    //如果要循环则重新开始
                    else
                    {
                        if (this.isLoop)
                        {
                            this.currentIndex = 0;
                            this.timeCount = 0;
                        }
                        else
                        {
                            this.isOver = true;
                            if (this.isKeepValue)
                            {
                                this.currentValue = this.actionList[this.actionList.Length - 1].Value;
                                return;
                            }
                            else
                            {
                                this.isEnable = false;
                                this.currentValue = 0;
                                return;
                            }
                        }
                    }
                }
                int elapsedTime = timeCount - actionList[currentIndex].Time;
                switch (this.easing)
                {
                    case 0:
                        this.currentValue = k * elapsedTime + prevalue;
                        break;
                    case 1:
                        this.currentValue = (float)Math.Sqrt(b * b - k * k * (elapsedTime - a) * (elapsedTime - a)) + prevalue;
                        break;
                    case 2:
                        this.currentValue = b - (float)Math.Sqrt(b * b - k * k * elapsedTime * elapsedTime) + prevalue;
                        break;
                    case 3:
                        this.currentValue = prevalue;
                        break;
                    default:
                        this.currentValue = k * elapsedTime + prevalue;
                        break;
                }
                //根据k和b算出值
            }
        }



        /// <summary>
        /// 设置某一结点的值
        /// </summary>
        /// <param name="pIndex">结点索引</param>
        /// <param name="pValue">欲设置的值</param>
        public void SetValue(int pIndex, float pValue)
        {
            this.actionList[pIndex].Value = pValue;
        }


        /// <summary>
        /// 设置某一结点的时间
        /// </summary>
        /// <param name="pIndex">结点索引</param>
        /// <param name="pTime">欲设置的时间</param>
        public void SetTime(int pIndex, int pTime)
        {
            this.actionList[pIndex].Time = pTime;
        }


        /// <summary>
        /// 重新开始动作
        /// </summary>
        public void Reset()
        {
            this.currentIndex = 0;
            this.currentValue = 0;
            this.k = 0;
            this.b = 0;
            this.timeCount = 0;
            this.isEnable = true;
            this.isOver = false;
        }
    }


    /// <summary>
    /// 动作结点
    /// </summary>
    public class TActionNode
    {
        private int time;
        private float value;
        private int easing;
        #region 访问器
        /// <summary>
        /// 获取或设置结点的时间
        /// </summary>
        public int Time
        {
            set { this.time = value; }
            get { return this.time; }
        }
        /// <summary>
        /// 获取或设置结点的动作值
        /// </summary>
        public float Value
        {
            set { this.value = value; }
            get { return this.value; }
        }
        /// <summary>
        /// 获取或设置结点的缓冲值
        /// </summary>
        public int Easing
        {
            set { this.easing = value; }
            get { return this.easing; }
        }
        public TActionNode(int pTime, float pValue,int pEasing)
        {
            this.time = pTime;
            this.value = pValue;
            this.easing = pEasing;
        }
        #endregion
    }
}
