using DataAccess.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IDapperDataAccess
    {
        public Task<IEnumerable<MyTodoItem>> GetTodoItems(CancellationToken cancellationToken);
    }
}