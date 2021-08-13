using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace VideoPlayerStateMachine.Controls
{
    public class VideoPlayer : MediaElement
    {
        public static readonly BindableProperty CommandProperty =
              BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(VideoPlayer), defaultBindingMode: BindingMode.OneWayToSource);

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public VideoPlayer()
        {
            ShowsPlaybackControls = false;

            Command = new Command<VideoTrigger>(ExecuteTrigger); 
        }

        private void ExecuteTrigger(VideoTrigger trigger)
        {
            switch (trigger)
            {
                case VideoTrigger.Play:
                    Play();
                    break;
                case VideoTrigger.Pause:
                    Pause();
                    break;
                case VideoTrigger.Stop:
                    Stop();
                    break;
            }
        }
    }
}
