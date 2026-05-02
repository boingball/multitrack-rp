using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MultitrackRecorder.Models;
using NAudio.Wave;

namespace MultitrackRecorder.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private bool _isRecordingToTape;

    public ObservableCollection<TrackViewModel> Tracks { get; } = new();

    public RelayCommand PlayAllCommand { get; }
    public RelayCommand RecordAllCommand { get; }
    public RelayCommand MuteAllCommand { get; }

    public MainWindowViewModel()
    {
        for (var i = 0; i < 8; i++)
        {
            var track = new TrackViewModel(i);
            track.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(TrackViewModel.RecordEnabled))
                {
                    UpdateRecordingState();
                }
            };

            Tracks.Add(track);
        }

        LoadDevices();

        PlayAllCommand = new RelayCommand(() => SetAll(track => track.PlayEnabled = !track.PlayEnabled));
        RecordAllCommand = new RelayCommand(() => SetAll(track => track.RecordEnabled = !track.RecordEnabled));
        MuteAllCommand = new RelayCommand(() => SetAll(track => track.Mute = !track.Mute));
        UpdateRecordingState();
    }

    public bool IsRecordingToTape
    {
        get => _isRecordingToTape;
        private set
        {
            _isRecordingToTape = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(RecordingTapeStatus));
        }
    }

    public string RecordingTapeStatus => IsRecordingToTape
        ? "● Recording mixdown tape (all armed inputs)"
        : "○ Mixdown tape idle";

    private void SetAll(Action<TrackViewModel> update)
    {
        foreach (var track in Tracks)
        {
            update(track);
        }
    }

    private void LoadDevices()
    {
        var inputs = Enumerable.Range(0, WaveInEvent.DeviceCount)
            .Select(i => WaveInEvent.GetCapabilities(i))
            .Select((caps, idx) => new AudioDevice(idx, caps.ProductName))
            .ToList();

        var outputs = Enumerable.Range(0, WaveOut.DeviceCount)
            .Select(i => WaveOut.GetCapabilities(i))
            .Select((caps, idx) => new AudioDevice(idx, caps.ProductName))
            .ToList();

        foreach (var track in Tracks)
        {
            foreach (var input in inputs)
            {
                track.InputDevices.Add(input);
            }

            foreach (var output in outputs)
            {
                track.OutputDevices.Add(output);
            }
        }

        if (inputs.Count == 0 || outputs.Count == 0)
        {
            MessageBox.Show("No audio devices found. Connect an audio interface and restart.", "Audio Devices", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void UpdateRecordingState()
    {
        IsRecordingToTape = Tracks.Any(track => track.RecordEnabled);
    }
}
