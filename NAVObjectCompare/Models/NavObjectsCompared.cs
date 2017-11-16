using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare.Models
{
    public class NavObjectsCompared
    {
        public enum EqualStatus { Equal = 0, Unequal = 1, Unexisting = 2 }

        public NavObjectsCompared(string internalId)
        {
            InternalId = internalId;
            Selected = false;
            Id = 0;
            Type = string.Empty;
            Name = string.Empty;
            StringDateA = string.Empty;
            StringDateB = string.Empty;
            StringTimeA = string.Empty;
            StringTimeB = string.Empty;
            VersionListA = string.Empty;
            VersionListB = string.Empty;
            NoOfLinesA = 0;
            NoOfLinesB = 0;
            Status = EqualStatus.Equal;
            ObjectPropertiesEqual = true;
            CodeEqual = true;
            Finished = false;
            Edited = false;
            Comment = string.Empty;
        }

        public string InternalId { get; private set; }
        public bool Selected { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string StringDateA { get; set; }
        public string StringDateB { get; set; }
        public string StringTimeA { get; set; }
        public string StringTimeB { get; set; }
        public string VersionListA { get; set; }
        public string VersionListB { get; set; }
        public int NoOfLinesA { get; set; }
        public int NoOfLinesB { get; set; }
        public EqualStatus Status { get; set; }
        public bool ObjectPropertiesEqual { get; set; }
        public bool CodeEqual { get; set; }
        public bool Finished { get; set; }
        public bool Edited { get; set; }
        public string Comment { get; set; }

        #region Serialize
        public void Serialize(ref BinaryWriter writer)
        {
            writer.Write(InternalId);
            writer.Write(Selected);
            writer.Write(Id);
            writer.Write(Type);
            writer.Write(Name);
            writer.Write(StringDateA);
            writer.Write(StringDateB);
            writer.Write(StringTimeA);
            writer.Write(StringTimeB);
            writer.Write(VersionListA);
            writer.Write(VersionListB);
            writer.Write(NoOfLinesA);
            writer.Write(NoOfLinesB);
            writer.Write((int)Status);
            writer.Write(ObjectPropertiesEqual);
            writer.Write(CodeEqual);
            writer.Write(Finished);
            writer.Write(Edited);
            writer.Write(Comment);
        }

        public static NavObjectsCompared Desserialize(ref BinaryReader reader)
        {
            string internalId = reader.ReadString();

            return new NavObjectsCompared(internalId)
            {
                Selected = reader.ReadBoolean(),
                Id = reader.ReadInt32(),
                Type = reader.ReadString(),
                Name = reader.ReadString(),
                StringDateA = reader.ReadString(),
                StringDateB = reader.ReadString(),
                StringTimeA = reader.ReadString(),
                StringTimeB = reader.ReadString(),
                VersionListA = reader.ReadString(),
                VersionListB = reader.ReadString(),
                NoOfLinesA = reader.ReadInt32(),
                NoOfLinesB = reader.ReadInt32(),
                Status = (EqualStatus)reader.ReadInt32(),
                ObjectPropertiesEqual = reader.ReadBoolean(),
                CodeEqual = reader.ReadBoolean(),
                Finished = reader.ReadBoolean(),
                Edited = reader.ReadBoolean(),
                Comment = reader.ReadString()
            };
        }

        #endregion Serialize
    }
}
