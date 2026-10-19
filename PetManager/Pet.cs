using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetManager
{
    public partial class Pet : ObservableObject
    {
        [ObservableProperty]
        [property:PrimaryKey]
        [property:AutoIncrement]
        int id;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string species;

        [ObservableProperty]
        DateTime birthDate;

        public Pet GetCopy()
        {
            return (Pet)this.MemberwiseClone();
        }
    }
}
