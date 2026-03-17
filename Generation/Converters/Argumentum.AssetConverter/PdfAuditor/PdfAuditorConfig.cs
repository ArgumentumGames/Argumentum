using System.Collections.Generic;
using System.Threading.Tasks;

namespace Argumentum.AssetConverter.PdfAuditor
{
    public class PdfAuditorConfig
    {
        public bool IsEnabled { get; set; } = true;

        public List<PdfAuditItem> Audits { get; set; } = new List<PdfAuditItem>();

        public async Task Apply(AssetConverterConfig masterConfig)
        {
            if (!IsEnabled)
            {
                Logger.Log("PDF Auditor is disabled. Skipping.");
                return;
            }

            //var auditor = new PdfAuditor();
            //foreach (var auditItem in Audits)
            //{
            //    await auditor.AuditPdf(auditItem, masterConfig);
            //}
        }
    }
}