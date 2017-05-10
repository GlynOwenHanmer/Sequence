using NUnit.Framework;

namespace GOH.Sequence.Tests
{
    [TestFixture]
    public abstract class SequenceTests<T> where T : Sequence<object>, new()
    {
        const int num_of_objects = 5;
        const int num_of_cycles = 1000;

        public object[] objects;
        public T sequence;

        [SetUp]
        public void SetUp()
        {
            this.objects = new object[num_of_objects];
            for (int i = 0; i < num_of_objects; i++)
            {
                this.objects[i] = new object();
            }
            this.sequence = new T();
            this.sequence.Contents = this.objects;
        }

        [Test]
        public void Test_ThrowsException_WhenInitialisingWithNoObjects()
        {
            T sequence = new T();
            try
            {
                sequence.initialise();
            }
            catch (System.InvalidOperationException) { return; }
            Assert.Fail("InvalidOperationException expected but not thrown.");
        }

        [Test]
        public void Test_Current_ShouldNever_CauseCurrentToChange()
        {
            this.sequence.initialise();
            for (int i = 0; i < num_of_cycles; i++)
            {
                object current = this.sequence.Current;
                for (int j = 0; j < num_of_cycles; j++)
                {
                    object actualCurrent = this.sequence.Current;
                    if (actualCurrent != current)
                    {
                        Assert.Fail("Current changed when called repeatedly.");
                    }
                }
                this.sequence.advance();
            }
        }

        [Test]
        public void Test_Current_ShouldNotBeAbleToSet_NegativeCurrentIndex()
        {
            this.sequence.initialise();
            try
            {
                this.sequence.CurrentIndex = -1;
            }
            catch (System.IndexOutOfRangeException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail("IndexOutOfRangeException expected but not thrown.");
        }

        [Test]
        public void Test_Current_ShouldNotBeAbleToSet_OverlyLargeCurrentIndex()
        {
            this.sequence.initialise();
            try
            {
                this.sequence.CurrentIndex = 9999;
            }
            catch (System.IndexOutOfRangeException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail("IndexOutOfRangeException expected but not thrown.");
        }
    }
}