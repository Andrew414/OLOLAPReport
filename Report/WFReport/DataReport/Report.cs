using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFReport.Metadata;

namespace WFReport.DataReport
{
    public class Report
    {
        public Table rTable = null;
        public Table cTable = null;
        public Table aTable = null;

        public Column rColumn = null;
        public Column cColumn = null;
        public Column aColumn = null;

        public String rGroup = null;
        public String cGroup = null;

        public static String SQLTEMPLATEREPORT = 
@"SELECT 
	<COLUMNS>,
	<ROWS>,
	count(<AGGREGATE>)
FROM
	Operation
	INNER JOIN Buyer ON Buyer.Id = Operation.BuyerId
	INNER JOIN Store ON Store.Id = Operation.StoreId
	INNER JOIN Item  ON Item.Id  = Operation.ItemId

<WHERESTATEMENT>

GROUP BY
	<COLUMNS>,
	<ROWS>;";

        public List<Restriction> restrictions = new List<Restriction>(); 
    }
}
