using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Wisej.Web;

namespace Captain.Common.Utilities.MixedDGVUtilities
{
	/// <summary>
	/// Represents a custom DataGridView cell that allows multiple selections from a list of items.
	/// </summary>
	/// <remarks>
	/// This class extends the DataGridViewComboBoxCell class to provide multi-select functionality.
	/// It uses a custom editing control, DataGridViewMultiComboBoxEditingControl, to display the list of items and allow multiple selections.
	/// The selected items are stored as a comma-separated string in the cell's Value property.
	/// </remarks>
	public class DataGridViewMultiComboBoxCell : DataGridViewCell
	{
		private const string SEPARATOR = ",";

		#region Properties

		/// <summary>
		/// Gets or sets the data source for the cell.
		/// </summary>
		public object DataSource
		{
			get => _dataSource;
			set => _dataSource = value;
		}
		private object _dataSource;

		/// <summary>
		/// Gets or sets the display member for the cell.
		/// </summary>
		public string DisplayMember
		{
			get => _displayMember;
			set => _displayMember = value;
		}
		private string _displayMember;

		/// <summary>
		/// Gets or sets the value member for the cell.
		/// </summary>
		public string ValueMember
		{
			get => _valueMember;
			set => _valueMember = value;
		}
		private string _valueMember;

		/// <summary>
		/// Gets the type of the editing control for the cell.
		/// </summary>
		public override Type EditType => typeof(DataGridViewMultiComboBoxEditingControl);

		#endregion

		#region Methods

		/// <summary>
		/// Initializes the editing control for the cell.
		/// </summary>
		/// <param name="editor">The editing control.</param>
		/// <param name="style">The cell style.</param>
		public override void InitializeEditingControl(Control editor, DataGridViewCellStyle style)
		{
			var multiComboBox = editor as DataGridViewMultiComboBoxEditingControl;
			if (multiComboBox != null)
			{
				multiComboBox.FormattingEnabled = true;
				multiComboBox.AllowHtml = this.OwningColumn.AllowHtml;

				if (multiComboBox.DataSource != this.DataSource)
				{
					multiComboBox.DataSource = null;
					multiComboBox.IconMember = null;
					multiComboBox.ValueMember = null;
					multiComboBox.DisplayMember = null;

					multiComboBox.Items.Clear();
					multiComboBox.Separator = SEPARATOR;
					multiComboBox.DisplayMember = this.DisplayMember;
					multiComboBox.ValueMember = this.ValueMember;
					multiComboBox.DataSource = this.DataSource;
				}

				// update the value in the combobox.
				multiComboBox.Text = GetFormattedValue(GetValue(), ref style, null, null) as string ?? string.Empty;
			}
		}

		/// <summary>
		/// Returns the value of the cell formatted to be displayed on the client.
		/// </summary>
		/// <returns>The value of the cell or null if the cell does not belong to a <see cref="T:Wisej.Web.DataGridView" /> control.</returns>
		/// <param name="value">The value to be formatted. </param>
		/// <param name="cellStyle">The <see cref="Wisej.Web.DataGridViewCellStyle" /> in for the cell.</param>
		/// <param name="valueTypeConverter">A <see cref="System.ComponentModel.TypeConverter" /> providing custom conversion from the formatted type to the value type, or null if no custom conversion is required.</param>
		/// <param name="formattedTypeConverter">A <see cref="System.ComponentModel.TypeConverter" /> providing custom conversion from the value type to the formatted type, or null if no custom conversion is required.</param>
		/// <exception cref="System.Exception">
		/// Formatting failed and there is no handler for the <see cref="Wisej.Web.DataGridView.DataError" /> event of the <see cref="Wisej.Web.DataGridView" />
		/// or the handler set the <see cref="Wisej.Web.DataGridViewDataErrorEventArgs.ThrowException" /> property to true.
		/// </exception>
		protected override object GetFormattedValue(object value, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedTypeConverter)
		{
			var text = value as string;
			if (String.IsNullOrEmpty(text))
				return text;

			var items = text.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

			var values = new List<string>();
			foreach (var item in items)
			{
				object displayValue = "";
				if (TryLookupDisplayValue(item, ref displayValue))
					values.Add(displayValue?.ToString());
			}

			return String.Join(SEPARATOR, values.ToArray());
		}

