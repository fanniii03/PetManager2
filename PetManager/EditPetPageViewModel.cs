using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetManager
{
    [QueryProperty(nameof(OriginalPet), "Pet")]
    public partial class EditPetPageViewModel : ObservableObject
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
            var param = new ShellNavigationQueryParameters
            {
                { "EditedPet", EditedPet }
            };
            await Shell.Current.GoToAsync("..", param);
        }

        [RelayCommand]
        public async Task CancelEdit()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
