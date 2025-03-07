using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {

    }
}
