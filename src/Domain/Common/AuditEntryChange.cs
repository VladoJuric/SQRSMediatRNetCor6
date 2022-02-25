namespace Domain.Common
{
    public class AuditEntryChange
    {
        public string ColumnName { get; set; } = "";
        public string OriginalValue { get; set; } = "";
        public string NewValue { get; set; } = "";
    }
}
