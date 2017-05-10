using NUnit.Framework;
using System;

namespace GOH.Sequence.Tests
{
    
    // TODO write tests for cases such as only one object.
    // Any more edge cases?

    [TestFixture]
    public class RandomSequenceTests : SequenceTests<RandomSequence<object>>
    {
        const int num_of_objects = 5;
        const int num_of_cycles = 1000;

        [Test]
        public void Test_RandomSequence_NeverRetrievesSameObject()
        {
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
            RandomSequence<object> sequence = new RandomSequence<object>(objects);
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
                } else
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
            this.sequence.initialise();
            for (int i = 0; i < 150 ; i++)
            {
                object nextBeforeAdvance = this.sequence.Next;
                this.sequence.advance();
                object nextAfterAdvance = this.sequence.Current;
                if(nextAfterAdvance != nextBeforeAdvance)
                {
                    Assert.Fail("Next does not become current after advance() method.");
                }
            }
        }
    }
}