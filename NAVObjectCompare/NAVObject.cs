using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare
{
    public class NavObject
    {
        public enum Difference { None, Unexisting, Id, Name, DateOrTime, ModifiedFlag, Code }

        private List<string> _lines = null;

        public NavObject()
        {
            _lines = new List<string>();
        }

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
            if(_lines.Count > index)
                return _lines[index];

            return string.Empty;
        }

        public List<string> Lines { get { return _lines; } }

        public bool IsEqualTo(NavObject b, out Difference difference)
        {
            difference = Difference.None;

            if(b == null)
            {
                difference = Difference.Unexisting;
                return false;
            }

            if (InternalId != b.InternalId)
            {
                difference = Difference.Id;
                return false;
            }

            if (Name != b.Name)
            {
                difference = Difference.Name;
                return false;
            }

            // Code Start

            ObjectSection currObjectSectionA = ObjectSection.Empty;
            ObjectSection currObjectSectionB = ObjectSection.Empty;

            for (int i = 0; i < this.Lines.Count; i++)
            {
                string lineA = GetLine(i);
                string lineB = b.GetLine(i);

                ObjectSection objectSectionA = ObjectHelper.FindObjectKeyWord(lineA);
                if (objectSectionA != ObjectSection.Empty)
                    currObjectSectionA = objectSectionA;

                ObjectSection objectSectionB = ObjectHelper.FindObjectKeyWord(lineB);
                if (objectSectionB != ObjectSection.Empty)
                    currObjectSectionB = objectSectionB;

                if (!lineA.Equals(lineB))
                {
                    if ((currObjectSectionA == ObjectSection.Code) || (currObjectSectionB == ObjectSection.Code))
                    {
                        difference = Difference.Code;
                        return false;
                    }
                }
            }
            
            // Code Stop

            if ((StringTime != b.StringTime) || (StringDate != b.StringDate))
            {
                difference = Difference.DateOrTime;
                return false;
            }

            if (Modified != b.Modified)
            {
                difference = Difference.ModifiedFlag;
                return false;
            }

            if (this.Lines.Count != b.Lines.Count)
            {
                difference = Difference.Code;
                return false;
            }

            return true;
        }


        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            NavObject b = obj as NavObject;
            if ((System.Object)b == null)
            {
                return false;
            }

            return Equals(b);
        }

        public bool Equals(NavObject b)
        {
            if (InternalId != b.InternalId)
                return false;

            if (this.Lines.Count != b.Lines.Count)
                return false;

            for (int i = 0; i < this.Lines.Count; i++)
            {
                string lineA = GetLine(i);
                string lineB = b.GetLine(i);

                if (!lineA.Equals(lineB))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() * Id;
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
    }
}
