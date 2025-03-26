using System.ComponentModel;
using System.Drawing;
using Wisej.Web;

namespace Captain.Common.Views.Controls.Compatibility
{
    public class TextBoxWithValidation : Wisej.Web.TextBox
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBoxValidation Validator { get; set; }

        protected override void OnValidating(CancelEventArgs e)
        {
            e.Cancel = !ProcessValidator();

            base.OnValidating(e);
        }

        private bool ProcessValidator()
        {
            var valid = true;
            if (this.Text != "")
            {
                if (this.Validator?.RegularExpression == null)
                    return true;


                valid = this.Validator.RegularExpression.Match(this.Text).Success;
                if (!valid)
                {
                    this.SelectOnEnter = true;
                    if (this.Validator.ErrorMessage != "")
                        AlertBox.Show(this.Validator.ErrorMessage, MessageBoxIcon.Error, alignment: ContentAlignment.BottomRight, showCloseButton: true);
                    else
                        AlertBox.Show("Invalid Character", MessageBoxIcon.Error, alignment: ContentAlignment.BottomRight, showCloseButton: true);

                }
            }
            return valid;
        }

        #region Wisej Implementation       

        protected override void OnWebRender(dynamic config)
        {
            base.OnWebRender((object)config);

            if (this.Validator?.ValidationMask != null)
            {
                config.filter = "[" + this.Validator.ValidationMask + "]";
            }
        }

        #endregion
    }
}
