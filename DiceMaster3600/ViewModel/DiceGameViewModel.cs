using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Devices.RealSenseCamera;
using DiceMaster3600.Model;
using DiceMaster3600.Model.Services;
using DiceMaster3600.Model.Yahtzee;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DiceMaster3600.ViewModel
{
    public class DiceGameViewModel : ObservableObject, IMenuControlViewModel
    {

        #region Fields
        private readonly IYahtzeeScoreCounter scoreManager;
        private readonly IRealSenseCamera camera;
        private readonly IProcessProvider processProvider;
        private int diceCounter = 5;

        private BitmapSource? imageSource;
        private int diceInGameCounter;
        private int diceRollsCounter;
        private bool correctCountDetected;
        private bool isConnecting;

        private EventHandler<BitmapSource>? frameReceivedHandler;
        private string diceRollsNumber = string.Empty;
        private string diceInGameNumber = string.Empty;
        #endregion

        #region Properties
        public bool IsConnecting
        {
            get => isConnecting;
            set => SetProperty(ref isConnecting, value);
        }

        public string DiceRollsNumber
        {
            get => diceRollsNumber;
            set => SetProperty(ref diceRollsNumber, value);
        }

        public string DiceInGameNumber
        {
            get => diceInGameNumber;
            set => SetProperty(ref diceInGameNumber, value);
        }

        public BitmapSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        public bool CorrectCountDetected
        {
            get => correctCountDetected;
            set => SetProperty(ref correctCountDetected, value);
        }

        public ObservableCollection<NotificationModel> Notifications { get; private set; } = new();
        public ObservableCollection<DiceModel> Dices { get; private set; } = new();
        public MenuControlType ControlType => MenuControlType.GamePanel;

        public ICommand? RollCommand { get; private set; }
        public ICommand? ResetCommand { get; private set; }

        public ICommand? ClearNotificationsCommand { get; private set; }
        public ICommand? ClearErrorCommand { get; private set; }
        public ICommand? ClearWarningCommand { get; private set; }
        public ICommand StartStreamCommand { get; private set; }

        #endregion

        #region Constructors
        public DiceGameViewModel(IYahtzeeScoreCounter scoreManager, IRealSenseCamera camera, IProcessProvider processProvider)
        {

            this.camera = camera;
            this.processProvider = processProvider;
            this.scoreManager = scoreManager;
            ResetGame();

            RollCommand = new RelayCommand(Roll);
            ResetCommand = new RelayCommand(ResetGame);
            ClearNotificationsCommand = new RelayCommand(() => Notifications.Clear());
            StartStreamCommand = new AsyncRelayCommand<bool>((isCheck) => ToggleStream(isCheck));

            processProvider.DiceRecognitionProcess!.OnResultChanged += () => Update();
        }
        #endregion

        #region Methods

        private async Task ToggleStream(bool isCheck)
        {
            try
            {
                await Task.WhenAll(
                    isCheck ? StartCameraStream() : Task.CompletedTask,
                    !isCheck ? DisconnectCameraAsync() : Task.CompletedTask);
            }
            catch (Exception ex)
            {
                AddNotification($"Error in streaming: {ex.Message}", GameNotificationType.Error);
            }

        }

        private async Task StartCameraStream()
        {
            IsConnecting = true;
            await camera.ConnectAsync();
            frameReceivedHandler = (s, bitmap) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { ImageSource = bitmap; }));
            };

            camera.OnNewFrame += frameReceivedHandler;
            IsConnecting = false;
            AddNotification("Camera is Connected.", GameNotificationType.Information);
            AddNotification("The game of Yahtzee begins!", GameNotificationType.Success);
            AddNotification("Take 5 dice and roll", GameNotificationType.Information);
        }

        public async Task DisconnectCameraAsync()
        {
            await camera.DisconnectAsync();
            if (frameReceivedHandler != null)
            {
                camera.OnNewFrame -= frameReceivedHandler;
                frameReceivedHandler = null;
                AddNotification("Camera is Disconnect.", GameNotificationType.Information);
            }
        }

        private void ResetGame()
        {
            scoreManager.Enable(true);
            diceInGameCounter = 5;
            diceRollsCounter = 0;
            diceCounter = 0;

            diceRollsNumber = "0/3";
            diceInGameNumber = "5/5";

            Notifications.Clear();
            Dices = new ObservableCollection<DiceModel>(Enumerable.Range(0, 5).Select(_ => new DiceModel()));
            AddNotification("Reset has been successful", GameNotificationType.Success);
        }


        private void Roll()
        {
            scoreManager.Enable(true);
            diceInGameCounter = Dices.Count(x => !x.IsSelected);
            DiceInGameNumber = $"{diceInGameCounter} / 5";
            DiceRollsNumber = $"{++diceRollsCounter} / 3";

            scoreManager.UpdatePossibleScores(Dices.Select(dice => dice.Score).ToArray());
            CorrectCountDetected = false;
            AddNotification("Update successful", GameNotificationType.Success);
            AddNotification($"Use {diceInGameCounter} dice for the next roll", GameNotificationType.Information);
        }

        private void Update()
        {
            var res = processProvider.DiceRecognitionProcess?.Result;
            if (diceInGameCounter == res?.Length && res.All(s => s > 0))
            {
                CorrectCountDetected = true;
                foreach (var (dice, result) in Dices.Zip(res, (d, r) => (d, r)))
                {
                    if (!dice.IsSelected)
                    {
                        dice.AddResult(result);
                        if (dice.ChangeCounter >= 3)
                        {
                            dice.Score = result;
                        }
                    }
                }

                if (Dices.All(d => d.ChangeCounter >= 3))
                {
                    scoreManager.UpdatePossibleScores(Dices.Select(dice => dice.Score).ToArray());
                    AddNotification("Possible scores updated after stable results", GameNotificationType.Success);
                }
            }
            else
            {
                CorrectCountDetected = false;
            }
        }

        //private void Update()
        //{
        //    var res = processProvider.DiceRecognitionProcess?.Result;
        //    if (diceInGameCounter == res?.Length && res.All(s => s > 0))
        //    {
        //        CorrectCountDetected = true;
        //        foreach (var (dice, result) in Dices.Zip(res, (d, r) => (d, r)))
        //        {
        //            if (!dice.IsSelected)
        //            {
        //                dice.Score = result;
        //            }
        //        }
        //        scoreManager.UpdatePossibleScores(Dices.Select(dice => dice.Score).ToArray());
        //    }
        //    else
        //    {
        //        CorrectCountDetected = false;
        //    }
        //}

        private void AddNotification(string text, GameNotificationType type)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Notifications.Add(new NotificationModel(text, type));
            });
        }
        #endregion

        #region Events
        #endregion
    }
}
