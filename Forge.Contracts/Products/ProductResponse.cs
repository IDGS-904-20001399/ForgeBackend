using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forge.Contracts.Products
{
    public record ProductResponse(
        Guid id,
        string Name,
        string Description,
        string Category,
        double Price);
}
