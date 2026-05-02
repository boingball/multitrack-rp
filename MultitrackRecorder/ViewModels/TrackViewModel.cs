using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Win32;
using MultitrackRecorder.Models;

namespace MultitrackRecorder.ViewModels;

public sealed class TrackViewModel : ViewModelBase
{
    private bool _playEnabled;
    private bool _recordEnabled;
    private float _volume = 0.8f;
    private bool _mute;
    private int _bass;
    private int _treble;
    private TrackChannelMode _channelMode = TrackChannelMode.Mono;
    private AudioDevice? _selectedInput;
    private AudioDevice? _selectedOutput;
    private string? _tapeFilePath;
    private string _tapeDisplayName = "No tape loaded";

    public TrackViewModel(int index)
    {
        Index = index;
        Name = $"Track {index + 1}";
        InputDevices = new ObservableCollection<AudioDevice>();
        OutputDevices = new ObservableCollection<AudioDevice>();
        LoadTapeCommand = new RelayCommand(LoadTapeFile);
        EjectTapeCommand = new RelayCommand(EjectTape, () => IsTapeLoaded);
    }

    public int Index { get; }
    public string Name { get; }
    public ObservableCollection<AudioDevice> InputDevices { get; }
    public ObservableCollection<AudioDevice> OutputDevices { get; }
    public RelayCommand LoadTapeCommand { get; }
    public RelayCommand EjectTapeCommand { get; }

    public bool PlayEnabled { get => _playEnabled; set { _playEnabled = value; OnPropertyChanged(); } }
    public bool RecordEnabled { get => _recordEnabled; set { _recordEnabled = value; OnPropertyChanged(); } }
    public float Volume { get => _volume; set { _volume = value; OnPropertyChanged(); } }
    public bool Mute { get => _mute; set { _mute = value; OnPropertyChanged(); } }
    public int Bass { get => _bass; set { _bass = value; OnPropertyChanged(); } }
    public int Treble { get => _treble; set { _treble = value; OnPropertyChanged(); } }
    public TrackChannelMode ChannelMode { get => _channelMode; set { _channelMode = value; OnPropertyChanged(); } }
    public AudioDevice? SelectedInput { get => _selectedInput; set { _selectedInput = value; OnPropertyChanged(); } }
    public AudioDevice? SelectedOutput { get => _selectedOutput; set { _selectedOutput = value; OnPropertyChanged(); } }

    public string? TapeFilePath
    {
        get => _tapeFilePath;
        private set
        {
            _tapeFilePath = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsTapeLoaded));
            OnPropertyChanged(nameof(IsInputSelectable));
        }
    }

    public string TapeDisplayName
    {
        get => _tapeDisplayName;
        private set
        {
            _tapeDisplayName = value;
            OnPropertyChanged();
        }
    }

    public bool IsTapeLoaded => !string.IsNullOrWhiteSpace(TapeFilePath);
    public bool IsInputSelectable => !IsTapeLoaded;

    private void LoadTapeFile()
    {
        var fileDialog = new OpenFileDialog
        {
            Title = $"Load cassette for {Name}",
            Filter = "Audio files (*.mp3;*.wav)|*.mp3;*.wav|MP3 files (*.mp3)|*.mp3|WAV files (*.wav)|*.wav",
            CheckFileExists = true,
            Multiselect = false
        };

        if (fileDialog.ShowDialog() != true)
        {
            return;
        }

        TapeFilePath = fileDialog.FileName;
        TapeDisplayName = Path.GetFileName(fileDialog.FileName);
        EjectTapeCommand.RaiseCanExecuteChanged();
    }

    private void EjectTape()
    {
        TapeFilePath = null;
        TapeDisplayName = "No tape loaded";
        EjectTapeCommand.RaiseCanExecuteChanged();
    }
}
