﻿namespace ApiEasier.Bll.Dto
{
    /// <summary>
    /// DTO для представления сущности без эндпоинтов
    /// </summary>
    public class ApiEntitySummaryDto
    {
        /// <summary>
        /// Имя сущности API.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Указывает, активна ли сущность API.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Структура сущности API.
        /// </summary>
        public object? Structure { get; set; } = null;
    }
}
