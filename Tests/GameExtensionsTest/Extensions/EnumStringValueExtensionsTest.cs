using GameExtensions.Extensions;
using NUnit.Framework;

namespace GameExtensionsTest.Extensions
{
    [TestFixture]
    public class EnumStringValueExtensionsTest
    {
        private enum Season
        {
            [StringAttr("spring")] Spring,

            [StringAttr("summer")] Summer,

            [StringAttr("autumn")] Autumn,

            [StringAttr("winter")] Winter
        }

        [Test]
        public void GetStringAttr()
        {
            Assert.AreEqual("spring", Season.Spring.GetStringAttr());
            Assert.AreEqual("summer", Season.Summer.GetStringAttr());
            Assert.AreEqual("autumn", Season.Autumn.GetStringAttr());
            Assert.AreEqual("winter", Season.Winter.GetStringAttr());
        }

        [Test]
        public void EnumToString()
        {
            Assert.AreEqual("Spring", Season.Spring.ToString());
            Assert.AreEqual("Summer", Season.Summer.ToString());
            Assert.AreEqual("Autumn", Season.Autumn.ToString());
            Assert.AreEqual("Winter", Season.Winter.ToString());
        }
    }
}