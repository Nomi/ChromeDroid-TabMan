using ChromeDroid_TabMan.Auxiliary;
using ChromeDroid_TabMan.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromeDroid_TabMan.Data
{
    internal class TabsDbContext: DbContext
    {
        public DbSet<TabInf> Tabs { get; set; }

        public string DbPath { get; }

        public TabsDbContext(string dbPath)
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            //DbPath = System.IO.Path.Join(path, "tabs.db");

            DbPath = dbPath;
        }

        // The following configures EF to create a Sqlite database file.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
