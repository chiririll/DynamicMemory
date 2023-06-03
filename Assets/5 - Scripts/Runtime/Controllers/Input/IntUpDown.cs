namespace DynamicMem
{
    public class IntUpDown : NumericUpDown<int>
    {
        protected override int Increment(int value, int step) => value + step;
        protected override int Decrement(int value, int step) => value - step;
        protected override bool TryParse(string text, out int value) => int.TryParse(text, out value);
    }
}
