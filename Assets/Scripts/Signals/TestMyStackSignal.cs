namespace Signals
{
    public class TestMyStackSignal
    {
        public bool IsEnabled { get; private set; }

        public TestMyStackSignal(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }
    }
}