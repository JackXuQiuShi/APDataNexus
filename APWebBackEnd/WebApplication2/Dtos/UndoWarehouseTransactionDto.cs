using System;

namespace APWeb.Dtos
{
    public class UndoWarehouseTransactionDto
    {
        public DateTime Date { get; set; }
        public string ProductID { get; set; }
        public string Action { get; set; }
        public int StoreID { get; set; }
        public string POID { get; set; }
        public int SellTo { get; set; }
    }
}
