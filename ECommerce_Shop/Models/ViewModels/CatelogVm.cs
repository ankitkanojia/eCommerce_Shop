using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce_Shop.Models.ViewModels
{
    public class CatelogVm
    {
        public long ProductId { get; set; }
        public string CategoryName { get; set; }
        public long CategoryId { get; set; }
        public List<ColorVm> Colors { get; set; }
        public List<SizeVm> Sizes { get; set; }
        public List<FilterDataVm> FilterDataVms { get; set; }
    }
}