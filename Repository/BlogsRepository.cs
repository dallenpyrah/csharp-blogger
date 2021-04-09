using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using blogger.Models;
using Dapper;

namespace blogger.Repository
{
    public class BlogsRepository
    {

        public readonly IDbConnection _db;

        public BlogsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal IEnumerable<Blog> GetBlogsByProfileId(string id)
        {
            string sql = @"SELECT
            b.*,
            p.*
            FROM theblogs b
            JOIN profiles p ON b.creatorId = p.id
            WHERE creatorId = @id;";
            return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) =>
            {
                blog.Creator = profile;
                return blog;
            }, new { id }, splitOn: "id");
        }

        internal IEnumerable<Blog> GetAll()
        {
            string sql = @"SELECT
            b.*,
            p.*
            FROM theblogs b
            JOIN profiles p ON b.creatorId = p.id;";
            return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) =>
            {
                blog.Creator = profile;
                return blog;
            }, splitOn: "id");
        }

        internal Blog GetOneById(int id)
        {
            string sql = @"SELECT
            b.*,
            p.*
            FROM theblogs b
            JOIN profiles p ON b.creatorId = p.id
            WHERE b.id = @id;";
            return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) =>
            {
                blog.Creator = profile;
                return blog;
            }, new { id }, splitOn: "id").FirstOrDefault();
        }

        internal Blog CreateBlog(Blog newBlog)
        {
            string sql = @"
            INSERT INTO theblogs
            (title, body, imgUrl, published, creatorId)
            VALUES
            (@Title, @Body, @ImgUrl, @Published, @CreatorId);
            SELECT LAST_INSERT_ID();";
            int id = _db.ExecuteScalar<int>(sql, newBlog);
            newBlog.Id = id;
            return newBlog;
        }

        internal Blog EditBlog(Blog current)
        {
            string sql = @"
            UPDATE theblogs
            SET
                title = @Title,
                body = @Body,
                imgUrl = @ImgUrl,
                published = @Published
                WHERE id = @id;
            SELECT * FROM theblogs WHERE id = @id;";
            return _db.QueryFirstOrDefault<Blog>(sql, current);
        }

        internal void DeleteBlog(int id, string userInfoId)
        {
            string sql = "DELETE FROM theblogs WHERE id = @id AND creatorId = @userInfoId;";
            _db.Execute(sql, new { id, userInfoId });
            return;
        }
    }
}