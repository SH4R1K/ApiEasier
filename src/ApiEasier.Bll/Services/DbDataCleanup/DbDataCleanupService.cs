using ApiEasier.Bll.Interfaces.DbDataCleanup;
using ApiEasier.Dal.Interfaces.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Bll.Services.DbDataCleanup
{
    public class DbDataCleanupService : IDbDataCleanupService
    {
        private readonly IDbResourceRepository _dbResourceRepository;
        

        public DbDataCleanupService(IDbResourceRepository dbResourceRepository)
        {
            _dbResourceRepository = dbResourceRepository;
        }

        public async Task CleanupAsync()
        {
            //await _dbResourceRepository.
        }

    }
}
