using System;
using Wisej.Web;

namespace Captain.Common.Utilities.MixedDGVUtilities
{
	/// <summary>
	///	Represents a custom editing control for a DataGridViewComboBoxColumn that allows multiple selections.
	/// </summary>
	public class DataGridViewMultiComboBoxEditingControl : UserComboBox, IDataGridViewEditingControl
	{

		// private field to hold the CheckedListBox control
		private CheckedListBox _checkedListBox = new CheckedListBox();

		/// <summary>
		/// Initializes a new instance of the DataGridViewMultiComboBoxEditingControl class.
		/// </summary>
		public DataGridViewMultiComboBoxEditingControl()
		{
			// set the DropDownControl property of the UserComboBox to the CheckedListBox control
			this.DropDownControl = _checkedListBox;

			// add an event handler for the AfterItemCheck event of the CheckedListBox control
			_checkedListBox.Separator = ",";
			_checkedListBox.AfterItemCheck += _checkedListBox_AfterItemCheck;
		}

		// public properties to expose the DataSource, DisplayMember, and ValueMember properties of the CheckedListBox control
		public new object DataSource
		{
			get { return _checkedListBox.DataSource; }
			set { _checkedListBox.DataSource = value; }
		}

		public new string DisplayMember
		{
			get { return _checkedListBox.DisplayMember; }
			set { _checkedListBox.DisplayMember = value; }
		}

		public new string ValueMember
		{
			get { return _checkedListBox.ValueMember; }
			set { _checkedListBox.ValueMember = value; }
		}

		public string Separator
		{
			get { return _checkedListBox.Separator; }
			set { _checkedListBox.Separator = value; }
		}

		// override the OnTextChanged method to notify the DataGridView that the current cell is dirty
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.DataGridView?.NotifyCurrentCellDirty(true);
		}

		// event handler for the AfterItemCheck event of the CheckedListBox control
		private void _checkedListBox_AfterItemCheck(object sender, ItemCheckEventArgs e)
		{
			this.DataGridView?.NotifyCurrentCellDirty(true);
		}

		#region IDataGridViewEditingControl

		public DataGridView DataGridView
		{
			get; set;
		}

		string IDataGridViewEditingControl.GetEditingControlFormattedValue()
		{
			return _checkedListBox.Text;
		}

		bool IDataGridViewEditingControl.Invalid
		{
			get;
		}

		void IDataGridViewEditingControl.ApplyCellStyleToEditingControl(DataGridViewCellStyle style)
		{
		}

		void IDataGridViewEditingControl.PrepareEditingControlForEdit(bool selectAll)
		{
		}

		#endregion

		#region Implementation 

		/// <summary>
		/// Gets or sets the text of the editing control.
		/// </summary>
		public override string Text
		{
			get => _checkedListBox.Text;
			set => _checkedListBox.Text = value;
		}

		#endregion
	}
}