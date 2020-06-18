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
            "���� GetCustomAttribute ����ʹ�ã��ܷ��ȡ��ö��ֵ".Test(() =>
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