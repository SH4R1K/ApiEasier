namespace ApiEasier.Server.Models
{
    public class RequestLog // В JSON Формате хранить логи
    {
        public DateTime RequestDate { get; set; }
        public string? RawJson { get; set; }
        public string RawResponseCode { get; set; }
        public string RawResponseBody { get; set; }
        public string EntityName { get; set; }
        public string ActionName{ get; set; }
        public string ActionType { get; set; }
    }
}