		/// <summary>
		/// Converts a value to the actual cell value.
		/// </summary>
		/// <returns>The cell value.</returns>
		/// <param name="value">The display value of the cell.</param>
		/// <param name="cellStyle">The <see cref="Wisej.Web.DataGridViewCellStyle" /> in effect for the cell.</param>
		/// <param name="formattedTypeConverter">A <see cref="System.ComponentModel.TypeConverter" /> for the display value type, or null to use the default converter.</param>
		/// <param name="valueTypeConverter">A <see cref="System.ComponentModel.TypeConverter" /> for the cell value type, or null to use the default converter.</param>
		/// <exception cref="System.ArgumentNullException">
		///   <paramref name="cellStyle" /> is null.</exception>
		/// <exception cref="System.FormatException">
		/// The <see cref="Wisej.Web.DataGridViewCell.FormattedValueType" /> property value is null 
		/// or the <see cref="Wisej.Web.DataGridViewCell.ValueType" /> property value is null
		/// or <paramref name="value" /> cannot be converted.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is null or the type of <paramref name="value" /> does not match the type indicated by the <see cref="Wisej.Web.DataGridViewCell.FormattedValueType" /> property.
		/// </exception>
		public override object ParseFormattedValue(object value, DataGridViewCellStyle cellStyle, TypeConverter formattedTypeConverter, TypeConverter valueTypeConverter)
		{
			var text = value as string;
			if (String.IsNullOrEmpty(text))
				return text;

			var items = text.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);

			var values = new List<string>();
			foreach (var item in items)
			{
				object itemValue = "";
				if (TryLookupValue(item, ref itemValue))
					values.Add(itemValue?.ToString());
			}

			return String.Join(SEPARATOR, values.ToArray());
		}
		#endregion

		#region Data Binding


		// Currency manager for this cell.
		internal CurrencyManager DataManager
		{
			get
			{
				if (_dataManager == null)
				{
					if (this.DataSource != null)
					{
						return CreateDataManager();
					}
				}
				return _dataManager;
			}
			set
			{
				_dataManager = value;
			}
		}
		private CurrencyManager _dataManager;

		// Creates the currency manager for the data source related to this cell.
		private CurrencyManager CreateDataManager()
		{
			var dataSource = this.DataSource;
			var dataGrid = this.DataGridView;

			// initialize the currency manager.
			if (dataSource != null && dataGrid != null && dataGrid.BindingContext != null && dataSource != DBNull.Value)
			{
				var dataSourceNotification = dataSource as ISupportInitializeNotification;
				if (dataSourceNotification == null || dataSourceNotification.IsInitialized)
				{
					// if the data source doesn't support the ISupportInitializeNotification interface or if
					// it's already initialized, save the data manager instance for this cell.
					var dataManager = (CurrencyManager)dataGrid.BindingContext[dataSource];
					this.DataManager = dataManager;

					return dataManager;
				}
			}
			return null;
		}

		// Property  binding for the DisplayMember.
		internal PropertyDescriptor DisplayMemberProperty
		{
			get
			{
				if (_displayMemberProperty == null)
				{
					InitializeDisplayMember();
				}
				return _displayMemberProperty;
			}
			set
			{
				_displayMemberProperty = value;
			}
		}
		private PropertyDescriptor _displayMemberProperty;

		private PropertyDescriptor InitializeDisplayMember()
		{
			PropertyDescriptor propertyDescriptor = null;
			if (this.DataManager != null)
			{
				var name = this.DisplayMember;
				if (!String.IsNullOrEmpty(name))
				{
					BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(name);

					propertyDescriptor = this.DataManager.GetItemProperties().Find(bindingMemberInfo.BindingField, true);
					if (propertyDescriptor == null)
						throw new ArgumentException($"Field not found: {name}");
				}
				this.DisplayMemberProperty = propertyDescriptor;
			}
			return propertyDescriptor;
		}

		// Property  binding for the ValueMember.
		internal PropertyDescriptor ValueMemberProperty
		{
			get
			{
				if (_valueMemberProperty == null)
				{
					InitializeValueMember();
				}
				return _valueMemberProperty;
			}
			set
			{
				_valueMemberProperty = value;
			}
		}
		private PropertyDescriptor _valueMemberProperty;

		private PropertyDescriptor InitializeValueMember()
		{
			PropertyDescriptor propertyDescriptor = null;
			if (this.DataManager != null)
			{
				var name = this.ValueMember;
				if (!String.IsNullOrEmpty(name))
				{
					BindingMemberInfo bindingMemberInfo = new BindingMemberInfo(name);
					propertyDescriptor = this.DataManager.GetItemProperties().Find(bindingMemberInfo.BindingField, true);
					if (propertyDescriptor == null)
						throw new ArgumentException($"Field not found: {name}");
				}
				this.ValueMemberProperty = propertyDescriptor;
			}
			return propertyDescriptor;
		}

