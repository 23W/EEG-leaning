using EEGCore.Processing.Model;

namespace EEGCoreTests
{
    [TestClass]
    public class WindowTests
    {
        [TestMethod]
        public void AccumulateWindow()
        {
            var data = new[] { 0.0, 1.0, 2.0, 3.0, 4.0 };
            var orderedResult = new[] { 2.0, 3.0, 4.0 };
            var unorderedResult = new[] { 3.0, 4.0, 2.0 };

            var wnd = new WindowFunction() { Width = 3 };
            foreach (var item in data)
            {
                wnd.Add(item);
            }

            Assert.IsTrue(wnd.Count == wnd.Width);
            Assert.IsTrue(wnd.AccumulatedCount == data.Length);
            Assert.IsTrue(wnd.OrderedSamples.SequenceEqual(orderedResult));
            Assert.IsTrue(wnd.Samples.SequenceEqual(unorderedResult));
        }

        [TestMethod]
        public void AccumulateWindowAliquotLength()
        {
            var data = new[] { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0 };
            var orderedResult = new[] { 3.0, 4.0, 5.0 };
            var unorderedResult = new[] { 3.0, 4.0, 5.0 };

            var wnd = new WindowFunction() { Width = 3 };
            foreach (var item in data)
            {
                wnd.Add(item);
            }

            Assert.IsTrue(wnd.Count == wnd.Width);
            Assert.IsTrue(wnd.AccumulatedCount == data.Length);
            Assert.IsTrue(wnd.OrderedSamples.SequenceEqual(orderedResult));
            Assert.IsTrue(wnd.Samples.SequenceEqual(unorderedResult));
        }

        [TestMethod]
        public void AccumulateWindowPart()
        {
            var data = new[] { 0.0, 1.0 };
            var orderedResult = new[] { 0.0, 1.0 };
            var unorderedResult = new[] { 0.0, 1.0, -1.0 };

            var wnd = new WindowFunction() { Width = 3 };
            wnd.Fill(-1);
            foreach (var item in data)
            {
                wnd.Add(item);
            }

            Assert.IsTrue(wnd.Count == data.Length);
            Assert.IsTrue(wnd.AccumulatedCount == data.Length);
            Assert.IsTrue(wnd.OrderedSamples.SequenceEqual(orderedResult));
            Assert.IsTrue(wnd.Samples.SequenceEqual(unorderedResult));
        }
    }
}
