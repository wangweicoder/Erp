using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CartLine
    {        
        public int Quantity { get; set; }
        public List<FlowerCartVM> Products { get; set; }
    }
}
