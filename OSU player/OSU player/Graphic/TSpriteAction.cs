using System;
using System.Collections.Generic;

namespace OSUplayer.Graphic
{
    /// <summary>
    ///     精灵动作
    /// </summary>
    internal class TSpriteAction
    {
        private readonly List<TActionNode> _actionList;
        private readonly bool _isKeepValue; //非循环动作在结束后是否保持值
        private float _k,_a, _b;
        private int _currentIndex;
        private int _easing; //计算类型
        public bool IsActive; //是否是首项
        private bool _isLoop; //动作是否循环
        private bool _isOver; //动作是否执行完

        #region 访问器

        /// <summary>
        ///     获取动作的值
        /// </summary>
        public float CurrentValue { get; private set; }

        /// <summary>
        ///     获取该动作是否可用（即动作结点数量大于等于1）
        /// </summary>
        public bool Enable { get; private set; }

        #endregion

        /// <summary>
        ///     创建一个精灵动作实例
        /// </summary>
        /// <param name="pActionList">动作列表</param>
        /// <param name="pIsLoop">是否循环</param>
        /// <param name="pIsKeepValue">动作结束后是否保持值</param>
        public TSpriteAction(List<TActionNode> pActionList, bool pIsLoop = false, bool pIsKeepValue = true)
        {
            if (pActionList.Count == 0)
            {
                Enable = false;
                IsActive = false;
            }
            else
            {
                _actionList = pActionList;
                _actionList.Sort();
                _isLoop = pIsLoop;
                _isKeepValue = pIsKeepValue;
                IsActive = false;
                Reset();
            }
        }


        /// <summary>
        ///     创建一个空白精灵动作实例
        /// </summary>
        public TSpriteAction()
        {
            //this.actionList = new TActionNode[0];
            _isLoop = false;
            _isKeepValue = false;
            _currentIndex = 0;
            CurrentValue = 0;
            _k = 0;
            _b = 0;
            Enable = false;
            _isOver = true;
        }


        /// <summary>
        ///     更新动作的值
        /// </summary>
        public void Update(int currentTime)
        {
            if (_isOver) return;
            if (_actionList.Count == 1)
            {
                if (currentTime < _actionList[0].Time) return;
                CurrentValue = _actionList[0].Value;
                _isOver = true;
                IsActive = true;
                Enable = false;
            }
            else
            {
                if (!IsActive)
                {
                    if (currentTime >= _actionList[0].Time)
                    {
                        _a = _actionList[1].Value - _actionList[0].Value;
                        _b = _actionList[1].Time - _actionList[0].Time;
                        _k = _a/_b;
                        _easing = _actionList[0].Easing;
                        CurrentValue = _actionList[0].Value;
                        IsActive = true;
                    }
                }
                else
                {
                    //根据结点设置k和b
                    if (currentTime >= _actionList[_currentIndex + 1].Time)
                    {
                        // while //跳帧
                        //     (this.currentIndex < this.actionList.Count - 2//不是最后一个
                        //     && CurrentTime >= actionList[currentIndex + 1].Time)//到了下一个节点，更新abk
                        //  {

                        //   }
                        if (_currentIndex < _actionList.Count - 2)
                        {
                            _currentIndex++;
                            _a = _actionList[_currentIndex + 1].Value - _actionList[_currentIndex].Value;
                            _b = _actionList[_currentIndex + 1].Time - _actionList[_currentIndex].Time;
                            _k = _a/_b;
                            _easing = _actionList[_currentIndex].Easing;
                            CurrentValue = _actionList[_currentIndex].Value;
                        }
                        else
                        {
                            _isOver = true;
                            if (_isKeepValue)
                            {
                                CurrentValue = _actionList[_actionList.Count - 1].Value;
                                return;
                            }
                            Enable = false;
                            CurrentValue = 0;
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

                    //根据k和b算出值
                    int elapsedTime = currentTime - _actionList[_currentIndex].Time;
                    switch (_easing)
                    {
                        case 0:
                            CurrentValue = _k*elapsedTime + _actionList[_currentIndex].Value;
                            break;
                        case 1:
                            CurrentValue = (float) Math.Sqrt(_b*_b - _k*_k*(elapsedTime - _a)*(elapsedTime - _a))
                                           + _actionList[_currentIndex].Value;
                            break;
                        case 2:
                            CurrentValue = _b - (float) Math.Sqrt(_b*_b - _k*_k*elapsedTime*elapsedTime)
                                           + _actionList[_currentIndex].Value;
                            break;
                        case 4: //endtime之后
                            CurrentValue = _actionList[_currentIndex].Value;
                            break; //currentvalue不变
                    }
                }
            }
        }


        /// <summary>
        ///     设置某一结点的值
        /// </summary>
        /// <param name="pIndex">结点索引</param>
        /// <param name="pValue">欲设置的值</param>
        public void SetValue(int pIndex, float pValue)
        {
            _actionList[pIndex].Value = pValue;
        }


        /// <summary>
        ///     设置某一结点的时间
        /// </summary>
        /// <param name="pIndex">结点索引</param>
        /// <param name="pTime">欲设置的时间</param>
        public void SetTime(int pIndex, int pTime)
        {
            _actionList[pIndex].Time = pTime;
        }


        /// <summary>
        ///     重新开始动作
        /// </summary>
        private void Reset()
        {
            _currentIndex = 0;
            CurrentValue = 0;
            _k = 0;
            _b = 0;
            Enable = true;
            _isOver = false;
            _easing = 0;
        }
    }


    /// <summary>
    ///     动作结点
    /// </summary>
    public class TActionNode : IComparable<TActionNode>
    {
        private int _time;

        #region 访问器

        public TActionNode(int pTime, float pValue, int pEasing)
        {
            _time = pTime;
            Value = pValue;
            Easing = pEasing;
        }

        /// <summary>
        ///     获取或设置结点的时间
        /// </summary>
        public int Time
        {
            set { _time = value; }
            get { return _time; }
        }

        /// <summary>
        ///     获取或设置结点的动作值
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        ///     获取或设置结点的缓冲值
        /// </summary>
        public int Easing { get; private set; }

        #endregion

        public int CompareTo(TActionNode other)
        {
            return _time.CompareTo(other._time);
        }
    }
}