using System.Drawing;
using System.Threading;
using Dreamland.Core.Simulate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dreamland.Core.Test
{
    [TestClass]
    public class MouseTests
    {
        [TestMethod(displayName:"鼠标单击测试")]
        public void ClickTest()
        {
            Mouse.Click();
        }

        [TestMethod(displayName:"鼠标在指定位置单击测试")]
        [DataTestMethod]
        [DataRow(0, 0, true)]
        public void ClickTest2(int x, int y, bool isAbsolute = true)
        {
            Mouse.Click(new Point(x, y), isAbsolute);
        }

        [TestMethod(displayName:"鼠标移动测试")]
        [DataTestMethod]
        [DataRow(100, 100, true)]
        [DataRow(200, 200, false)]
        [DataRow(300, 300, true)]
        [DataRow(200, 200, false)]
        public void MouseMoveTest(int x, int y, bool isAbsolute = true)
        {
            Mouse.Move(new Point(x, y), isAbsolute);
            Assert.IsTrue(Mouse.GetCursorPos(out var point));
            if (isAbsolute)
            {
                Assert.AreEqual(x, point.X);
                Assert.AreEqual(y, point.Y);
            }
            Thread.Sleep(1000);
        }
    }
}