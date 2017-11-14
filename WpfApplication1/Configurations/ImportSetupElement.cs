using NAVObjectCompareWinClient.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.Configurations
{
    public class ImportSetupElement : ConfigurationElement
    {
        private const string NameKey = "Name";
        private const string ServerSetupNameElement = "ServerSetupName";
        private const string ModifiedElement = "Modified";
        private const string DateFromElement = "DateFrom";
        private const string DateToElement = "DateTo";
        private const string VersionListElement = "VersionList";
        private const string CustomFilterElement = "CustomFilter";
        private const string FilterElement = "Filter";
        private const string ImportFileNameElement = "ImportFileName";
        private const string ImportTypeElement = "ImportType";

        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        [ConfigurationProperty(ImportTypeElement, IsRequired = true, IsKey = false)]
        public int ImportType
        {
            get { return (int)this[ImportTypeElement]; }
            set { this[ImportTypeElement] = value; }
        }

        [ConfigurationProperty(ServerSetupNameElement, IsRequired = false, IsKey = false)]
        public string ServerSetupName
        {
            get { return (string)this[ServerSetupNameElement]; }
            set { this[ServerSetupNameElement] = value; }
        }

        [ConfigurationProperty(ModifiedElement, IsRequired = false, IsKey = false)]
        public bool Modified
        {
            get { return (bool)this[ModifiedElement]; }
            set { this[ModifiedElement] = value; }
        }

        [ConfigurationProperty(DateFromElement, IsRequired = false, IsKey = false)]
        public DateTime? DateFrom
        {
            get { return (DateTime?)this[DateFromElement]; }
            set { this[DateFromElement] = value; }
        }

        [ConfigurationProperty(DateToElement, IsRequired = false, IsKey = false)]
        public DateTime? DateTo
        {
            get { return (DateTime?)this[DateToElement]; }
            set { this[DateToElement] = value; }
        }

        [ConfigurationProperty(VersionListElement, IsRequired = false, IsKey = false)]
        public string VersionList
        {
            get { return (string)this[VersionListElement]; }
            set { this[VersionListElement] = value; }
        }

        [ConfigurationProperty(CustomFilterElement, IsRequired = false, IsKey = false)]
        public bool CustomFilter
        {
            get { return (bool)this[CustomFilterElement]; }
            set { this[CustomFilterElement] = value; }
        }

        [ConfigurationProperty(FilterElement, IsRequired = false, IsKey = false)]
        public string Filter
        {
            get { return (string)this[FilterElement]; }
            set { this[FilterElement] = value; }
        }

        [ConfigurationProperty(ImportFileNameElement, IsRequired = false, IsKey = false)]
        public string ImportFileName
        {
            get { return (string)this[ImportFileNameElement]; }
            set { this[ImportFileNameElement] = value; }
        }

        public ImportSheetModel ToImportSetupModel()
        {
            return new ImportSheetModel(Name)
            {
                ImportType = (ImportTypes)ImportType,
                ServerSetupName = ServerSetupName,
                Modified = Modified,
                DateFrom = DateFrom,
                DateTo = DateTo,
                VersionList = VersionList,
                CustomFilter = CustomFilter,
                Filter = Filter,
                ImportFileName = ImportFileName
            };
        }

        public void Fill(ImportSheetModel importSetup)
        {
            Name = importSetup.Name;
            ImportType = (int)importSetup.ImportType;
            ServerSetupName = importSetup.ServerSetupName;
            Modified = importSetup.Modified;
            DateFrom = importSetup.DateFrom;
            DateTo = importSetup.DateTo;
            VersionList = importSetup.VersionList;
            CustomFilter = importSetup.CustomFilter;
            Filter = importSetup.Filter;
            ImportFileName = importSetup.ImportFileName;
        }
    }

    public class ImportSetupElementCollection : ConfigurationElementCollection
    {
        public ImportSetupElementCollection()
        {
            this.AddElementName = "ImportSetup";
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ImportSetupElement).Name;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ImportSetupElement();
        }

        public new ImportSetupElement this[string key]
        {
            get { return base.BaseGet(key) as ImportSetupElement; }
        }

        public ImportSetupElement this[int index]
        {
            get { return base.BaseGet(index) as ImportSetupElement; }
        }

        public void Add(ImportSetupElement importSetupElement)
        {
            base.BaseAdd(importSetupElement);
        }

        public void Remove(ImportSetupElement importSetupElement)
        {
            base.BaseRemove(importSetupElement.Name);
        }
    }

    public class ImportSetupSection : ConfigurationSection
    {
        public const string SectionName = "ImportSetupGroup/ImportSetups";

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ImportSetupElementCollection ImportSetups
        {
            get
            {
                return this[""] as ImportSetupElementCollection;
            }
        }

        public static ImportSetupSection GetSection()
        {
            return (ImportSetupSection)ConfigurationManager.GetSection(SectionName);
        }
    }
}
