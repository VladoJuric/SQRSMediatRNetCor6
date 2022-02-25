using Domain.Common;

namespace Domain.Entities.Audit
{
    public class UserAudit : IAudit
    {
        public int AuditId { get; set; }
        public string UserName { get; set; } = "";
        public string AuditAction { get; set; } = "";
        public DateTime AuditDate { get; set; }
        public string IpAddress { get; set; } = "";
        public List<AuditEntryChange> Changes { get; set; } = new List<AuditEntryChange>();

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
    }
}
