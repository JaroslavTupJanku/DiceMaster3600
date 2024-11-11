using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core;
using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class EntryFormViewModel : ReactiveViewModel
    {
        #region Fields
        private string? name;
        private string? surname;
        private string? email;
        private string? password;

        private FacultyType faculty = FacultyType.None;
        private UniversityType university = UniversityType.None;
        private Gender selectedGender = Gender.None;
        private bool isSavingInProgress = false;
        private DateTime dateOfBirth;
        private readonly IDataAccessManager dataManager;
        #endregion

        #region Properties
        public string? Name
        {
            get => name;
            set => SetTrigger(ref name, value);
        }

        public Gender SelectedGender
        {
            get => selectedGender;
            set => SetTrigger(ref selectedGender, value);
        }

        public bool IsSavingInProgress
        {
            get => isSavingInProgress;
            set => SetProperty(ref isSavingInProgress, value);
        }

        public string? Surname
        {
            get => surname;
            set => SetTrigger(ref surname, value);
        }

        public string? Email
        {
            get => email;
            set => SetTrigger(ref email, value);
        }

    public string? Password
    {
        get => password;
        set => SetTrigger(ref password, value);
    }

    public DateTime DateOfBirth
    {
        get => dateOfBirth;
        set => SetProperty(ref dateOfBirth, value);
    }


        public UniversityType University
        {
            get => university;
            set => SetTrigger(ref university, value);
        }

        public FacultyType Faculty
        {
            get => faculty;
            set => SetTrigger(ref faculty, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand GenderSelectCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructors
        public EntryFormViewModel(IDataAccessManager dataManager, IMessageService messageService) : base(messageService)
        {
            this.dataManager = dataManager;
            SubsribeNotification(NotificationContext.RegistrationFailure, m => Notify(m, MessageType.Failed));

            SaveCommand = new RelayCommand(async
                () => await ExecuteExampleCommand(),
                () => CanSaveExecute());

            dataManager.OnProcessingStatusChanged += (isInProgress) 
                => IsSavingInProgress = isInProgress;

            CancelCommand = new RelayCommand(() => RequestClose?.Invoke());
            GenderSelectCommand = new RelayCommand<Gender>((g) => SelectedGender = g);
        }
        #endregion

        #region Methods
        private bool CanSaveExecute()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Surname)
                && !string.IsNullOrWhiteSpace(Email)
                && Email.Contains('@')
                && !string.IsNullOrWhiteSpace(Password)
                && University != UniversityType.None
                && Faculty != FacultyType.None
                && (DateTime.Now.Year - DateOfBirth.Year) >= 15;
        }

        private async Task ExecuteExampleCommand()
        {
            try
            {
                var user = new UserDTO(Name!, Surname!, Email!, SelectedGender);
                await dataManager.RegisterUserAsync(user, Password, University, Faculty);

                Notify(NotificationContext.RegistrationSuccess, "Registration was byla succesfull!");
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                Notify(NotificationContext.RegistrationFailure, $"Nastala chyba: {ex.Message}");
                AppLogger.Error(ex.Message);
            }
        }

        public override void Dispose()
            => UnsubsribeNotification(NotificationContext.RegistrationFailure);      

        public override void RefreshCommand()
        {
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
        #endregion

        #region Events
        public event Action? RequestClose;
        #endregion
    }
}
