using System;

namespace GOH.Sequence
{
    // VariableSequence holds a group of objects that can be iterated through, but the OrderType can be set, whether Regular or Random.
    public class VariableSequence<T> : ISequence<T>
    {
        [Serializable]
        public enum OrderType { Regular, Random }

        private OrderType _order = OrderType.Regular;
        public OrderType Order
        {
            get { return _order; }
            set {
                this._order = value;
                Sequence<T> newSequence = NewSequence(value);
                if (this.sequence != null)
                {
                    newSequence.Contents = this.sequence.Contents;
                    if (this.sequence.CurrentIndex >= 0) newSequence.CurrentIndex = this.sequence.CurrentIndex;
                }
                if (newSequence.hasObjects()) newSequence.initialise();
                this.sequence = newSequence;
                //this.random = value == OrderType.Random;
            }
        }

        protected Sequence<T> sequence;

        // Create Sequence without any contained objects
        public VariableSequence() { this.sequence = NewSequence(this.Order); }

        // Create Sequence with objects
        public VariableSequence(T[] objects) {
            this.sequence = NewSequence(this.Order, objects);
        }

        // Returns a new sequence with the oreder type orderType and contents contents.
        private Sequence<T> NewSequence(OrderType orderType, T[] contents)
        {
            Sequence<T> sequence = NewSequence(orderType);
            sequence.Contents = contents;
            return sequence;
        }

        // Generates new empty sequence with OrderType orderType
        private static Sequence<T> NewSequence(OrderType orderType)
        {
            Sequence<T> sequence;
            switch (orderType)
            {
                case OrderType.Regular:
                    sequence = new RegularSequence<T>();
                    break;
                case OrderType.Random:
                    sequence = new RandomSequence<T>();
                    break;
                default:
                    string message = string.Format("Unsupported OrderType: {0}", orderType);
                    throw new ArgumentException(message);
            }
            return sequence;
        }

        public T Next
        {
            get
            {
                return ((ISequence<T>)sequence).Next;
            }
        }

        public T Current
        {
            get
            {
                return ((ISequence<T>)sequence).Current;
            }
        }

        public int CurrentIndex
        {
            get
            {
                return ((ISequence<T>)sequence).CurrentIndex;
            }

            set
            {
                ((ISequence<T>)sequence).CurrentIndex = value;
            }
        }

        public T[] Contents
        {
            get
            {
                return ((ISequence<T>)sequence).Contents;
            }
            set
            {
                ((ISequence<T>)sequence).Contents = value;
            }
        }

        public int Length
        {
            get
            {
                return ((ISequence<T>)sequence).Length;
            }
        }

        public int NextIndex
        {
            get
            {
                return ((ISequence<T>)sequence).NextIndex;
            }
        }

        public void initialise()
        {
            ((ISequence<T>)sequence).initialise();
        }

        public bool hasObjects()
        {
            return ((ISequence<T>)sequence).hasObjects();
        }

        public void advance()
        {
            ((ISequence<T>)sequence).advance();
        }

        // Generates an OrderType from a bool that flags for a random order or not.
        // This method will become depracted when more OrderTypes than Random and Regular are implemented.
        static internal OrderType orderFromBool(bool random)
        {
            return random ? OrderType.Random : OrderType.Regular;
        }
    }
}
