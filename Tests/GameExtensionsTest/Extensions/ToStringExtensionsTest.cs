using GameExtensions.Extensions;
using NUnit.Framework;

namespace GameExtensionsTest.Extensions
{
    [TestFixture]
    public class ToStringExtensionsTest
    {
        private class Person
        {
            public long Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return this.GetPropertiesAsText();
            }
        }

        [Test]
        public void GetPropertiesAsText()
        {
            var lisa = new Person
            {
                Id = 1,
                Name = "Lisa"
            };

            Assert.AreEqual("{Id=1, Name=Lisa}", lisa.ToString());
        }
    }
}