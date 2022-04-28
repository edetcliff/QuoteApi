using Microsoft.EntityFrameworkCore;
using QuoteApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteApi.Data
{
    public class QuoteApiDbContext : DbContext
    {
        public QuoteApiDbContext(DbContextOptions<QuoteApiDbContext> options) : base(options)
        {

        }
        public DbSet<Quote> Quotes { get; set; }
    }
}
