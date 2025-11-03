using Microsoft.EntityFrameworkCore;
using MyInsurancePortal.Models;

namespace MyInsurancePortalTest.TestHelper
{
    public static class DbContextHelper
    {
        public static InsuranceDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<InsuranceDbContext>() //This is the EF Core builder used to configure options for your database context. normally we configure with connection string
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // method is a unit test helper that creates a temporary, in-memory version of your InsuranceDbContext
                .Options; //This simply builds and returns the configured options for the DbContext.

            return new InsuranceDbContext(options); // pass the options as parameter and return a new instance.
        }
    }
}

//The Guid.NewGuid().ToString() part gives the database a unique name each time you call this method. GUID : Global Unique Identifiers.


