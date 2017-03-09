using System;
using System.Text;

namespace GOH.Sequence
{
    // Sequence holds a group of objects and which can be advanced through in ways implemented by concrete classes of Sequence
    public abstract class Sequence<T> : ISequence<T>
    {
        private bool initialised = false;
        private T[] objects = new T[0];
        public T[] Contents {
            get { return objects; }
            set { this.objects = value; }
        }

        protected int currentIndex = -1; //start with -1 to have no conflicts with index 0 when it becomes available

        // Set currentIndex if value is in range
        public int CurrentIndex {
            get { return this.currentIndex; }
            set {
                if (value >= this.Contents.Length)
                {
                    string message = string.Format("Index out of range.\nIndex : {0}\tLength: {1}", value, this.Length);
                    throw new IndexOutOfRangeException(message);
                }
                else if (value < 0)
                {
                    throw new IndexOutOfRangeException("Negative index given.");
                }
                this.currentIndex = value;
                this.initialised = true;
            }
        }

        public T Current { get { return this.getObjectAtIndex(this.CurrentIndex); } }

        // Returns index of next transform
        public abstract int NextIndex { get; }

        // returns the next object in the sequence without advancing
        public T Next { get { return this.getObjectAtIndex(this.NextIndex); } }

        // use instead of getting all contents and attempting to use array directly.
        // provides more useful error handling
        private T getObjectAtIndex(int index)
        {
            if (index >= this.Contents.Length)
            {
                string message = string.Format("Index out of range.\nIndex : {0}\tLength: {1}", index, this.Length);
                throw new IndexOutOfRangeException(message);
            } else if (index < 0)
            {
                throw new IndexOutOfRangeException("Negative index given. Does the sequence need to be initialised?");
            }
            return this.Contents[index];
        }

        // Create Sequence without any contined objects
        public Sequence() { }

        // Create Sequence with objects
        public Sequence(T[] objects) { this.Contents = objects; }
        
        // Returns the number of transforms in the sequence
        public int Length { get { return this.Contents.Length; } }

        // Initialises sequence and sets current to first object.
        public void initialise()
        {
            if (initialised) return;
            if (!this.hasObjects())
            {
                throw new InvalidOperationException("Sequence initialised with no objects present.");
            }
            this.advance();
            initialised = true;
        }

        // internally sets state of Sequence to next state in sequence
        abstract public void advance();

        // Returns true if any transforms are present.
        public bool hasObjects()
        {
            if (this.Contents.Length > 0) return true;
               return false;
        }

        public override string ToString() { return ToString(", "); }

        public string ToString(string delimiter) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("Current: {0}{1}", currentIndex, delimiter);
            sb.AppendFormat("Next: {0}{1}", NextIndex, delimiter);
            sb.AppendFormat("Length: {0}{1}", Length, delimiter);
            return sb.ToString();
        }
    }
}
