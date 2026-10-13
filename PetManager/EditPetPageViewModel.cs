using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetManager
{
    public partial class EditPetPageViewModel :  ObservableObject
    {
        [ObservableProperty]
        Pet originalPet;

        [ObservableProperty]
        Pet editedPet;

        public void InitDraft()
        {
            EditedPet = OriginalPet.GetCopy();
        }

        [RelayCommand]
        public async Task SavePet()
        {
            
        }

        [RelayCommand]
        public async Task CancelEdit()
        {

        }
    }
}
