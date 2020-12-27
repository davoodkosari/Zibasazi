using Radyn.Framework;
using Radyn.Framework.DbHelper;
using System;
using System.Data.SqlClient;

namespace Radyn.Gallery.DA
{
    public sealed class GalleryDA : DALBase<Gallery.DataStructure.Gallery>
    {
        public GalleryDA(IConnectionHandler connectionHandler)
            : base(connectionHandler)
        { }

        public int HasPhoto(Guid id)
        {
            GalleryCommandBuilder galleryCommandBuilder = new GalleryCommandBuilder();
            var query = galleryCommandBuilder.HasPhoto(id);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }

        public int HasChild(Guid id)
        {
            GalleryCommandBuilder galleryCommandBuilder = new GalleryCommandBuilder();
            SqlCommand query = galleryCommandBuilder.HasChild(id);
            return DBManager.ExecuteScalar<int>(base.ConnectionHandler, query);
        }


    }
    internal class GalleryCommandBuilder
    {
        public SqlCommand HasPhoto(Guid id)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@ID", id));
            query.CommandText =
            "SELECT   COUNT(Id) FROM         Gallery.Photo where Gallery.Photo.GalleryId=@ID";
            return query;
        }

        public SqlCommand HasChild(Guid id)
        {
            SqlCommand query = new SqlCommand();
            query.Parameters.Add(new SqlParameter("@ID", id));
            query.CommandText =
            "SELECT   COUNT(Id) FROM         Gallery.Gallery where Gallery.ParentGallery=@ID";
            return query;
        }


    }
}
