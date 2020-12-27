using System;
using System.Collections.Generic;
using Radyn.Framework;

namespace Radyn.Congress.Tools
{
    [Schema("ModelView")]
    public static class ModelView
    {
        [Serializable]
        public class ModifyResult<T>
        {
            public ModifyResult()
            {
                InformList = new InFormEntitiyList<T>();
            }

            public bool Result { get; set; }
            public bool SendInform { get; set; }
            public InFormEntitiyList<T> InformList { get; set; }
            public T RefObject { get; set; }
            public Guid TransactionId { get; set; }

            public void AddInform(T obj, string emilnody, string smsbody)
            {
                if (obj == null) return;
                InformList.Add(obj, emilnody, smsbody);


            }
        }
        [Serializable]
        public class InFormEntitiyList<T> : List<InFormObject<T>>
        {

            public void Add(T obj, string emilnody, string smsbody)
            {
                if (obj == null) return;
                var inFormBody = new InFormObject<T> { obj = obj, SmsBody = smsbody, EmailBody = emilnody };
                this.Add(inFormBody);

            }
        }
        [Serializable]
        public class InFormObject<T>
        {
            public string EmailBody { get; set; }
            public string SmsBody { get; set; }
            public T obj { get; set; }
        }



        [Schema("SerachResultvalue")]
        public class SerachResultvalue
        {
            public string Key { get; set; }
            public string ResultValue { get; set; }
            public Enums.SearchType SearchType { get; set; }
        }
        [Schema("ReportChartModel")]
        public class ReportChartModel
        {


            public string Value { get; set; }
            public long Count { get; set; }
            public string StringFormat { get; set; }
        }
        [Schema("UserCardModel")]
        public class UserCardModel
        {

            public string Id { get; set; }
            public long Number { get; set; }
            public byte[] CongressLogo { get; set; }
            public byte[] UserImage { get; set; }
            public string CongressTitle { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UseName { get; set; }
            public string NationalCode { get; set; }
            public string Type { get; set; }
            public string ChairHallParent { get; set; }
            public string ChairHall { get; set; }
            public string ChairRow { get; set; }
            public string ChairNumber { get; set; }
            public string ChairColumn { get; set; }
            public Enums.CardType CardType { get; set; }
            public string CardId { get; set; }
            public string ChipFoodName { get; set; }
            public string ChipFoodDescription { get; set; }
            public string ChipFoodDays { get; set; }
            public string RegisterDays { get; set; }
            public string Description { get; set; }
            public string Prefix { get; set; }
        }
        [Schema("UserArticleAbstract")]
        public class UserArticleAbstract
        {

            public string Id { get; set; }
            public byte[] CongressLogo { get; set; }
            public string CongressTitle { get; set; }
            public string Abstract { get; set; }
            public string OrginalTextFile { get; set; }
            public string Title { get; set; }


            public string Keyword { get; set; }

            public string Authors { get; set; }
            public string Description { get; set; }

            public string Pivot { get; set; }

        }


        [Schema("ArticleCertificateModel")]
        public class ArticleCertificateModel
        {
            public Guid Id { get; set; }
            public string CongressTitle { get; set; }
            public string ArticleTitle { get; set; }
            public string ArticleCode { get; set; }
            public string ArtileTypeTitle { get; set; }
            public string Author { get; set; }
            public string AuthorDirector { get; set; }

            public string Percentage { get; set; }

        }

    }
}
