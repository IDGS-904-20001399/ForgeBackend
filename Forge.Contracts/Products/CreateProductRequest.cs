using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Contracts.Products
{
    public record CreateProductRequest(
        string Name,
        string Description,
        string Category,
        double Price,
        string Image
    );

}