		/// <summary>
		/// Returns the text representation of the specified item.
		/// </summary>
		/// <param name="item">The object from which to get the text to display. </param>
		internal string GetItemDisplayText(object item)
		{
			var value = GetItemDisplayValue(item);
			return value == null
				? string.Empty
				: Convert.ToString(value);
		}

		/// <summary>
		/// Returns the value of the specified item.
		/// </summary>
		/// <param name="item">The object from which to get the value to display. </param>
		/// <returns></returns>
		internal object GetItemDisplayValue(object item)
		{
			if (item == null)
				return null;

			object value = item;

			// use the display or value property if they have been resolved.
			var property = this.DisplayMemberProperty ?? this.ValueMemberProperty;
			if (property != null)
			{
				value = property.GetValue(item);
			}

			// otherwise use the name of the display or value fields.
			var name = String.IsNullOrEmpty(this.DisplayMember) ? this.ValueMember : this.DisplayMember;
			if (!String.IsNullOrEmpty(name))
			{
				property = TypeDescriptor.GetProperties(item).Find(name, true);
				if (property != null)
					value = property.GetValue(item);
			}

			return value;
		}

		/// <summary>
		/// Returns the value field for the specified item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		internal object GetItemValue(object item)
		{
			if (item == null)
				return null;

			object value = item;

			// use the value or display property if they have been resolved.
			var property = this.ValueMemberProperty ?? this.DisplayMemberProperty;
			if (property != null)
			{
				value = property.GetValue(item);
			}

			// otherwise use the name of the value or display fields.
			var name = String.IsNullOrEmpty(this.ValueMember) ? this.DisplayMember : this.ValueMember;
			if (!String.IsNullOrEmpty(name))
			{
				property = TypeDescriptor.GetProperties(item).Find(name, true);
				if (property != null)
					value = property.GetValue(item);
			}

			return value;
		}

		/// <summary>
		/// Finds the item that matches the specified value (key). This method searches
		/// the bound data items or the item collection for unbounded combobox cells.
		/// </summary>
		/// <param name="key">The key to search, it's either the value or the display value of the item.</param>
		/// <param name="property">The property descriptor of the property to compare to find the item.</param>
		/// <param name="fieldName">The name of the field to read and compare to find the item.</param>
		/// <returns></returns>
		private object FindItem(object key, PropertyDescriptor property, string fieldName)
		{
			// find the item in the data source bound to the combobox cell.
			if (this.DataManager != null && property != null)
			{
				object item = null;
				IBindingList bindingList = this.DataManager as IBindingList;
				if (bindingList != null && bindingList.SupportsSearching)
				{
					int index = bindingList.Find(property, key);
					if (index > -1)
						return this.DataManager.List[index];
				}
				else
				{
					for (int i = 0, count = this.DataManager.Count; i < count; i++)
					{
						item = this.DataManager.List[i];

						if (key.Equals(property.GetValue(item)))
							return item;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Searches the item using the formatted value and returns the value from the bound property or field name.
		/// </summary>
		/// <param name="formattedValue"></param>
		/// <param name="value"></param>
		/// <returns>True if the item was found, otherwise false.</returns>
		private bool TryLookupValue(object formattedValue, ref object value)
		{
			if (formattedValue == null)
				return true;

			var item = FindItem(
				formattedValue,
				this.DisplayMemberProperty ?? this.ValueMemberProperty,
				String.IsNullOrEmpty(this.DisplayMember) ? this.ValueMember : this.DisplayMember);

			if (item == null)
				return false;

			value = GetItemValue(item);
			return true;
		}

		/// <summary>
		/// Searches the item using the value and returns the display value from the bound property or field name.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="displayValue"></param>
		/// <returns>True if the item was found, otherwise false.</returns>
		private bool TryLookupDisplayValue(object value, ref object displayValue)
		{
			var item = FindItem(
				value,
				this.ValueMemberProperty ?? this.DisplayMemberProperty,
				String.IsNullOrEmpty(this.ValueMember) ? this.DisplayMember : this.ValueMember);

			if (item == null)
				return false;

			displayValue = GetItemDisplayValue(item);
			return true;
		}
		#endregion
	}
}