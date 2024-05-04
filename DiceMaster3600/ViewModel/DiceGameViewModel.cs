using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Devices.RealSenseCamera;
using DiceMaster3600.Model;
using DiceMaster3600.Model.FrameProcesses;
using DiceMaster3600.Model.Services;
using DiceMaster3600.Model.Yahtzee;
using Intel.RealSense;
using System;
using System.Collections.Generic;
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
        private readonly IYahtzeeScoreManager scoreManager;
        private readonly IRealSenseCamera camera;
        private readonly IProcessProvider processProvider;
        private readonly IFrameProcces process;

        private BitmapSource imageSource;
        private int diceInGameCounter;
        private int diceRollsCounter;
        private bool isCameraConnected;
        private bool isConnecting;

        private EventHandler<FrameSet>? frameReceivedHandler;
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

        public bool IsCameraConnected
        {
            get => isCameraConnected;
            set => SetProperty(ref isCameraConnected, value);
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
        public DiceGameViewModel(IYahtzeeScoreManager scoreManager, IRealSenseCamera camera, IProcessProvider processProvider)
        {
            ResetGame();

            this.camera = camera;
            this.processProvider = processProvider;
            this.scoreManager = scoreManager;

            ResetCommand = new RelayCommand(ResetGame);
            RollCommand = new RelayCommand<ScoreTypes>((type) => Roll(type));
            ClearNotificationsCommand = new RelayCommand(() => Notifications.Clear());
            StartStreamCommand = new AsyncRelayCommand<bool>((isCheck) => ToggleStream(isCheck));

            //processProvider.SimulationDiceRecognitionProcess!.OnResultChanged += () => Update();
        }

        #endregion

        #region Methods

        private async Task ToggleStream(bool isCheck)
        {
            try
            {
                IsCameraConnected = isCheck;
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
            frameReceivedHandler = (s, frameSet) =>
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        ImageSource = BitMapConverter.ConvertToBitMatSource(frameSet.ColorFrame);
                    }
                    finally
                    {
                        frameSet.Dispose();
                    }
                }));
            };

            camera.OnNewFrame += frameReceivedHandler;
            IsConnecting = false;
            AddNotification("Camera is Connected.", GameNotificationType.Information);
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
            diceInGameCounter = 5;
            diceRollsCounter = 0;

            diceRollsNumber = "0/3";
            diceInGameNumber = "5/5";

            Notifications.Clear();
            Dices = new ObservableCollection<DiceModel>(Enumerable.Repeat(new DiceModel(), 5));
            AddNotification("Reset has been successful", GameNotificationType.Success);
        }


        private void Roll(ScoreTypes type)
        {
            scoreManager.AssignScoreToCategory(type!);
            AddNotification($"Use {diceInGameCounter} dice for the next roll", GameNotificationType.Information);
        }

        private void Update()
        {
            if (process is IResultFrameProcess<List<CameraDiceResult>> resultProcessor)
            {
                var results = resultProcessor.Result!.ToArray();
                diceInGameCounter = 0;

                foreach (var (dice, result) in Dices.Zip(results, (d, r) => (d, r)))
                {
                    if (!dice.IsSelected)
                    {
                        dice.Score = result.DotCount;
                        diceInGameCounter++;
                    }
                }

                DiceInGameNumber = $"{diceInGameCounter} / 5";
                DiceRollsNumber = $"{++diceRollsCounter} / 3";
                scoreManager.UpdatePossibleScores(Dices.Select(dice => dice.Score).ToArray());
                AddNotification("Update successful", GameNotificationType.Success);
            }
        }

        private void AddNotification(string text, GameNotificationType type) => Notifications.Add(new NotificationModel(text, type));
        #endregion

        #region Events
        #endregion
    }
}
