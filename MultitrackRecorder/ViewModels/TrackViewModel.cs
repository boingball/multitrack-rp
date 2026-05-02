using System.Collections.ObjectModel;
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

    public TrackViewModel(int index)
    {
        Index = index;
        Name = $"Track {index + 1}";
        InputDevices = new ObservableCollection<AudioDevice>();
        OutputDevices = new ObservableCollection<AudioDevice>();
    }

    public int Index { get; }
    public string Name { get; }
    public ObservableCollection<AudioDevice> InputDevices { get; }
    public ObservableCollection<AudioDevice> OutputDevices { get; }

    public bool PlayEnabled { get => _playEnabled; set { _playEnabled = value; OnPropertyChanged(); } }
    public bool RecordEnabled { get => _recordEnabled; set { _recordEnabled = value; OnPropertyChanged(); } }
    public float Volume { get => _volume; set { _volume = value; OnPropertyChanged(); } }
    public bool Mute { get => _mute; set { _mute = value; OnPropertyChanged(); } }
    public int Bass { get => _bass; set { _bass = value; OnPropertyChanged(); } }
    public int Treble { get => _treble; set { _treble = value; OnPropertyChanged(); } }
    public TrackChannelMode ChannelMode { get => _channelMode; set { _channelMode = value; OnPropertyChanged(); } }
    public AudioDevice? SelectedInput { get => _selectedInput; set { _selectedInput = value; OnPropertyChanged(); } }
    public AudioDevice? SelectedOutput { get => _selectedOutput; set { _selectedOutput = value; OnPropertyChanged(); } }
}
