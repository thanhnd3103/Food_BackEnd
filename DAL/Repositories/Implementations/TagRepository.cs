using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories.Implementations
{
    public class TagRepository(MyDBContext context) : GenericRepository<Tag>(context), ITagRepository
    {
    }
}
