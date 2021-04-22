namespace ff.WpfTools
{
    public sealed class BoolToInverseConverter : BoolToObjectConverter
    {
        public BoolToInverseConverter()
        {
            this.TrueValue = false;
            this.FalseValue = true;
        }
    }
}
