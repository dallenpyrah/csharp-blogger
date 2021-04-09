using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using blogger.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace blogger.Repository
{
    public class CommentsRepository
    {

        public readonly IDbConnection _db;

        public CommentsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal IEnumerable<Comment> GetAll()
        {

            string sql = @"SELECT
            c.*,
            p.*
            FROM thecomments c
            JOIN profiles p ON c.creatorId = p.id;";
            return _db.Query<Comment, Profile, Comment>(sql, (comment, profile) =>
            {
                comment.Creator = profile;
                return comment;
            }, splitOn: "id");
        }

        internal ActionResult<Comment> CreateComment(Comment newComment)
        {
            string sql = @"INSERT INTO thecomments
            (body, blogId, creatorId)
            VALUES
            (@Body, @BlogId, @CreatorId);
            SELECT LAST_INSERT_ID();";
            int id = _db.ExecuteScalar<int>(sql, newComment);
            newComment.Id = id;
            return newComment;
        }

        internal Comment GetOneById(int id)
        {
            string sql = @"SELECT
            c.*,
            p.*
            FROM thecomments c
            JOIN profiles p ON c.creatorId = p.id
            WHERE c.id = @id;";
            return _db.Query<Comment, Profile, Comment>(sql, (comment, profile) =>
            {
                comment.Creator = profile;
                return comment;
            }, new { id }, splitOn: "id").FirstOrDefault();
        }

        internal Comment EditComment(Comment original)
        {
            string sql = @"UPDATE thecomments
            SET
                body = @Body
                WHERE id = @id;";
            return _db.QueryFirstOrDefault<Comment>(sql, original);
        }

        internal void DeleteComment(int id, string userInfoId)
        {
            string sql = "DELETE FROM thecomments WHERE id = @id and creatorId = @userInfoId;";
            _db.Execute(sql, new { id, userInfoId });
            return;
        }
    }
}