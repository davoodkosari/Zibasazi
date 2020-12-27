using System.Configuration;

namespace Radyn.Congress.Settings
{
    public class JobSchedulerSetting : ConfigurationSection
    {
        [ConfigurationProperty("dailySchedulerTriggers")]
        public TriggerCollection Triggers
        {
            get { return (TriggerCollection)this["dailySchedulerTriggers"]; }
        }
    }

    public sealed class TriggerCollection : ConfigurationElementCollection
    {
        public void Add(Trigger element)
        {
            this.BaseAdd((element));
        }

        public void Clear()
        {
            base.BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Trigger();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Trigger)element).Id;
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public Trigger this[string id]
        {
            get { return (Trigger)base.BaseGet(id); }
            set
            {
                if (base.BaseGet(id) != null)
                {
                    base.BaseRemove(id);
                }
                this.BaseAdd(value);
            }
        }

        public Trigger this[int index]
        {
            get { return (Trigger)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }
    }

    public sealed class Trigger : ConfigurationElement
    {
        [ConfigurationProperty("hour", IsRequired = true)]
        public int Hour
        {
            get { return (int)base["hour"]; }
            set { base["hour"] = value; }
        }

        [ConfigurationProperty("minute", IsRequired = true)]
        public int Minute
        {
            get { return (int)base["minute"]; }
            set { base["minute"] = value; }
        }

        [ConfigurationProperty("priority", DefaultValue = 5)]
        public int Priority
        {
            get { return (int)base["priority"]; }
            set { base["priority"] = value; }
        }

        [ConfigurationProperty("class", IsRequired = true)]
        public string Class
        {
            get { return (string)base["class"]; }
            set { base["class"] = value; }
        }

        [ConfigurationProperty("Id", IsRequired = true)]
        public string Id
        {
            get { return (string)base["Id"]; }
            set { base["Id"] = value; }
        }
    }


   
}
