using NUnit.Framework;
using System;

namespace GOH.Sequence.Tests
{

    // TODO write tests for cases such as only one object.
    // Any more edge cases?

    [TestFixture]
    public class VariableSequenceTests
    {
        const int num_of_objects = 5;
        const int num_of_cycles = 1000;


        public object[] objects;
        public VariableSequence<object> sequence;

        [SetUp]
        public void SetUp()
        {
            this.objects = new object[num_of_objects];
            for (int i = 0; i < num_of_objects; i++)
            {
                this.objects[i] = new object();
            }
            this.sequence = new VariableSequence<object>();
            this.sequence.Contents = this.objects;
        }

        [Test]
        public void Test_ThrowsException_WhenInitialisingWithNoObjects()
        {
            VariableSequence<object> sequence = new VariableSequence<object>();
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
                IntegrationTest.Pass();
                return;
            }
            IntegrationTest.Fail("IndexOutOfRangeException expected but not thrown.");
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
                IntegrationTest.Pass();
                return;
            }
            IntegrationTest.Fail("IndexOutOfRangeException expected but not thrown.");
        }

        [Test]
        public void Test_RegularSequence_RetrievesObjectsInOrder()
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
        public void Test_OrderTypeSwitch_ShouldYield_InitialisedSequence_WherePossible()
        {
            VariableSequence<object>.OrderType previousOrder = this.sequence.Order;
            this.sequence.Order = previousOrder == VariableSequence<object>.OrderType.Random ? VariableSequence<object>.OrderType.Regular : VariableSequence<object>.OrderType.Random;
            Assert.AreNotEqual(this.sequence.Order, previousOrder, "VariableSequence order did not switch during test.");
            Assert.GreaterOrEqual(this.sequence.CurrentIndex, 0, "VariableSequence did not yield initialised sequence when OrderType was switched.");
        }

        [Test]
        public void Test_RandomSequence_NeverRetrievesSameObject()
        {
            this.sequence.Order = VariableSequence<object>.OrderType.Random;
            this.sequence.initialise();
            object lastObject = null;
            for (int i = 0; i < num_of_cycles; i++)
            {
                object current = this.sequence.Current;
                if (current == lastObject)
                {
                    Console.WriteLine(i);
                    Console.WriteLine(current.GetHashCode());
                    Console.WriteLine(lastObject.GetHashCode());
                    Assert.Fail("RandomSequence returned same object twice in a row.");
                }
                lastObject = current;
                this.sequence.advance();
            }
        }

        [Test]
        public void Test_RandomSequence_ShouldAlternate_WhenOnlyTwoObjects()
        {
            object[] objects = new object[2] { new object(), new object() };
            VariableSequence<object> sequence = new VariableSequence<object>(objects);
            this.sequence.Order = VariableSequence<object>.OrderType.Random;
            sequence.initialise();
            object firstObject = sequence.Current;
            object secondObject = sequence.Next;
            for (int i = 0; i < num_of_cycles; i++)
            {
                object current = sequence.Current;
                object next = sequence.Next;
                if (i % 2 == 0)
                {
                    if (current != firstObject || next != secondObject)
                    {
                        FailWithShouldAlternateMessage();
                    }
                }
                else
                {
                    if (current == firstObject || next == secondObject)
                    {
                        FailWithShouldAlternateMessage();
                    }
                }
                sequence.advance();
            }
        }

        private void FailWithShouldAlternateMessage()
        {
            Assert.Fail("RandomSequence didn't alternate between two objects in sequence when only two are present.");
        }
        [Test]
        public void Test_RandomSequence_ShouldNotAdvance_WhenInitialisedMultipleTimes()
        {
            this.sequence.Order = VariableSequence<object>.OrderType.Random;
            this.sequence.initialise();
            object startingObject = this.sequence.Current;
            for (int i = 0; i < num_of_objects * 13; i++)
            {
                this.sequence.initialise();
                object currentObject = this.sequence.Current;
                if (currentObject != startingObject)
                {
                    Assert.Fail("RandomSequence changed current object after multiple initialisations.");
                }
            }
        }

        [Test]
        public void Test_RandomSequence_NextValueShouldBecomeCurrentValue_WhenAdvanced()
        {
            this.sequence.Order = VariableSequence<object>.OrderType.Random;
            this.sequence.initialise();
            for (int i = 0; i < 150; i++)
            {
                object nextBeforeAdvance = this.sequence.Next;
                this.sequence.advance();
                object nextAfterAdvance = this.sequence.Current;
                Assert.AreEqual(nextAfterAdvance, nextBeforeAdvance, "Next does not become current after advance() method.");
            }
        }


        [Test]
        public void Test_ContentsPersist_WhenOrderType_IsSwitched()
        {
            object[] previousContents = this.sequence.Contents;
            VariableSequence<object>.OrderType previousOrder = this.sequence.Order;
            this.sequence.Order = previousOrder == VariableSequence<object>.OrderType.Random ? VariableSequence<object>.OrderType.Regular : VariableSequence<object>.OrderType.Random;
            Assert.AreNotEqual(this.sequence.Order, previousOrder, "VariableSequence order did not switch during test.");
            Assert.AreEqual(previousContents, this.sequence.Contents, "VariableSequence contents changed when switching random.");
        }
    }
}