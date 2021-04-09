using System;
using System.Collections.Generic;
using blogger.Models;
using blogger.Repository;

namespace blogger.Services
{

    public class BlogsService
    {
        private readonly BlogsRepository _repo;

        public BlogsService(BlogsRepository repo)
        {
            _repo = repo;
        }

        internal IEnumerable<Blog> GetBlogsByProfileId(string id)
        {
            return _repo.GetBlogsByProfileId(id);
        }

        internal IEnumerable<Blog> GetAll()
        {
            return _repo.GetAll();
        }

        internal Blog GetOneById(int id)
        {
            Blog current = _repo.GetOneById(id);
            if(current == null){
                throw new SystemException("INVALID ID");
            }else{
                return current;
            }
        }

        internal Blog CreateBlog(Blog newBlog)
        {
            return _repo.CreateBlog(newBlog);
        }

        internal Blog EditBlog(Blog editedBlog)
        {
            Blog current = GetOneById(editedBlog.Id);
            if(current == null){
                throw new SystemException("INVALID ID");
            }else{
                current.Body = editedBlog.Body != null ? editedBlog.Body : current.Body;
                current.ImgUrl = editedBlog.ImgUrl != null ? editedBlog.ImgUrl : current.ImgUrl;
                current.Published = editedBlog.Published != true ? editedBlog.Published : current.Published;
                current.Title = editedBlog.Title != null ? editedBlog.Title : current.Title;
                return _repo.EditBlog(current);
            }
        }

        internal object DeleteBlog(int id, string userInfoId)
        {
            GetOneById(id);
            _repo.DeleteBlog(id, userInfoId);
            string deleted = "Blog deleted";
            return deleted;
        }
    }
}