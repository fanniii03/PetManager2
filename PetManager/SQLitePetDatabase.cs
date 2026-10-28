using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetManager
{
    internal class SQLitePetDatabase : IPetDatabase
    {
        //Nuget: sqlite-net-pcl
        SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create;

        string databasePath =
            Path.Combine(FileSystem.Current.AppDataDirectory, "pets.db3");

        SQLiteAsyncConnection database;

        public SQLitePetDatabase()
        {
            database = new SQLiteAsyncConnection(databasePath, Flags);
            database.CreateTableAsync<Pet>().Wait();
        }

        public async Task<List<Pet>> GetPetsAsync()
        {
            return await database.Table<Pet>().ToListAsync();
        }

        public async Task<Pet> GetPetAsync(int id)
        {
            return await database.Table<Pet>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreatePetAsync(Pet pet)
        {
            await database.InsertAsync(pet);
        }

        public async Task UpdatePetAsync(Pet pet)
        {
            await database.UpdateAsync(pet);
        }

        public async Task DeletePetAsync(Pet pet)
        {
            await database.DeleteAsync(pet);
        }
    }
}
