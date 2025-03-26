using System.ComponentModel;

namespace Captain.Common.Views.Controls.Compatibility
{
	internal class MaskProvider
	{
		MaskedTextProvider maskedTextProvider;

		public string Mask
		{
			get { return this.maskedTextProvider?.Mask ?? ""; }
			set { this.maskedTextProvider = new MaskedTextProvider(value); }
		}

		public string Text
		{
			get { return this.maskedTextProvider?.ToString() ?? ""; }
			set { this.maskedTextProvider?.Set(value); }
		}
	}
}
