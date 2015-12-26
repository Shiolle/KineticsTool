namespace Kinetics.Core.Data
{
    internal class RefTableResult<T>
    {
        public int Column { get; set; }
        public int Row { get; set; }

        public T Value { get; set; }
    }
}
