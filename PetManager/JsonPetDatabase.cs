using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManager
{
    internal class JsonPetDatabase : IPetDatabase
    {
        string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "pets.json");

        public async Task<List<Pet>> GetPetsAsync()
        {
            if (File.Exists(filePath))
            {
                string jsonString = await File.ReadAllTextAsync(filePath);
                if (!string.IsNullOrEmpty(jsonString))
                {
                    return JsonSerializer.Deserialize<List<Pet>>(jsonString);
                }
                else
                {
                    return new List<Pet>();
                }
            }
            else return new List<Pet>();
        }

        public async Task<Pet> GetPetAsync(int id)
        {
            var pets = await GetPetsAsync();
            return pets.FirstOrDefault(p => p.Id == id);
        }

        public async Task CreatePetAsync(Pet pet)
        {
            var pets = await GetPetsAsync();
            pet.Id = pets.Count == 0 ? 1 : (pets.Max(p => p.Id) + 1);
            pets.Add(pet);
            await WritePetsAsync(pets);
        }

        public async Task UpdatePetAsync(Pet pet)
        {
            var pets = await GetPetsAsync();
            var index = pets.FindIndex(p => p.Id == pet.Id);
            if (index != -1)
            {
                pets[index] = pet;
                await WritePetsAsync(pets);
            }
        }

        public async Task DeletePetAsync(Pet pet)
        {
            var pets = await GetPetsAsync();
            pets.RemoveAll(p => p.Id == pet.Id);
            await WritePetsAsync(pets);
        }

        private async Task WritePetsAsync(List<Pet> pets)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(pets);
                await File.WriteAllTextAsync(filePath, jsonString);
            }
            catch (Exception e)
            {
                WeakReferenceMessenger.Default.Send("File write error: " + e.Message);
            }
        }
    }
}
