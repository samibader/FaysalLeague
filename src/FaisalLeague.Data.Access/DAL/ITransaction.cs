using System;

namespace FaisalLeague.Data.Access.DAL
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}