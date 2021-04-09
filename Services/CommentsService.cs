using System;
using System.Collections.Generic;
using blogger.Models;
using blogger.Repository;
using Microsoft.AspNetCore.Mvc;

namespace blogger.Services
{
    public class CommentsService
    {

        private readonly CommentsRepository _repo;

        public CommentsService(CommentsRepository repo)
        {
            _repo = repo;
        }

        internal IEnumerable<Comment> GetAll()
        {
            return _repo.GetAll();
        }

        internal ActionResult<Comment> CreateComment(Comment newComment)
        {
            return _repo.CreateComment(newComment);
        }

        internal Comment EditComment(Comment editedComment)
        {
            Comment original = _repo.GetOneById(editedComment.Id);
            if(original == null){
                throw new SystemException("INVALID ID");
            }else{
                original.Body = editedComment.Body != null ? editedComment.Body : original.Body;
                return _repo.EditComment(original);
            }
        }

        internal object DeleteComment(int id, string userInfoId)
        {
            _repo.GetOneById(id);
            _repo.DeleteComment(id, userInfoId);
            string deleted = "Comment Deleted";
            return deleted;
        }

        internal IEnumerable<Comment> GetCommentsByBlogId(int id)
        {
            return _repo.GetCommentsByBlogId(id);
        }

        internal IEnumerable<Comment> GetCommentsByProfileId(string id)
        {
            return _repo.GetCommentsByProfileId(id);
        }
    }
}