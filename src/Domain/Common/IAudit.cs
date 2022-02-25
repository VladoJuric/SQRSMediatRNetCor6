namespace Domain.Common
{
    public interface IAudit
    {
        public int AuditId { get; set; }
        public string UserName { get; set; }
        public string AuditAction { get; set; }
        public DateTime AuditDate { get; set; }
        public string IpAddress { get; set; }
        public List<AuditEntryChange> Changes { get; set; }
    }
}
