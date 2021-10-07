using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using onboarding.dal.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace onboarding.dal.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly DwikiDbContext dbContext;

        public IBaseRepository<MovieModel> MovieRepository { get; set; }

        public IBaseRepository<National> NationalRepository { get; set; }

        public UnitOfWork(DwikiDbContext context)
        {
            dbContext = context;

            MovieRepository = new BaseRepository<MovieModel>(context);

            NationalRepository = new BaseRepository<National>(context);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction StartNewTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> StartNewTransactionAsync()
        {
            return dbContext.Database.BeginTransactionAsync();
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dbContext?.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
