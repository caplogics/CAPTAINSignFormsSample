/************************************************************************************
 * Class Name   : CaptainException
 * Author       : 
 * Created Date : 
 * Version      : 
 * Description  : Created to Handle Generic Exception raised in the ViewPoint Application.
 *
 *************************************************************************************/

#region Using

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections; 

#endregion

namespace Captain.Common.Exceptions
{
    [Serializable]
    public class CaptainException : ApplicationException
    {
        #region Properties

        /// <summary>
        /// override <b>Message</b> property of 
        /// <b>Exception</b> class
        /// </summary>
        public override string Message
        {
            get { return base.Message; }
        }

        /// <summary>
        /// override <b>HelpLink</b> property of 
        /// <b>Exception</b> class
        /// </summary>
        public override string HelpLink
        {
            get { return base.HelpLink; }
            set { base.HelpLink = value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// override <b>Equals</b> method of 
        /// <b>Exception</b> class
        /// </summary>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// override <b>GetBaseException</b> method of 
        /// <b>Exception</b> class
        /// </summary>
        public override Exception GetBaseException()
        {
            return base.GetBaseException();
        }

        /// <summary>
        /// override <b>GetHashCode</b> method of 
        /// <b>Exception</b> class
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// override <b>GetObjectData</b> method of 
        /// <b>Exception</b> class
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// override <b>Source</b> property of 
        /// <b>Exception</b> class
        /// </summary>
        public override string Source
        {
            get { return base.Source; }
            set { base.Source = value; }
        }

        /// <summary>
        /// override <b>StackTrace</b> property of 
        /// <b>Exception</b> class
        /// </summary>
        public override string StackTrace
        {
            get { return base.StackTrace; }
        }

        /// <summary>
        /// override <b>ToString</b> method of 
        /// <b>Exception</b> class
        /// </summary>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// override <b>Data</b> property of 
        /// <b>Exception</b> class
        /// </summary>
        public override IDictionary Data
        {
            get { return base.Data; }
        }

        /// <summary>
        /// InnerException
        /// </summary>
        public new Exception InnerException
        {
            get { return base.InnerException; }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public CaptainException() : base()
        {
        }

        /// <summary>
        /// overloaded constructor
        /// </summary>
        public CaptainException(string message) : base(message)
        {
        }

        /// <summary>
        /// overloaded constructor
        /// </summary>
        public CaptainException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// overloaded constructor
        /// </summary>
        protected CaptainException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion
    }
}
