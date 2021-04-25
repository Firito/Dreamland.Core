using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Dreamland.Core.Simulate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dreamland.Core.Test
{
    [TestClass]
    public class MouseTests
    {
        #region 点击测试

        [TestMethod(displayName: "鼠标单击测试")]
        public void ClickTest()
        {
            Mouse.Click();
        }

        [TestMethod(displayName: "鼠标在指定位置单击测试")]
        [DataTestMethod]
        [DataRow(0, 0, true)]
        public void ClickTest2(int x, int y, bool isAbsolute = true)
        {
            Mouse.Click(new Point(x, y), isAbsolute);
        }

        #endregion

        #region 移动测试

        [TestMethod(displayName: "鼠标移动测试")]
        [DataTestMethod]
        [DataRow(100, 100, true)]
        [DataRow(200, 200, false)]
        [DataRow(300, 300, true)]
        [DataRow(200, 200, false)]
        public void MouseMoveTest(int x, int y, bool isAbsolute = true)
        {
            Mouse.Move(new Point(x, y), isAbsolute);
            Thread.Sleep(1000);
            Assert.IsTrue(Mouse.GetCursorPos(out var point));
            if (isAbsolute)
            {
                Assert.AreEqual(x, point.X);
                Assert.AreEqual(y, point.Y);
            }
        }

        #endregion

        #region 鼠标拖拽测试
        
        [TestMethod(displayName: "鼠标拖拽测试")]
        public void DragTest()
        {
            Mouse.Move(new Point());
            Assert.IsTrue(Mouse.Drag(100, 100));
            Assert.IsTrue(Mouse.Drag(400, 200));
            Assert.IsTrue(Mouse.Drag(new Point(400, 600), new Point(600, 800)));
            Assert.IsTrue(Mouse.Drag(new Point(1000, 500), new List<Point>()
            {
                new(1000, 600),
                new(1200, 800)
            }));
        }

        #endregion
    }
}