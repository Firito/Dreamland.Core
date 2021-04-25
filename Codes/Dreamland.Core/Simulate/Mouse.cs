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
        private const int MouseDelay = 50;

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

        #region Drag

        /// <summary>
        ///     默认拖拽速度
        /// </summary>
        private const int DefaultDragSpeed = 10;

        /// <summary>
        ///     从鼠标当前位置进行拖拽 <paramref name="offsetX"/> 个横向偏移量，<paramref name="offsetY"/> 个纵向偏移量
        /// </summary>
        /// <param name="offsetX">拖拽横向偏移量</param>
        /// <param name="offsetY">拖拽纵向偏移量</param>
        /// <param name="speed">拖拽速度</param>
        public static bool Drag(int offsetX, int offsetY, uint speed = DefaultDragSpeed)
        {
            return GetCursorPos(out var currentPoint) && Drag(currentPoint, offsetX, offsetY, speed);
        }

        /// <summary>
        ///     从鼠标当前位置进行拖拽至指定坐标点<paramref name="point"/>
        /// </summary>
        /// <param name="point">拖拽目标坐标点</param>
        /// <param name="speed">拖拽速度</param>
        public static bool Drag(Point point, uint speed = DefaultDragSpeed)
        {
            return GetCursorPos(out var currentPoint) && Drag(currentPoint, point, speed);
        }

        /// <summary>
        ///     将鼠标从指定坐标点<paramref name="startPoint"/>拖拽至指定坐标点<paramref name="endPoint"/>
        /// </summary>
        /// <param name="startPoint">拖拽起始坐标点</param>
        /// <param name="endPoint">拖拽目标坐标点</param>
        /// <param name="speed">拖拽速度</param>
        public static bool Drag(Point startPoint, Point endPoint, uint speed = DefaultDragSpeed)
        {
            return Drag(startPoint, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, speed);
        }

        /// <summary>
        ///     将鼠标从指定坐标点<paramref name="startPoint"/>拖拽 <paramref name="offsetX"/> 个横向偏移量，<paramref name="offsetY"/> 个纵向偏移量
        /// </summary>
        /// <param name="startPoint">拖拽起始坐标点</param>
        /// <param name="offsetX">拖拽横向偏移量</param>
        /// <param name="offsetY">拖拽纵向偏移量</param>
        /// <param name="speed">拖拽速度</param>
        public static bool Drag(Point startPoint, int offsetX, int offsetY, uint speed = DefaultDragSpeed)
        {
            User32.mouse_event(User32.mouse_eventFlags.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(MouseDelay);

            //执行移动
            ExecuteDrag(startPoint, offsetX, offsetY, speed);

            Thread.Sleep(MouseDelay);
            User32.mouse_event(User32.mouse_eventFlags.MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero);
            return true;
        }

        /// <summary>
        ///     执行一次拖拽<para>将鼠标从指定坐标点<paramref name="startPoint"/>拖拽 <paramref name="offsetX"/> 个横向偏移量，<paramref name="offsetY"/> 个纵向偏移量</para>
        /// </summary>
        /// <param name="startPoint">拖拽起始坐标点</param>
        /// <param name="offsetX">拖拽横向偏移量</param>
        /// <param name="offsetY">拖拽纵向偏移量</param>
        /// <param name="speed">拖拽速度</param>
        private static void ExecuteDrag(Point startPoint, int offsetX, int offsetY, uint speed)
        {
            long moveTimes;
            int thisOffsetX;
            int thisOffsetY;
            var minOffset = (speed == 0 ? 1 : speed) * 10;

            if (offsetX == 0 || offsetY == 0)
            {
                moveTimes = Math.Max(Math.Abs(offsetX) / minOffset, Math.Abs(offsetY) / minOffset);
                thisOffsetX = (int) (offsetX / moveTimes);
                thisOffsetY = (int) (offsetY / moveTimes);
            }
            else
            {
                moveTimes = Math.Min(Math.Abs(offsetX) / minOffset, Math.Abs(offsetY) / minOffset);
                thisOffsetX = (int) (offsetX / moveTimes);
                thisOffsetY = (int) (offsetY / moveTimes);
            }

            for (var i = 1; i <= moveTimes; i++)
            {
                Move(new Point(startPoint.X + thisOffsetX * i, startPoint.Y + thisOffsetY * i));
                Thread.Sleep(MouseDelay);
            }
            
            Move(new Point(startPoint.X + offsetX, startPoint.Y + offsetY));
        }

        #endregion
    }
}
