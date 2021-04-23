using System;
using System.Drawing;
using System.Threading;
using PInvoke;

namespace Dreamland.Core.Simulate
{
    /// <summary>
    ///     提供鼠标相关的模拟操作
    /// </summary>
    public static class Mouse
    {
        /// <summary>
        ///     鼠标事件延时
        /// </summary>
        private const int MouseDelay = 100;

        #region Click

        /// <summary>
        ///     单击鼠标按钮。
        /// </summary>
        /// <param name="buttons">点击的鼠标按钮，默认使用鼠标左键。</param>
        public static void Click(MouseButtons buttons = MouseButtons.Left)
        {
            if (!GetCursorPos(out var point))
            {
                return;
            }

            var flags = User32.mouse_eventFlags.MOUSEEVENTF_LEFTDOWN | User32.mouse_eventFlags.MOUSEEVENTF_LEFTUP;
            switch (buttons)
            {
                case MouseButtons.Left:
                    break;
                case MouseButtons.Right:
                    flags = User32.mouse_eventFlags.MOUSEEVENTF_RIGHTDOWN | User32.mouse_eventFlags.MOUSEEVENTF_RIGHTUP;
                    break;
                case MouseButtons.Middle:
                    flags = User32.mouse_eventFlags.MOUSEEVENTF_MIDDLEDOWN | User32.mouse_eventFlags.MOUSEEVENTF_MIDDLEUP;
                    break;
                case MouseButtons.XButton:
                    flags = User32.mouse_eventFlags.MOUSEEVENTF_XDOWN | User32.mouse_eventFlags.MOUSEEVENTF_XUP;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }

            User32.mouse_event(flags | User32.mouse_eventFlags.MOUSEEVENTF_ABSOLUTE, point.X, point.Y, 0, IntPtr.Zero);
        }

        /// <summary>
        ///     在指定位置单击一次
        /// </summary>
        /// <param name="point">指定的点击坐标</param>
        /// <param name="isAbsolute">使用绝对坐标时为 True，使用相对坐标时为 False。</param>
        public static void Click(Point point, bool isAbsolute = true)
        {
            Click(MouseButtons.Left, point, isAbsolute);
        }

        /// <summary>
        ///     在指定位置单击一次
        /// </summary>
        /// <param name="buttons">点击的鼠标按钮。</param>
        /// <param name="point">指定的点击坐标</param>
        /// <param name="isAbsolute">使用绝对坐标时为 True，使用相对坐标时为 False。</param>
        public static void Click(MouseButtons buttons, Point point, bool isAbsolute = true)
        {
            Move(point, isAbsolute);
            Thread.Sleep(MouseDelay);
            Click(buttons);
        }

        #endregion

        #region DoubleClick

        /// <summary>
        ///     在光标当前位置双击一次
        /// </summary>
        /// <param name="buttons">点击的鼠标按钮，默认使用鼠标左键。</param>
        public static void DoubleClick(MouseButtons buttons = MouseButtons.Left)
        {
            Click(buttons);
            Thread.Sleep(MouseDelay);
            Click(buttons);
        }

        /// <summary>
        ///     在指定位置双击一次
        /// </summary>
        /// <param name="point">指定的点击坐标</param>
        /// <param name="isAbsolute">使用绝对坐标时为 True，使用相对坐标时为 False。</param>
        public static void DoubleClick(Point point, bool isAbsolute = true)
        {
            DoubleClick(MouseButtons.Left, point, isAbsolute);
        }

        /// <summary>
        ///     在指定位置双击一次
        /// </summary>
        /// <param name="buttons">点击的鼠标按钮。</param>
        /// <param name="point">指定的点击坐标</param>
        /// <param name="isAbsolute">使用绝对坐标时为 True，使用相对坐标时为 False。</param>
        public static void DoubleClick(MouseButtons buttons, Point point, bool isAbsolute = true)
        {
            Move(point, isAbsolute);
            Thread.Sleep(MouseDelay);
            DoubleClick(buttons);
        }

        #endregion

        #region Postion

        /// <summary>
        ///     获取鼠标位置
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns></returns>
        public static bool GetCursorPos(out Point point)
        {
            var success = User32.GetCursorPos(out var cvPoint);
            point = new Point(cvPoint.x, cvPoint.y);
            return success;
        }

        /// <summary>
        ///     移动鼠标到指定位置
        /// </summary>
        /// <param name="point">指定的点击坐标</param>
        /// <param name="isAbsolute">使用绝对坐标时为 True，使用相对坐标时为 False。</param>
        public static void Move(Point point, bool isAbsolute = true)
        {
            //先使用 mouse_event 触发鼠标事件
            const User32.mouse_eventFlags moveFlags = User32.mouse_eventFlags.MOUSEEVENTF_MOVE;
            var flags = isAbsolute ? User32.mouse_eventFlags.MOUSEEVENTF_ABSOLUTE | moveFlags : moveFlags;
            User32.mouse_event(flags, point.X, point.Y, 0, IntPtr.Zero);

            Thread.Sleep(MouseDelay);

            //使用 SetCursorPos() 设置鼠标坐标
            if (isAbsolute)
            {
                User32.SetCursorPos(point.X, point.Y);
            }
            else
            {
                GetCursorPos(out var currentPoint);
                User32.SetCursorPos(currentPoint.X + point.X, currentPoint.Y + point.Y);
            }
        }

        #endregion
    }
}
