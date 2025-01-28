using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEasier.Domain.Interfaces
{
    public interface IFileRepository
    {
        string GetFilePath<T>();
        Task<T> ReadFromFileAsync<T>();
        Task WriteToFileAsync<T>(T data);
    }
}
