﻿using Newtonsoft.Json;
using SFUListParser.Model;
using SFUListParser.Scripts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SFUListParser.ViewModel
{
    internal class MainPageViewModel
    {
        private static MainPageViewModel instance;

        public ObservableCollection<CompetitionListData> CompetitionLists { get; set; }

        private SaveDataHandler saveDataHandler;

        public static MainPageViewModel Init()
        {
            if (instance == null)
                instance = new MainPageViewModel();

            return instance;
        }

        private MainPageViewModel()
        {
            saveDataHandler = SaveDataHandler.Init();
            LoadCompetitionLists();

            CompetitionLists.CollectionChanged += (sender, e) =>
                saveDataHandler.SaveData(JsonConvert.SerializeObject(CompetitionLists, Formatting.Indented));
        }

        private void LoadCompetitionLists() =>
            CompetitionLists = JsonConvert.DeserializeObject<ObservableCollection<CompetitionListData>>(saveDataHandler.LoadData());

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
