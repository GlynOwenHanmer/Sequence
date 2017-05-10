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
    }
}