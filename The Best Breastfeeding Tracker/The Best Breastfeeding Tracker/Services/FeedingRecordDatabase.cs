using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using The_Best_Breastfeeding_Tracker.Models;
using SQLite;

namespace The_Best_Breastfeeding_Tracker.Services
{
    class FeedingRecordDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<FeedingRecordDatabase> Instance = new AsyncLazy<FeedingRecordDatabase>(async () =>
        {
            var instance = new FeedingRecordDatabase();
            CreateTableResult result = await Database.CreateTableAsync<FeedingRecord>();
            return instance;
        });

        public FeedingRecordDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }
        public Task<List<FeedingRecord>> GetItemsAsync()
        {
            return Database.Table<FeedingRecord>().ToListAsync();
        }

        public Task<List<FeedingRecord>> GetItemsDAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<FeedingRecord>("SELECT * FROM [FeedingRecord] ORDER BY Date DESC");
        }

        public Task<FeedingRecord> GetItemAsync(int id)
        {
            return Database.Table<FeedingRecord>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(FeedingRecord item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(FeedingRecord item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
