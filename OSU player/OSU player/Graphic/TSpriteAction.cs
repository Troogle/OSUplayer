using System;
using System.Collections.Generic;
namespace OSUplayer.Graphic
{
    /// <summary>
    /// 精灵动作
    /// </summary>
    class TSpriteAction
    {
        private float currentValue, k, a, b;
        private int currentIndex;
        private List<TActionNode> actionList;
        private int easing;    //计算类型
        private bool isLoop;  //动作是否循环
        private bool isEnable;  //动作是否激活
        private bool isActive; //是否是首项
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
                this.isEnable = false;
                this.isActive = true;
            }
            else
            {
                actionList = pActionList;
                this.isLoop = pIsLoop;
                this.isKeepValue = pIsKeepValue;
                this.isActive = false;
                this.Reset();
            }
        }


        /// <summary>
        /// 创建一个空白精灵动作实例
        /// </summary>
        public TSpriteAction()
        {
            //this.actionList = new TActionNode[0];
            this.isLoop = false;
            this.isKeepValue = false;
            this.currentIndex = 0;
            this.currentValue = 0;
            this.k = 0;
            this.b = 0;
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
                if (this.actionList.Count == 1)
                {
                    if (CurrentTime >= actionList[0].Time)
                    {
                        this.currentValue = actionList[0].Value;
                        this.isOver = true;
                        this.isEnable = false;
                    }
                }
                else
                {
                    if (!this.isActive)
                    {
                        if (CurrentTime >= actionList[0].Time)
                        {
                            this.a = this.actionList[1].Value - this.actionList[0].Value;
                            this.b = this.actionList[1].Time - this.actionList[0].Time;
                            this.k = a / b;
                            this.easing = this.actionList[0].Easing;
                            this.currentValue = this.actionList[0].Value;
                            this.isActive = true;
                            return;
                        }
                    }
                    else
                    {
                        //根据结点设置k和b
                        if (CurrentTime >= actionList[currentIndex + 1].Time)//到了下一个节点，更新abk
                        {
                            if (this.currentIndex < this.actionList.Count - 2)//不是最后一个
                            {
                                this.currentIndex++;
                                this.a = this.actionList[currentIndex + 1].Value - this.actionList[currentIndex].Value;
                                this.b = this.actionList[currentIndex + 1].Time - this.actionList[currentIndex].Time;
                                this.k = a / b;
                                this.easing = this.actionList[currentIndex].Easing;
                                this.currentValue = this.actionList[currentIndex].Value;
                                return;
                            }
                            else
                            {
                                this.isOver = true;
                                if (this.isKeepValue)
                                {
                                    this.currentValue = this.actionList[this.actionList.Count - 1].Value;
                                    return;
                                }
                                else
                                {
                                    this.isEnable = false;
                                    this.currentValue = 0;
                                    return;
                                }
                            }
                            /*   if (this.easing == 3)
                                {
                                    if (!isActive)
                                    {
                                        this.currentValue = this.actionList[currentIndex].Value;
                                        this.isActive = true;
                                        return;
                                    }
                                    this.currentValue = (int)this.currentValue | (int)this.actionList[this.currentIndex].Value;
                                    if (CurrentTime >= actionList[currentIndex].ETime)
                                    {
                                        this.currentValue = (int)this.currentValue & (~(int)this.actionList[this.currentIndex].SValue);
                                        if (this.currentIndex < this.actionList.Count - 1)
                                        {
                                            currentIndex++;
                                            this.currentValue = (int)this.currentValue | (int)this.actionList[this.currentIndex].SValue;
                                        }
                                        else
                                        {
                                            this.isOver = true;
                                            this.currentValue = 0;
                                        }
                                    }
                                    return;
                                }*/
                        }
                        //根据k和b算出值
                        int elapsedTime = CurrentTime - this.actionList[this.currentIndex].Time;
                        switch (this.easing)
                        {
                            case 0:
                                this.currentValue = k * elapsedTime + this.actionList[this.currentIndex].Value;
                                break;
                            case 1:
                                this.currentValue = (float)Math.Sqrt(b * b - k * k * (elapsedTime - a) * (elapsedTime - a))
                                    + this.actionList[this.currentIndex].Value;
                                break;
                            case 2:
                                this.currentValue = b - (float)Math.Sqrt(b * b - k * k * elapsedTime * elapsedTime)
                                    + this.actionList[this.currentIndex].Value;
                                break;
                            case 4:  //endtime之后
                                break; //currentvalue不变
                            default:
                                break;
                        }
                    }
                }
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
            this.isEnable = true;
            this.isOver = false;
            this.easing = 0;
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
        public TActionNode(int pTime, float pValue, int pEasing)
        {
            this.time = pTime;
            this.value = pValue;
            this.easing = pEasing;
        }
        #endregion
    }
}
