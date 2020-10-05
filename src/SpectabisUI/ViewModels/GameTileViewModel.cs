using System.Runtime.CompilerServices;
using System;
using Avalonia.Media.Imaging;
using ReactiveUI;
using SpectabisLib.Models;

namespace SpectabisUI.ViewModels
{
    public class GameTileViewModel : ReactiveObject
    {
        public GameTileViewModel(GameProfile profile)
        {
            _title = profile.Title;
            _playtime = profile.Playtime;
        }

        private string _title;
        private TimeSpan _playtime;
        private Bitmap _boxart;
        private bool _showActiveEffect;

        public string Title { get => _title; set => this.RaiseAndSetIfChanged(ref _title, value); }
        public TimeSpan Playtime { get => _playtime; set => this.RaiseAndSetIfChanged(ref _playtime, value); }
        public Bitmap Boxart { get => _boxart; set => this.RaiseAndSetIfChanged(ref _boxart, value); }
        public bool ShowActiveEffect { get => _showActiveEffect; set => this.RaiseAndSetIfChanged(ref _showActiveEffect, value); }
    }
}