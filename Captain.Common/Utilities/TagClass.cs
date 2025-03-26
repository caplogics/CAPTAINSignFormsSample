/************************************************************************************
* Class Name    : TagClass
* Author        : Chitti
* Created Date  : 
* Version       : 1.0
* Description   : This class is used to define the TagClass properties
*************************************************************************************/

using System;
using Captain.Common.Controllers;
using System.Drawing;
using System.Collections.Generic;
using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using System.Xml.Serialization;
namespace Captain.Common.Utilities
{
    [Serializable()]
    public class TagClass : ICloneable, IComparable<TagClass>
    {
        #region Variables

        [NonSerialized]
        private CaptainModel _model;
        private TagClass _parentTagClass = null;
        private string _objectID = string.Empty;
        private string _fileName = string.Empty;
        private string _icon = string.Empty;
        #endregion

        #region Constructor

        public TagClass()
        {
            
        }

        #endregion

        #region Properties

        [XmlIgnore]
        public CaptainModel Model
        {
            get
            {
                if (_model == null)
                {
                    _model = new CaptainModel();
                }
                return _model;
            }
            set
            {
                _model = value;
            }
        }

        [XmlIgnore]
        public object NodeObject { get; set; }

        public int BusinessModuleID { get; set; }

        [XmlIgnore]
        public Color MenuColor { get; set; }

        public bool HasNodes { get; set; }

        public string Level { get; set; }
          
        /// <summary>
        /// Used for checkin
        /// </summary>
        public string Description { get; set; }

        [XmlIgnore]
        public string Message { get; set; }

        [XmlIgnore]
        public string ProjectPath { get; set; }

        [XmlIgnore]
        public string QueryID { get; set; }

        [XmlIgnore]
        public string TypeElement { get; set; }

        [XmlIgnore]
        public string Path { get; set; }

        public string NodeID { get; set; }

        public string ParentNodeID { get; set; }

        [XmlIgnore]
        public string OperationalNodeID { get; set; }

        [XmlIgnore]
        public string DocumentType { get; set; }

        [XmlIgnore]
        public string FormID { get; set; }

        [XmlIgnore]
        public string GlobalGuid { get; set; }

        [XmlIgnore]
        public string ObjectID
        {
            get { return _objectID; }
            set
            {
                _objectID = value;

                if (!String.IsNullOrEmpty(_objectID)) { SetState(); }
            }
        }

        [XmlIgnore]
        public string CreatedByUserID { get; set; }

        [XmlIgnore]
        public string CreatedDate { get; set; }

        [XmlIgnore]
        public string DeletionDate { get; set; }

        [XmlIgnore]
        public string ModificationDate { get; set; }

        [XmlIgnore]
        public string ModifiedByUserID { get; set; }

        [XmlIgnore]
        public string CompanyID { get; set; }

        [XmlIgnore]
        public string CompanyName { get; set; }

        [XmlIgnore]
        public string ReferenceNodeID { get; set; }

        public int Multiplier { get; set; }

        [XmlIgnore]
        public string[] Types { get; set; }

        [XmlIgnore]
        public string[] Paths { get; set; }

        [XmlIgnore]
        public int RowNumber { get; set; }

        /// <summary>
        /// Specifies the order number.
        /// </summary>
        [XmlIgnore]
        public string OrderNumber { get; set; }

        /// <summary>
        /// Specifies the level of the node in a tree.
        /// </summary>
        [XmlIgnore]
        public string NodeLevel { get; set; }

        /// <summary>
        /// Specifies the selected view of the node.
        /// </summary>
        [XmlIgnore]
        public string SelectedView { get; set; }

        /// <summary>
        /// Specifies if the object has children.
        /// </summary>
        [XmlIgnore]
        public bool HasChildren { get; set; }

        /// <summary>
        /// Specifies if the object has children.
        /// </summary>
        [XmlIgnore]
        public bool HasRelatedChildren { get; set; }

        /// <summary>
        /// Specifies the name of the object.
        /// </summary>
        [XmlIgnore]
        public string ObjectName { get; set; }

        /// <summary>
        /// Specifies the icon that is to be used in the treeview or the hierarchical grid.
        /// </summary>
        [XmlIgnore]
        public string Icon
        {
            get { return _icon; }
            set 
            {
                _icon = value;
                //Operation = CommonFunctions.GetOperation(this);
            }
        }

        /// <summary>
        /// Specifies the item that was clicked in a menu item.
        /// </summary>
        [XmlIgnore]
        public string ItemClicked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public string VisiblePrivilegeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public string EnabledPrivilegeName { get; set; }

        /// <summary>
        /// Specifies the first page number to display.
        /// </summary>
        [XmlIgnore]
        public int InitialPageNumber { get; set; }

        /// <summary>
        /// Specifies the first Name Destination to display.
        /// </summary>
        [XmlIgnore]
        public string InitialNamedDestination { get; set; }

        /// <summary>
        /// set Undo Enable state.
        /// </summary>
        [XmlIgnore]
        public bool UndoEnabled { get; set; }

        /// <summary>
        /// set Redo Enable state.
        /// </summary>
        [XmlIgnore]
        public bool RedoEnabled { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets all fields with null to empty strings.
        /// </summary>
        public void EnsureNotNull()
        {
            if (ObjectID.Equals(string.Empty)) { _objectID = Consts.Common.NullIntegerValue; }
            if (GlobalGuid == null) { GlobalGuid = string.Empty; }            
            if (ProjectPath == null) { ProjectPath = string.Empty; }
            if (NodeID == null) { NodeID = string.Empty; }
            if (ParentNodeID == null) { ParentNodeID = string.Empty; }
            if (FormID == null) { FormID = string.Empty; }
            if (Icon == null || Icon.Equals(string.Empty)) { Icon = Consts.Icons16x16.Document; }
            if (ItemClicked == null) { ItemClicked = string.Empty; }
        }   

        public override string ToString()
        {
            return string.Concat(Consts.PropertyNames.ItemClicked, Consts.Common.Colon, Consts.Common.Space, ItemClicked, Consts.Common.Comma, Consts.PropertyNames.ObjectName, Consts.Common.Colon, Consts.Common.Space, ObjectName);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This sets the state flags in the TagClass object
        /// It is used to determine if the object is a Submission, Application, or Project node
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private void SetState()
        {
            try
            {
            }
            catch { throw; }
        }
       
        #endregion

        #region Public Methods

        public int CompareTo(TagClass other)
        {
            return OrderNumber.CompareTo(other.OrderNumber);
        }

        public TagClass Clone()
        {
            TagClass result = (TagClass)((ICloneable)this).Clone();
            return result;
        }

        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
