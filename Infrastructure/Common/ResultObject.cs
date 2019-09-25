namespace ELOTEC.Infrastructure.Common
{
    using System;
    using System.Collections.Generic;

    public class ResultObject : Dictionary<string, Object>
    {
        private string _typeName;

        /// <summary>
        /// Initializes a new instance of the ResultObject class.
        /// </summary>
        public ResultObject()
        {
        }
        /// <summary>
        /// Initializes a new instance of the ResultObject class.
        /// </summary>
        /// <param name="typeName">Typed object type name.</param>
        public ResultObject(string typeName)
        {
            _typeName = typeName;
        }
        /// <summary>
        /// Initializes a new instance of the ResultObject class by copying the elements from the specified dictionary to the new ResultObject object.
        /// </summary>
        /// <param name="dictionary">The IDictionary object to copy to a new ResultObject object.</param>
        public ResultObject(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Gets or sets the type name for a typed object.
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }
        /// <summary>
        /// Gets the Boolean value indicating whether the ResultObject is typed.
        /// </summary>
        public bool IsTypedObject
        {
            get { return _typeName != null && _typeName != string.Empty; }
        }
    }
}
