using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReadableCodeSamples
{
    [TestClass]
    public class StringExtensionsRemoveSpaces
    {
        [TestMethod]
        public void NullTest()
        {
            string testNull = null;
            string result = testNull.RemoveSpaces();
        }

        [TestMethod]
        public void RemovesSpaces()
        {

            string result = "Hello, World!".RemoveSpaces();
            Assert.AreEqual("Hello,World!", result);
        }
    }

    public static class StringExtentions
    {
        public static string RemoveSpaces(this string subject)
        {
            if(subject == null) return null;
            return subject.Replace(" ", string.Empty);
        }
    }
}
