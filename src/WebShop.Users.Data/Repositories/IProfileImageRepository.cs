using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GetBee.Users.Data.Repositories
{
    public interface IProfileImageRepository
    {
        Task SetImage(Guid userId, byte[] image);
        Task RemoveImage(Guid userId);
        Task<byte[]> GetImage(Guid userId);
    }
}
