using System.Collections.Generic;
using Audit.EntityFramework;
using Domain.Common;

namespace Infrastructure.Persistence.Extensions
{
    public static class AuditExtensions
    {
        public static List<AuditEntryChange> AsAuditChanges(this List<EventEntryChange> changes)
        {
            var auditChages = new List<AuditEntryChange>();
            if (changes == null) return auditChages;

            changes.ForEach(change => {
                if (change.ColumnName != null || change.OriginalValue != null)
                {
                    auditChages.Add(new AuditEntryChange
                    {
                        ColumnName = change?.ColumnName,
                        OriginalValue = change?.OriginalValue?.ToString(),
                        NewValue = change?.NewValue?.ToString()
                    });
                }
            });

            return auditChages;
        }
    }
}
