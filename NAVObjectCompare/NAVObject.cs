using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare
{
    public enum ObjectSection { Unknown, Object, ObjectProperties, Properties, Fields, Keys, FieldGroups, Code };
    public class NavObject
    {
        private List<string> _objectLines = new List<string>();
        private List<string> _objectProperties = new List<string>();
        private List<string> _properties = new List<string>();
        private List<string> _fields = new List<string>();
        private List<string> _keys = new List<string>();
        private List<string> _fieldGroups = new List<string>();
        private List<string> _code = new List<string>();

        public string InternalId
        {
            get
            {
                if (string.IsNullOrEmpty(this.Type))
                    throw new Exception("'Type' cannot be null or empty.");

                return string.Format("{0}-{1}", this.Type.ToUpper(), this.Id);
            }
        }

        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime
        {
            get
            {
                if ((!string.IsNullOrEmpty(this.StringDate)) && (!string.IsNullOrEmpty(this.StringTime)))
                {
                    string stringDateTime = string.Format("{0} {1}", this.StringDate, this.StringTime);
                    return ObjectHelper.GetDateTime(stringDateTime);
                }

                return DateTime.MinValue;
            }
        }
        public bool Modified { get; set; } 
        public string VersionList { get; set; }
        public string StringTime { get; set; }
        public string StringDate { get; set; }

        public string GetLine(int index)
        {
            if(_objectLines.Count > index)
                return _objectLines[index];

            return string.Empty;
        }

        public List<string> ObjectLines { get { return _objectLines; } }
        public List<string> ObjectProperties { get { return _objectProperties; } }
        public List<string> Properties { get { return _properties; } }
        public List<string> Fields { get { return _fields; } }
        public List<string> Keys { get { return _keys; } }
        public List<string> FieldGroups { get { return _fieldGroups; } }
        public List<string> Code { get { return _code; } }

        public bool IsEqualTo(NavObject objectToCompare)
        {
            if (objectToCompare == null)
                return false;

            if (this.ObjectLines.Count != objectToCompare.ObjectLines.Count)
                return false;

            for (int i = 0; i < this.ObjectLines.Count; i++)
            {
                string lineA = GetLine(i);
                string lineB = objectToCompare.GetLine(i);

                if (!lineA.Equals(lineB))
                    return false;
            }

            return true;
        }

        public bool IsObjectPropertiesEqual(NavObject objectToCompare)
        {
            if (objectToCompare == null)
                return false;

            if (this.ObjectProperties.Count != objectToCompare.ObjectProperties.Count)
                return false;

            for (int i = 0; i < this.ObjectProperties.Count; i++)
            {
                string lineA = this.ObjectProperties[i];
                string lineB = string.Empty;
                if (objectToCompare.ObjectProperties.Count > i)
                    lineB = objectToCompare.ObjectProperties[i];

                if (!lineA.Equals(lineB))
                    return false;
            }

            return true;
        }

        public bool IsPropertiesEqual(NavObject objectToCompare)
        {
            if (objectToCompare == null)
                return false;

            if (this.Properties.Count != objectToCompare.Properties.Count)
                return false;

            for (int i = 0; i < this.Properties.Count; i++)
            {
                string lineA = this.Properties[i];
                string lineB = string.Empty;
                if (objectToCompare.Properties.Count > i)
                    lineB = objectToCompare.Properties[i];

                if (!lineA.Equals(lineB))
                    return false;
            }

            return true;
        }

        public bool IsCodeEqual(NavObject objectToCompare)
        {
            if (objectToCompare == null)
                return false;

            if (this.Code.Count != objectToCompare.Code.Count)
                return false;

            for (int i = 0; i < this.Code.Count; i++)
            {
                string lineA = this.Code[i];
                string lineB = string.Empty;
                if (objectToCompare.Code.Count > i)
                    lineB = objectToCompare.Code[i];

                if (!lineA.Equals(lineB))
                    return false;
            }

            return true;
        }

        public static bool operator ==(NavObject a, NavObject b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.InternalId == b.InternalId;
        }

        public static bool operator !=(NavObject a, NavObject b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
