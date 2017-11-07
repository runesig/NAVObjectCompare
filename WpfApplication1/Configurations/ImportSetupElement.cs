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

        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        [ConfigurationProperty(ServerSetupNameElement, IsRequired = true, IsKey = false)]
        public string ServerSetupName
        {
            get { return (string)this[ServerSetupNameElement]; }
            set { this[ServerSetupNameElement] = value; }
        }

        [ConfigurationProperty(ModifiedElement, IsRequired = true, IsKey = false)]
        public bool Modified
        {
            get { return (bool)this[ModifiedElement]; }
            set { this[ModifiedElement] = value; }
        }

        [ConfigurationProperty(DateFromElement, IsRequired = true, IsKey = false)]
        public DateTime? DateFrom
        {
            get { return (DateTime)this[DateFromElement]; }
            set { this[DateFromElement] = value; }
        }

        [ConfigurationProperty(DateToElement, IsRequired = true, IsKey = false)]
        public DateTime? DateTo
        {
            get { return (DateTime)this[DateToElement]; }
            set { this[DateToElement] = value; }
        }

        [ConfigurationProperty(VersionListElement, IsRequired = true, IsKey = false)]
        public string VersionList
        {
            get { return (string)this[VersionListElement]; }
            set { this[VersionListElement] = value; }
        }

        [ConfigurationProperty(CustomFilterElement, IsRequired = true, IsKey = false)]
        public bool CustomFilter
        {
            get { return (bool)this[CustomFilterElement]; }
            set { this[CustomFilterElement] = value; }
        }

        [ConfigurationProperty(FilterElement, IsRequired = true, IsKey = false)]
        public string Filter
        {
            get { return (string)this[FilterElement]; }
            set { this[FilterElement] = value; }
        }

        public ImportSetupModel ToImportSetupModel()
        {
            return new ImportSetupModel()
            {
                Name = Name,
                ServerSetupName = ServerSetupName,
                Modified = Modified,
                DateFrom = DateFrom,
                DateTo = DateTo,
                CustomFilter = CustomFilter,
                Filter = Filter
            };
        }

        public void Fill(ImportSetupModel importSetup)
        {
            Name = importSetup.Name;
            ServerSetupName = importSetup.ServerSetupName;
            Modified = importSetup.Modified;
            DateFrom = importSetup.DateFrom;
            DateTo = importSetup.DateTo;
            CustomFilter = importSetup.CustomFilter;
            Filter = importSetup.Filter;
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
