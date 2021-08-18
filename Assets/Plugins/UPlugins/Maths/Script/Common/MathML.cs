/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathML.cs
//  Info     : 数学辅助计算类 —— 机器学习相关
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Maths
{
    public static partial class MathUtil
    {
        /// <summary>
        /// Logistic 逻辑回归<para/>
        /// 将任意值拟合到 (0,1)
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static float Sigmoid(float value)
        {
            var result = (float)(1f / (1f + Math.Pow(Math.E, -value)));
            return result;
        }

        /// <summary>
        /// Logistic 逻辑回归指定值的曲线导数<para/>
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static float SigmoidDerivative(float value)
        {
            var result = Sigmoid(value) * Sigmoid(1f - Sigmoid(value));
            return result;
        }

        /// <summary>
        /// 非线性激励函数 修正线性单元
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>结果</returns>
        public static float ReLU(float value)
        {
            var result = Math.Max(0f, value);
            return result;
        }
    }
}
