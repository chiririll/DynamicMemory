namespace DynamicMem
{
    public class FloatUpDown : NumericUpDown<float>
    {
        protected override float Decrement(float value, float step) => value - step;
        protected override float Increment(float value, float step) => value + step;
        protected override bool TryParse(string text, out float value) => float.TryParse(text, out value);
    }
}
