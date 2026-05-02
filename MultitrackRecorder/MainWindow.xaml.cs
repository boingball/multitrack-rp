using System.Windows;
using System.Windows.Controls;
using MultitrackRecorder.Models;
using MultitrackRecorder.ViewModels;

namespace MultitrackRecorder;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        Closed += (_, _) => (DataContext as MainWindowViewModel)?.Dispose();
    }

    private void ChannelMode_Checked(object sender, RoutedEventArgs e)
    {
        if (sender is not RadioButton radio || radio.DataContext is not TrackViewModel track)
        {
            return;
        }

        if (radio.Tag is TrackChannelMode mode)
        {
            track.ChannelMode = mode;
        }
    }
}
