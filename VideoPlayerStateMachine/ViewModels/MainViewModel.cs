using System.ComponentModel;
using System.Windows.Input;
using Stateless;
using Xamarin.Forms;

namespace VideoPlayerStateMachine.ViewModels
{

    public class MainViewModel : INotifyPropertyChanged
    {
        public string Status { get; private set; }

        public bool CanStop { get; private set; }

        public bool CanPause { get; private set; }

        public bool CanPlay { get; private set; }

        public bool CanAutoPlay { get; set; }

        public ICommand TriggerCommand { get; }

        public ICommand VideoActionCommand { get; set; }

        public MainViewModel()
        {
            _videoPlayerStateMachine = new StateMachine<VideoState, VideoTrigger>(CanAutoPlay ? VideoState.Playing: VideoState.Idle);

            _videoPlayerStateMachine.Configure(VideoState.Idle)
                   .OnActivate(OnStateEntry)
                   .OnEntry(OnStateEntry)
                   .Permit(VideoTrigger.Play, VideoState.Playing);

            _videoPlayerStateMachine.Configure(VideoState.Playing)
                   .OnActivate(OnStateEntry)
                   .OnEntry(OnStateEntry)
                   .Permit(VideoTrigger.Pause, VideoState.Paused)
                   .Permit(VideoTrigger.Stop, VideoState.Stopped);

            _videoPlayerStateMachine.Configure(VideoState.Paused)
                   .OnEntry(OnStateEntry)
                   .Permit(VideoTrigger.Play, VideoState.Playing)
                   .Permit(VideoTrigger.Stop, VideoState.Stopped);

            _videoPlayerStateMachine.Configure(VideoState.Stopped)
                   .OnEntry(OnStateEntry)
                   .Permit(VideoTrigger.Play, VideoState.Playing);

            TriggerCommand = new Command<VideoTrigger>(OnTrigger);

            _videoPlayerStateMachine.Activate();
        }

        private void OnStateEntry()
        {
            Status = $"{_videoPlayerStateMachine.State}";
            CanPlay = _videoPlayerStateMachine.CanFire(VideoTrigger.Play);
            CanPause = _videoPlayerStateMachine.CanFire(VideoTrigger.Pause);
            CanStop = _videoPlayerStateMachine.CanFire(VideoTrigger.Stop);
        }

        private void OnTrigger(VideoTrigger videoTrigger)
        {
            if (_videoPlayerStateMachine.CanFire(videoTrigger))
            {
                VideoActionCommand?.Execute(videoTrigger);

                _videoPlayerStateMachine.Fire(videoTrigger);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private StateMachine<VideoState, VideoTrigger> _videoPlayerStateMachine;
    }
}