/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AnimatorExtension.cs
//  Info     : Animator 扩展方法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UIElements;

namespace Aya.Extension
{
    public static class AnimatorExtension
    {
        #region Parameter

        /// <summary>
        /// 重置所有参数
        /// </summary>
        /// <param name="animator">动画状态机</param>
        public static void ResetParameters(this Animator animator)
        {
            for (var i = 0; i < animator.parameters.Length; i++)
            {
                var param = animator.parameters[i];
                switch (param.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        animator.SetBool(param.name, param.defaultBool);
                        break;
                    case AnimatorControllerParameterType.Int:
                        animator.SetInteger(param.name, param.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Float:
                        animator.SetFloat(param.name, param.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        animator.ResetTrigger(param.name);
                        break;
                }
            }
        }

        /// <summary>
        /// 检查是否存在参数
        /// </summary>
        /// <param name="animator">动画状态机</param>
        /// <param name="parameterName">参数名</param>
        /// <returns>结果</returns>
        public static bool CheckParameterExist(this Animator animator, string parameterName)
        {
            for (var i = 0; i < animator.parameters.Length; i++)
            {
                var parameter = animator.parameters[i];
                if (parameter.name == parameterName) return true;
            }

            return false;
        }

        /// <summary>
        /// 检查是否存在参数
        /// </summary>
        /// <param name="animator">动画状态机</param>
        /// <param name="parameterName">参数名</param>
        /// <param name="type">参数类型</param>
        /// <returns>结果</returns>
        public static bool CheckParameterExist(this Animator animator, string parameterName, AnimatorControllerParameterType type)
        {
            for (var i = 0; i < animator.parameters.Length; i++)
            {
                var parameter = animator.parameters[i];
                if (parameter.name == parameterName && parameter.type == type) return true;
            }

            return false;
        }

        #endregion

        #region State

        /// <summary>
        /// 检查是否存在状态
        /// </summary>
        /// <param name="animator">动画状态机</param>
        /// <param name="clipName">状态名</param>
        /// <returns>结果</returns>
        public static bool CheckStateExist(this Animator animator, string clipName)
        {
            var controller = animator.runtimeAnimatorController;
            if (controller == null) return false;
            var clips = controller.animationClips;
            for (var i = 0; i < clips.Length; i++)
            {
                var clip = clips[i];
                if (clip.name == clipName) return true;
            }

            return false;
        }

        #endregion
    }
}
