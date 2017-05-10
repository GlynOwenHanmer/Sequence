using System;

namespace GOH.Sequence
{
    // RegularSequence holds a group of T and will return them in the same order when iterated through.
    public class RegularSequence<T> : Sequence<T>, ISequence<T>
    {
        // Create Sequence without any contined objects
        public RegularSequence()
        {
        }

        // Create Sequence with objects
        public RegularSequence(T[] objects) : base(objects)
        {
        }

        // Advance sequence into next state.
        public override void advance()
        {
            this.currentIndex = this.NextIndex;
        }

        // Get index of Next T in sequence
        public override int NextIndex
        {
            get { return (this.currentIndex + 1) % this.Length; }
        }

        new public string ToString()
        {
            return string.Format("Order: Regular\t{0}", base.ToString());
        }

        public void setNextIndex(int i)
        {
            if (i >= this.Length || i < 0)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Index {0} out of range for sequence length {1}", i, this.Length));
            }
            int newCurrentIndex = (i - 1) % Length;
            // for a negative value, modulus returns negative remainder, so must add Length of sequence to ensure value is in positive range
            newCurrentIndex = newCurrentIndex < 0 ? newCurrentIndex + Length : newCurrentIndex;
            this.currentIndex = newCurrentIndex;
        }
    }
}