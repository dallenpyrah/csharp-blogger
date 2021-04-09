using System;
using blogger.Models;
using blogger.Repository;

namespace blogger.Services
{
  public class ProfilesService
  {
    private readonly ProfilesRepository _repo;
    public ProfilesService(ProfilesRepository repo)
    {
      _repo = repo;
    }
    internal Profile GetOrCreateProfile(Profile userInfo)
    {
      Profile profile = _repo.GetById(userInfo.Id);
      if (profile == null)
      {
        return _repo.Create(userInfo);
      }
      return profile;
    }

    internal Profile GetProfileById(string id)
    {
      Profile profile = _repo.GetById(id);
      if (profile == null)
      {
        throw new Exception("Invalid Id");
      }
      return profile;
    }

        internal Profile EditAccount(Profile newProfile)
        {
            Profile account = GetProfileById(newProfile.Id);
            if(account == null){
              throw new SystemException("INVALID ID");
            }else{
              account.Name = newProfile.Name != null ? newProfile.Name : account.Name;
              account.Picture = newProfile.Picture != null ? newProfile.Picture : account.Picture;
              return _repo.Edit(account);
            }
        }
    }
}