/////////////////////////////////////////////////////////////////////////////
//
//  Script   : MathMatrix.cs
//  Info     : 数学辅助计算类 - 矩阵
//  Author   : ls9512 / Internet
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2017 / Internet http://wiki.unity3d.com/index.php/MathfMatrix
//
/////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;

namespace Aya.Maths
{
    public class MathMatrix
    {
        #region Matrix

        public float[] Matrix;

        public MathMatrix()
        {
            LoadIdentity();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void LoadIdentity()
        {
            Matrix = new float[16];

            for (var x = 0; x < 16; ++x)
            {
                Matrix[x] = 0;
            }

            Matrix[0] = 1;
            Matrix[5] = 1;
            Matrix[10] = 1;
            Matrix[15] = 1;
        }

        #endregion

        #region Get Translate Matrix

        /// <summary>
        /// 获得平移矩阵 - XYZ
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <returns>结果</returns>
        public static MathMatrix Translate(float x, float y, float z)
        {
            var wk = new MathMatrix
            {
                Matrix =
                {
                    [12] = x,
                    [13] = y,
                    [14] = z
                }
            };


            return wk;
        }

        #endregion

        #region Get Rotate Matrix

        /// <summary>
        /// 获得旋转矩阵 - X
        /// </summary>
        /// <param name="degree">角度</param>
        /// <returns>结果</returns>
        public static MathMatrix RotateX(float degree)
        {
            var wk = new MathMatrix();

            if (Math.Abs(degree) < 1e-6)
            {
                return wk;
            }

            var c = Mathf.Cos(degree * Mathf.Deg2Rad);
            var s = Mathf.Sin(degree * Mathf.Deg2Rad);

            wk.Matrix[5] = c;
            wk.Matrix[6] = s;
            wk.Matrix[9] = -s;
            wk.Matrix[10] = c;

            return wk;
        }

        /// <summary>
        /// 获得旋转矩阵 - Y
        /// </summary>
        /// <param name="degree">角度</param>
        /// <returns>结果</returns>
        public static MathMatrix RotateY(float degree)
        {
            var wk = new MathMatrix();

            if (Math.Abs(degree) < 1e-6)
            {
                return wk;
            }

            var c = Mathf.Cos(degree * Mathf.Deg2Rad);
            var s = Mathf.Sin(degree * Mathf.Deg2Rad);

            wk.Matrix[0] = c;
            wk.Matrix[2] = -s;
            wk.Matrix[8] = s;
            wk.Matrix[10] = c;

            return wk;
        }

        /// <summary>
        /// 获得旋转矩阵 - Z
        /// </summary>
        /// <param name="degree">角度</param>
        /// <returns>结果</returns>
        public static MathMatrix RotateZ(float degree)
        {
            var wk = new MathMatrix();

            if (Math.Abs(degree) < 1e-6)
            {
                return wk;
            }

            var c = Mathf.Cos(degree * Mathf.Deg2Rad);
            var s = Mathf.Sin(degree * Mathf.Deg2Rad);

            wk.Matrix[0] = c;
            wk.Matrix[1] = s;
            wk.Matrix[4] = -s;
            wk.Matrix[5] = c;

            return wk;
        }

        #endregion

        #region Get Scale Matrix

        /// <summary>
        /// 获得缩放矩阵 - XYZ
        /// </summary>
        /// <param name="_In">倍数</param>
        /// <returns>结果</returns>
        public static MathMatrix Scale(float _In)
        {
            return Scale3D(_In, _In, _In);
        }

        /// <summary>
        /// 获得缩放矩阵 - XYZ
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <returns>结果</returns>
        public static MathMatrix Scale3D(float x, float y, float z)
        {
            var wk = new MathMatrix
            {
                Matrix =
                {
                    [0] = x,
                    [5] = y,
                    [10] = z
                }
            };


            return wk;
        }

        #endregion

        #region Transform Vector

        /// <summary>
        /// 平移向量
        /// </summary>
        /// <param name="v">向量</param>
        /// <returns>结果</returns>
        public Vector3 TransformVector(Vector3 v)
        {
            return new Vector3(0, 0, 0)
            {
                x = (v.x * Matrix[0]) + (v.y * Matrix[4]) + (v.z * Matrix[8]) + Matrix[12],
                y = (v.x * Matrix[1]) + (v.y * Matrix[5]) + (v.z * Matrix[9]) + Matrix[13],
                z = (v.x * Matrix[2]) + (v.y * Matrix[6]) + (v.z * Matrix[10]) + Matrix[14]
            };
            ;
        }

        #endregion

        #region Operator

        /// <summary>
        /// 平移向量(运算*符重载)
        /// </summary>
        /// <param name="a">矩阵</param>
        /// <param name="v">向量</param>
        /// <returns>结果</returns>
        public static Vector3 operator *(MathMatrix a, Vector3 v)
        {
            return a.TransformVector(v);
        }

        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="a">矩阵a</param>
        /// <param name="b">矩阵b</param>
        /// <returns>结果</returns>
        public static MathMatrix operator *(MathMatrix a, MathMatrix b)
        {
            var wk = new MathMatrix
            {
                Matrix =
                {
                    [0] = a.Matrix[0] * b.Matrix[0] + a.Matrix[4] * b.Matrix[1] + a.Matrix[8] * b.Matrix[2] +
                          a.Matrix[12] * b.Matrix[3],
                    [4] = a.Matrix[0] * b.Matrix[4] + a.Matrix[4] * b.Matrix[5] + a.Matrix[8] * b.Matrix[6] +
                          a.Matrix[12] * b.Matrix[7],
                    [8] = a.Matrix[0] * b.Matrix[8] + a.Matrix[4] * b.Matrix[9] + a.Matrix[8] * b.Matrix[10] +
                          a.Matrix[12] * b.Matrix[11],
                    [12] = a.Matrix[0] * b.Matrix[12] + a.Matrix[4] * b.Matrix[13] + a.Matrix[8] * b.Matrix[14] +
                           a.Matrix[12] * b.Matrix[15],
                    [1] = a.Matrix[1] * b.Matrix[0] + a.Matrix[5] * b.Matrix[1] + a.Matrix[9] * b.Matrix[2] +
                          a.Matrix[13] * b.Matrix[3],
                    [5] = a.Matrix[1] * b.Matrix[4] + a.Matrix[5] * b.Matrix[5] + a.Matrix[9] * b.Matrix[6] +
                          a.Matrix[13] * b.Matrix[7],
                    [9] = a.Matrix[1] * b.Matrix[8] + a.Matrix[5] * b.Matrix[9] + a.Matrix[9] * b.Matrix[10] +
                          a.Matrix[13] * b.Matrix[11],
                    [13] = a.Matrix[1] * b.Matrix[12] + a.Matrix[5] * b.Matrix[13] + a.Matrix[9] * b.Matrix[14] +
                           a.Matrix[13] * b.Matrix[15],
                    [2] = a.Matrix[2] * b.Matrix[0] + a.Matrix[6] * b.Matrix[1] + a.Matrix[10] * b.Matrix[2] +
                          a.Matrix[14] * b.Matrix[3],
                    [6] = a.Matrix[2] * b.Matrix[4] + a.Matrix[6] * b.Matrix[5] + a.Matrix[10] * b.Matrix[6] +
                          a.Matrix[14] * b.Matrix[7],
                    [10] = a.Matrix[2] * b.Matrix[8] + a.Matrix[6] * b.Matrix[9] + a.Matrix[10] * b.Matrix[10] +
                           a.Matrix[14] * b.Matrix[11],
                    [14] = a.Matrix[2] * b.Matrix[12] + a.Matrix[6] * b.Matrix[13] + a.Matrix[10] * b.Matrix[14] +
                           a.Matrix[14] * b.Matrix[15],
                    [3] = a.Matrix[3] * b.Matrix[0] + a.Matrix[7] * b.Matrix[1] + a.Matrix[11] * b.Matrix[2] +
                          a.Matrix[15] * b.Matrix[3],
                    [7] = a.Matrix[3] * b.Matrix[4] + a.Matrix[7] * b.Matrix[5] + a.Matrix[11] * b.Matrix[6] +
                          a.Matrix[15] * b.Matrix[7],
                    [11] = a.Matrix[3] * b.Matrix[8] + a.Matrix[7] * b.Matrix[9] + a.Matrix[11] * b.Matrix[10] +
                           a.Matrix[15] * b.Matrix[11],
                    [15] = a.Matrix[3] * b.Matrix[12] + a.Matrix[7] * b.Matrix[13] + a.Matrix[11] * b.Matrix[14] +
                           a.Matrix[15] * b.Matrix[15]
                }
            };
            return wk;
        }

        #endregion
    }
}