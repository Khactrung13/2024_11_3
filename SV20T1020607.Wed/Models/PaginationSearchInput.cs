using System;
namespace SV20T1020607.Wed.Models
{
	public class PaginationSearchInput
	{
		public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public string SearchValue { get; set; } = "";
    }

    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public decimal minPrice { get; set; } = 0;
        public decimal maxPrice { get; set; } = 0;

    }
}

