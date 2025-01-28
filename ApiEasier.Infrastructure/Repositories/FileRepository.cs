using ApiEasier.Domain.Interfaces;
using ApiEasier.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiEasier.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly string _dbFolderPath;

        public FileRepository(string dbFolderPath)
        {
            _dbFolderPath = dbFolderPath;
        }

        public string GetFilePath<T>()
        {
            var fileName = typeof(T).Name + ".json";
            return Path.Combine(_dbFolderPath, fileName);
        }

        public async Task<T?> ReadFromFileAsync<T>()
        {
            var filePath = GetFilePath<T>();

            if (!File.Exists(filePath))
                return default;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }

        public async Task WriteToFileAsync<T>(T data)
        {
            var filePath = GetFilePath<T>();
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
