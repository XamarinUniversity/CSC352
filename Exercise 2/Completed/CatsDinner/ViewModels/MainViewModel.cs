using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using CatsDinner.Models;
using XamarinUniversity.Infrastructure;

namespace CatsDinner.ViewModels
{
    public class MainViewModel : SimpleViewModel
    {
        readonly DiningRoom diningRoom = new DiningRoom();
        bool isRunning;
        int totalFights;
        string fightDetails;

        public Command Start { get; private set; }
        public IList<CatViewModel> Cats { get; private set; }

        public string FightDetails {
            get { return fightDetails; }
            private set { SetPropertyValue(ref fightDetails, value); }
        }

        public MainViewModel()
        {
            Start = new Command(StartSimulation, () => !isRunning);
            Cats = diningRoom.Cats.Select(p => new CatViewModel(p)).ToList();

            diningRoom.CatFightDetected += (currentOwner, newOwner, sensor) => {
                FightDetails = string.Format("{0}: {1} vs. {2} over sensor {3}", 
                    ++totalFights, currentOwner.Name, newOwner.Name, sensor.Id);
            };
        }

        void StartSimulation()
        {
            isRunning = true;
            Start.ChangeCanExecute();

            Device.StartTimer(TimeSpan.FromSeconds(0.5), () => {
                foreach (var c in Cats)
                    c.Update();
                return true;
            });

            diningRoom.StartDinnerParty();
        }
    }
}
