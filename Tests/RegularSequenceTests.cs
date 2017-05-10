using System;
using NUnit.Framework;

namespace GOH.Sequence.Tests
{
    [TestFixture]
    public class RegularSequenceTests : SequenceTests<RegularSequence<object>>
    {
        const int num_of_objects = 5;

        [Test]
        public void Test_Sequence_RetrievesObjectsInOrder()
        {
            this.sequence.initialise();
            for (int i = 0; i < num_of_objects; i++)
            {
                object expected = this.objects[i];
                object actual = this.sequence.Current;
                if (actual != expected)
                {
                    Assert.Fail("Non random sequence did not return first object after initialisation.");
                }
                this.sequence.advance();
            }
        }

        [Test]
        public void Test_RegularSequence_LoopsAsExpected()
        {
            this.sequence.initialise();
            for (int i = 0; i < num_of_objects * 40; i++)
            {
                object expected = this.objects[i % num_of_objects];
                object actual = this.sequence.Current;
                if (actual != expected)
                {
                    Assert.Fail("Object returned not as expected.");
                }
                this.sequence.advance();
            }
        }

        [Test]
        public void Test_RegularSequence_ShouldNotAdvance_WhenInitialisedMultipleTimes()
        {
            for (int i = 0; i < num_of_objects * 13; i++)
            {
                this.sequence.initialise();
            }
            object expected = this.objects[0];
            object actual = this.sequence.Current;
            if (actual != expected)
            {
                Assert.Fail("Non random sequence did not return first object after multiple initialisation.");
            }
        }

        [Test]
        public void Test_RegularSequence_SetNext()
        {
            this.sequence.initialise();
            Assert.AreEqual(this.objects[0], this.sequence.Current);
            Assert.AreEqual(this.objects[1], this.sequence.Next);

            int[] invalidIndices = {num_of_objects + 200, -65};
            foreach (var invalidIndex in invalidIndices)
            {
                try
                {
                    this.sequence.setNextIndex(invalidIndex);
                }
                catch (IndexOutOfRangeException e)
                {
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            // Set to last index
            this.sequence.setNextIndex(num_of_objects - 1);
            Assert.AreEqual(num_of_objects - 2, this.sequence.CurrentIndex);
            Assert.AreEqual(this.objects[num_of_objects - 2], this.sequence.Current);
            Assert.AreEqual(num_of_objects - 1, this.sequence.NextIndex);
            Assert.AreEqual(this.objects[num_of_objects - 1], this.sequence.Next);

            // Set to first index
            this.sequence.setNextIndex(0);
            //Current should be set to last index
            int expectedCurrentIndex = num_of_objects - 1;
            Assert.AreEqual(expectedCurrentIndex, this.sequence.CurrentIndex);
            Assert.AreEqual(this.objects[expectedCurrentIndex], this.sequence.Current);
            int expectedNextIndex = 0;
            Assert.AreEqual(expectedNextIndex, this.sequence.NextIndex);
            Assert.AreEqual(this.objects[expectedNextIndex], this.sequence.Next);
        }
    }
}