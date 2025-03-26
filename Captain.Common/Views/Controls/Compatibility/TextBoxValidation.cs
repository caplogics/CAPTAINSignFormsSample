using System.Text.RegularExpressions;
using Wisej.Web;

namespace Captain.Common.Views.Controls.Compatibility
{
    public class TextBoxValidation
    {
        static TextBoxValidation()
        {
            IntegerValidator = new TextBoxValidation("^-{0,1}[0-9]*$", "", "0-9\\-");
            //PIntegerValidator = new TextBoxValidation("^[^0-9]", "", "0-9\\");
            // FloatValidator = new TextBoxValidation("/^-{0,1}[0-9]*([" + DecimalSeparator + "][0-9]+){0,1}$/", "", "0-9\\" + DecimalSeparator + "\\|\\x1B");

            CustomDecimalValidation2dot2 = new TextBoxValidation(@"^[0-9]\d{0,2}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 99.99", "0-9\\.");
            CustomDecimalValidation3dot2 = new TextBoxValidation(@"^[0-9]\d{0,2}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 999.99", "0-9\\.");
            CustomDecimalValidation = new TextBoxValidation(@"^[0-9]\d{0,2}(\.\d{1,3})*(,\d+)?$", "Value must be between 0 - 99.999", "0-9\\.");
            CustomDecimalValidation = new TextBoxValidation(@"^[0-9]\d{0,2}(\.\d{1,3})*(,\d+)?$", "Value must be between 0 - 99.999", "0-9\\.");
            CustomDecimalValidation3dot5 = new TextBoxValidation(@"^[0-9]\d{0,3}(\.\d{1,5})*(,\d+)?$", "Value must be between 0 - 999.999999", "0-9\\.");
            CustomDecimalValidation4dot2 = new TextBoxValidation(@"^[0-9]\d{0,3}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 9999.99", "0-9\\.");
            CustomDecimalValidation5dot2 = new TextBoxValidation(@"^[0-9]\d{0,4}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 99999.99", "0-9\\.");
            FloatValidator = new TextBoxValidation(@"^[0-9]\d{0,5}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 999999.99", "0-9\\.");
            CustomDecimalValidation8dot3 = new TextBoxValidation(@"^[0-9]\d{0,5}(\.\d{1,3})*(,\d+)?$", "Value must be between 0 - 99999.999", "0-9\\.");
            CustomDecimalValidation9dot2 = new TextBoxValidation(@"^[0-9]\d{0,5}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 9,999,999.99", "0-9\\.");
            CustomDecimalValidation13dot2 = new TextBoxValidation(@"^[0-9]\d{0,11}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 99999999999.99", "0-9\\.");

            CustomDecimalValidation15dot2 = new TextBoxValidation(@"^[0-9]\d{0,11}(\.\d{1,2})*(,\d+)?$", "Value must be between 0 - 9999999999999.99", "0-9\\.");

        }

        public TextBoxValidation(string regularExpression, string errorMessage, string validationMask)
        {
            this.ErrorMessage = errorMessage;
            this.ValidationMask = validationMask;
            this.RegularExpression = new Regex(regularExpression);
        }

        public static TextBoxValidation IntegerValidator { get; }

        public static TextBoxValidation PIntegerValidator { get; }
        public static TextBoxValidation FloatValidator { get; }
        public static TextBoxValidation IntegerMaskValidator { get; }
        public static TextBoxValidation CustomDecimalValidation { get; }
        public static TextBoxValidation CustomDecimalValidation8dot3 { get; }
        public static TextBoxValidation CustomDecimalValidation3dot2
        {
            get;
        }
        public static TextBoxValidation CustomDecimalValidation5dot2 { get; }
        public static TextBoxValidation CustomDecimalValidation4dot2 { get; }
        public static TextBoxValidation CustomDecimalValidation13dot2 { get; }
        public static TextBoxValidation CustomDecimalValidation15dot2
        {
            get;
        }
        public static TextBoxValidation CustomDecimalValidation2dot2 { get; set; }
        public static TextBoxValidation CustomDecimalValidation3dot5 { get; }

        public static TextBoxValidation CustomDecimalValidation9dot2 { get; }
        public string ErrorMessage { get; set; }

        public string ValidationMask { get; set; }

        public Regex RegularExpression { get; set; }

        private static string DecimalSeparator => Application.CurrentCulture.NumberFormat.NumberDecimalSeparator;
    }
}
