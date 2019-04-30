using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Data.Access.Maps.Common
{
    public interface IMap
    {
        void Visit(ModelBuilder builder);
    }
}