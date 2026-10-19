using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetManager
{
    public interface IPetDatabase
    {
        Task<List<Pet>> GetPetsAsync();
        Task<Pet> GetPetAsync(int id);
        Task CreatePetAsync(Pet pet);
        Task UpdatePetAsync(Pet pet);
        Task DeletePetAsync(Pet pet);
    }
}
