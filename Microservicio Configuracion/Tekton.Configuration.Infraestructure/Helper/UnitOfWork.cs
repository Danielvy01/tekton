using Tekton.Configuration.Damain.Helper.Repository;
using Tekton.Configuration.Infraestructure.Data;
using System.Data;

namespace Tekton.Configuration.Infraestructure.Helper;
public class UnitOfWork : IUnitOfWork
{
    protected readonly IDbConnection _connection;
    public UnitOfWork(IConnectionFactory connectionFactory)
    {
        var _connectionFactory = connectionFactory;

        Id = Guid.NewGuid();
        _connection = _connectionFactory.GetConnection;

        if (_connection.State == ConnectionState.Closed)
        {
            _connection.Open();
        }
    }
    public IDbTransaction Transaction { get; private set; }
    public IDbConnection Connection => _connection;
    public Guid Id { get; }

    public void Begin()
    {
        Transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        Transaction.Commit();
        Dispose();
    }

    public void Dispose()
    {
        if (Transaction != null)
            Transaction.Dispose();
        Transaction = null;
        _connection.Dispose();
    }

    public void Rollback()
    {
        Transaction.Rollback();
        Dispose();
    }
}
