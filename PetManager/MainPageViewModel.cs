using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetManager
{
    public partial class MainPageViewModel : ObservableObject
    {
        public ObservableCollection<Pet> Pets { get; set; }

        private Pet selectedPet;
        public Pet SelectedPet
        {
            get { return selectedPet; }
            set
            {
                selectedPet = value;
                OnPropertyChanged();
                DeletePetCommand.NotifyCanExecuteChanged();
                EditSelectedPetCommand.NotifyCanExecuteChanged();
            }
        }

        public bool CanEditOrDelete => SelectedPet != null;

        [ObservableProperty]
        Pet editedPet;

        private bool isEditingOn;
        public bool IsEditingOn
        {
            get => isEditingOn;
            set
            {
                if (isEditingOn != value)
                {
                    isEditingOn = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsListingOn));
                }
            }
        }

        public bool IsListingOn
        {
            get => !isEditingOn;
        }

        public MainPageViewModel()
        {
            Pets = new ObservableCollection<Pet>();
            Pets.Add(new Pet() { Name = "Samu", Species = "Cat", BirthDate = new DateTime(2021, 9, 28) });
            Pets.Add(new Pet() { Name = "Zoé", Species = "Francia bulldog palota pincsi", BirthDate = new DateTime(2014, 12, 14) });
        }

        [RelayCommand]
        public async Task ShowPetDetailsAsync()
        {
            if (SelectedPet != null)
            {
                var param = new ShellNavigationQueryParameters
            {
                { "Potato", SelectedPet }
            };
                await Shell.Current.GoToAsync("petdetails", param);
            }
        }

        [RelayCommand(CanExecute = nameof(CanEditOrDelete))]
        public void EditSelectedPet()
        {
            if (SelectedPet != null)
            {
                EditedPet = SelectedPet.GetCopy();
                IsEditingOn = true;
            }
            else
            {
                WeakReferenceMessenger.Default
                    .Send("Select a pet to edit.");
            }
        }

        [RelayCommand]
        public void SavePet()
        {
            if (SelectedPet != null)
            {
                Pets.Remove(SelectedPet);
            }
            Pets.Add(EditedPet);
            EditedPet = null;
            IsEditingOn = false;
        }

        [RelayCommand]
        public void CancelEdit()
        {
            EditedPet = null;
            IsEditingOn = false;
        }


        [RelayCommand(CanExecute = nameof(CanEditOrDelete))]
        public void DeletePet()
        {
            if (SelectedPet != null)
            {
                Pets.Remove(SelectedPet);
                SelectedPet = null;
            }
            else
            {
                WeakReferenceMessenger.Default
                    .Send("Select a pet a delete.");
            }
        }

        [RelayCommand]
        public void NewPet()
        {
            SelectedPet = null;
            EditedPet = new Pet()
            {
                Name = "New Name",
                BirthDate = DateTime.Today
            };
            IsEditingOn = true;
        }
    }
}
