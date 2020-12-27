using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Radyn.XmlModel
{

    [Serializable, XmlRoot("Congress",Namespace = "Radyn.XmlModel"), DataContract(Name = "Congress")]
    public class CongressXml
    {

        public CongressXml()
        {
            if (AttendanceType == null)
                AttendanceType = new List<KeyValueXml>();

            if (CongressModelXml == null)
                CongressModelXml = new List<CongressModelXml>();


        }
        [XmlElement(ElementName = "AttendanceType"), DataMember(Name = "AttendanceType")]
        public List<KeyValueXml> AttendanceType { get; set; }



        [XmlElement(ElementName = "Congress"), DataMember(Name = "Congress")]

        public List<CongressModelXml> CongressModelXml { get; set; }









    }

    [Serializable]
    public class WorkShopModelXml : KeyValueXml
    {
        public WorkShopModelXml()
        {
            if (UserList == null)
                UserList = new List<KeyValueXml>();
        }
        [XmlElement(ElementName = "User"), DataMember(Name = "User")]

        public List<KeyValueXml> UserList { get; set; }



    }
    [Serializable]
    public class CongressModelXml
    {

        public CongressModelXml()
        {
            if (UserList == null)
                UserList = new List<KeyValueXml>();

            if (WorkShopModelList == null)
                WorkShopModelList = new List<WorkShopModelXml>();
        }

        [XmlElement(ElementName = "User"), DataMember(Name = "User")]

        public List<KeyValueXml> UserList { get; set; }



        [XmlAttribute(AttributeName = "Id"), DataMember(Name = "Id")]
        public Guid CongressId { get; set; }

        [XmlAttribute(AttributeName = "Title"), DataMember(Name = "Title")]
        public string Title { get; set; }


        [XmlElement(ElementName = "WorkShop"), DataMember(Name = "WorkShop")]

        public List<WorkShopModelXml> WorkShopModelList { get; set; }


    }





    [Serializable]
    public class WorkShopAbsentXml
    {
        public WorkShopAbsentXml()
        {
            if (AbsentItem == null)
                AbsentItem = new List<AbsentItemXml>();
        }
        [XmlElement(ElementName = "User"), DataMember(Name = "User")]
        public List<AbsentItemXml> AbsentItem { get; set; }

        [XmlAttribute(AttributeName = "WorkShopId"), DataMember(Name = "WorkShopId")]
        public Guid WorkShopId { get; set; }

        [XmlAttribute(AttributeName = "CongressId"), DataMember(Name = "CongressId")]

        public Guid CongressId { get; set; }


    }
    [Serializable]
    public class CongressAbsentXml
    {
        public CongressAbsentXml()
        {
            if (AbsentItem == null)
                AbsentItem = new List<AbsentItemXml>();
        }

        [XmlElement(ElementName = "User"), DataMember(Name = "User")]
        public List<AbsentItemXml> AbsentItem { get; set; }


        [XmlAttribute(AttributeName = "Id"), DataMember(Name = "Id")]
        public Guid CongressId { get; set; }

    }

    [Serializable]
    public class AbsentItemXml
    {
        [XmlAttribute(AttributeName = "Value"), DataMember(Name = "Value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "Date"), DataMember(Name = "Date")]
        public string Date { get; set; }

    }

    [Serializable]
    public class KeyValueXml
    {
        [XmlAttribute(AttributeName = "Value"), DataMember(Name = "Value")]
        public string Value { get; set; }

        [XmlAttribute(AttributeName = "Key"), DataMember(Name = "Key")]
        public string Key { get; set; }



    }
    [Serializable,XmlRoot(ElementName = "Attendance", Namespace = "Radyn.XmlModel"), DataContract(Name = "Attendance")]
    public class AttendanceXml
    {
        public AttendanceXml()
        {

            if (WorkShopAbsentList == null)
                WorkShopAbsentList = new List<WorkShopAbsentXml>();

            if (CongressAbsentList == null)
                CongressAbsentList = new List<CongressAbsentXml>();
        }

        [XmlElement(ElementName = "WorkShop"), DataMember(Name = "WorkShop")]
        public List<WorkShopAbsentXml> WorkShopAbsentList { get; set; }

        [XmlElement(ElementName = "Congress"), DataMember(Name = "Congress")]

        public List<CongressAbsentXml> CongressAbsentList { get; set; }


    }

}
