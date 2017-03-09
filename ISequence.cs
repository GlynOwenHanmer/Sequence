namespace GOH.Sequence
{
    // Classes that contain conntents that can be iterated through
    public interface ISequence<T>
    {
        // Get current T in sequence
        T Current { get; }

        // Get next T in sequence
        T Next { get; }

        // Get index of current T in sequence
        int CurrentIndex { get; set; }
        
        // Get index of next T in sequence
        int NextIndex { get; }

        // Get or set Contents within sequence
        T[] Contents { get; set; }

        // Get number of Ts in sequence
        int Length { get; }

        // Calculate current and next indexes if not yet calculated.
        void initialise();

        // Advance sequence into next state
        void advance();

        // Returns true if the sequence has objects
        bool hasObjects();
    }
}