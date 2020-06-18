using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSTest.Extensions.Contracts;

namespace Dreamland.Core.Test.Attributes
{
    [TestClass]
    public class UnitTest
    {
        [ContractTestCase]
        public void EnumTagAttributeTest()
        {
            "测试 GetCustomAttribute 正常使用，能否获取到枚举值".Test(() =>
            {
                // Arrange
                var model = EnumTagTestModel.One;

                // Action
                var result = model.GetCustomAttribute<EnumTagAttribute>();

                // Assert
                Assert.AreEqual("one", result.Name);
                Assert.AreEqual("numb = 1", result.Description);
            });
        }
    }
}