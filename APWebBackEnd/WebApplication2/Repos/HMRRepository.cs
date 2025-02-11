using APWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace APWeb.Repos
{
    public class HMRRepository
    {
        private readonly ApplicationDbContext _context;

        public HMRRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HMRProduct> FindProduct(string productID)
        {
            var product = await _context.HMRProducts.FindAsync(productID);
            return product;
        }

        public async Task<double> GetProductQty(string productID)
        {
            var totalQty = await _context.HMRInventory
                                       .Where(item => item.ProductID == productID)
                                       .SumAsync(item => item.UnitQty);
            return totalQty;
        }
    }
}
