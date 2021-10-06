using Microsoft.EntityFrameworkCore.Storage;
using onboarding.dal.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace onboarding.dal.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<MovieModel> MovieRepository { get; set; }
        IBaseRepository<National> NationalRepository { get; set; }
        void Save();
        Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
        IDbContextTransaction StartNewTransaction();
        Task<IDbContextTransaction> StartNewTransactionAsync();
        Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}
