using System.Collections.Generic;

namespace Argumentum.AssetConverter.PdfAuditor
{
    public class AuditResult
    {
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
    }
}