using Java.Lang;
using Java.Text;

namespace Android.Utilities
{
    public class EmptyNumberFormat : NumberFormat
    {
        public override StringBuffer Format(double value, StringBuffer buffer, FieldPosition field)
        {
            return new StringBuffer();
        }

        public override StringBuffer Format(long value, StringBuffer buffer, FieldPosition field)
        {
            return new StringBuffer();
        }

        public override Number Parse(string @string, ParsePosition position)
        {
            return (Number)0;
        }
    }
}