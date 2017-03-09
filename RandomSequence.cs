using GOHAV;
using System;

namespace GOH.Sequence
{
    // RandomSequence holds a group of T and can be iterated through.
    // When advancing to a next state, the next T in the sequence will be generated randomly.
    public class RandomSequence<T> : Sequence<T>
    {
        private int nextIndex = -1;

        // Create Sequence without any contained objects
        public RandomSequence() { }

        // Create Sequence with objects
        public RandomSequence(T[] objects) : base(objects) { }

        // Returns index of next transform
        public override int NextIndex
        {
            get
            {
                // If it has not been set.
                if (this.nextIndex < 0)
                {
                    this.nextIndex = GOHAVUtility.randomNumberNotThis(0, this.Length, this.CurrentIndex); ;
                }
                return this.nextIndex;
            }
        }

        // Set the next index of the sequence.
        public void setNextIndex(int nextIndex)
        {
            if (nextIndex >= this.Contents.Length)
            {
                string message = string.Format("Index out of range.\nIndex : {0}\tLength: {1}", nextIndex, this.Length);
                throw new IndexOutOfRangeException(message);
            }
            else if (nextIndex < 0)
            {
                throw new IndexOutOfRangeException("Negative NextIndex given.");
            }
            this.nextIndex = nextIndex;
        }

        // Advance sequence into next state
        public override void advance()
        {
            this.currentIndex = this.NextIndex;
            this.nextIndex = -1;
        }
    }
}
