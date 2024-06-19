using System.Data;

namespace Tekton.Configuration.Damain.Helper.Repository;

public interface IUnitOfWork : IDisposable
{
    Guid Id { get; }
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    void Begin();
    void Commit();
    void Rollback();
}
