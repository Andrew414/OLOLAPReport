using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFReport.DataReport
{
    public interface Restriction
    {
        string ToXML();
        WFReport.Metadata.Table GetTable();
        WFReport.Metadata.Column GetColumn();
    }
}
