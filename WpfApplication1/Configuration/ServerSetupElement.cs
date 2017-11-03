using NAVObjectCompareWinClient.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.Configuration
{
    public class ServerSetupElement : ConfigurationElement
    {
        private const string NameKey = "Name";
        private const string FinSQLPathElement = "FinSQLPath";
        private const string ServerElement = "Server";
        private const string DatabaseElement = "Database";
        private const string UseNTAuthenticationElement = "UseNTAuthentication";
        private const string UserNameElement = "UserName";
        private const string PasswordElement = "Password";

        [ConfigurationProperty(NameKey, IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this[NameKey]; }
            set { this[NameKey] = value; }
        }

        [ConfigurationProperty(FinSQLPathElement, IsRequired = true, IsKey = false)]
        public string FinSQLPath
        {
            get { return (string)this[FinSQLPathElement]; }
            set { this[FinSQLPathElement] = value; }
        }

        [ConfigurationProperty(ServerElement, IsRequired = true, IsKey = false)]
        public string Server
        {
            get { return (string)this[ServerElement]; }
            set { this[ServerElement] = value; }
        }

        [ConfigurationProperty(DatabaseElement, IsRequired = true, IsKey = false)]
        public string Database
        {
            get { return (string)this[DatabaseElement]; }
            set { this[DatabaseElement] = value; }
        }

        [ConfigurationProperty(UseNTAuthenticationElement, IsRequired = true, IsKey = false)]
        public bool UseNTAuthentication
        {
            get { return (bool)this[UseNTAuthenticationElement]; }
            set { this[UseNTAuthenticationElement] = value; }
        }

        [ConfigurationProperty(UserNameElement, IsRequired = true, IsKey = false)]
        public string UserName
        {
            get { return (string)this[UserNameElement]; }
            set { this[UserNameElement] = value; }
        }

        [ConfigurationProperty(PasswordElement, IsRequired = true, IsKey = false)]
        public string Password
        {
            get { return (string)this[PasswordElement]; }
            set { this[PasswordElement] = value; }
        }

        public ServerSetupModel ToServerSetupModel()
        {
            return new ServerSetupModel
            {
                Name = Name,
                FinSQLPath = FinSQLPath,
                Server = this.FinSQLPath,
                Database = this.Database,
                UseNTAuthentication = UseNTAuthentication,
                UserName = UserName,
                Password = Password
            };
        }
    }

    public class ServerSetupElementCollection : ConfigurationElementCollection
    {
        public ServerSetupElementCollection()
        {
            this.AddElementName = "ServerSetup";
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ServerSetupElement).Name;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerSetupElement();
        }

        public new ServerSetupElement this[string key]
        {
            get { return base.BaseGet(key) as ServerSetupElement; }
        }

        public ServerSetupElement this[int index]
        {
            get { return base.BaseGet(index) as ServerSetupElement; }
        }
    }

    public class ServerSetupSection : ConfigurationSection
    {
        public const string _sectionName = "ServerSetups";

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ServerSetupElementCollection ServerSetups
        {
            get
            {
                return this[""] as ServerSetupElementCollection;
            }
        }

        public static ServerSetupSection GetSection()
        {
            return (ServerSetupSection)ConfigurationManager.GetSection(_sectionName);
        }
    }
}
