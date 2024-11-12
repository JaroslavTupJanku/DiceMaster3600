using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core;
using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Model.Services;
using DiceMaster3600.Model.Validators;
using FluentValidation.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel.Control
{
    public class EntryFormViewModel : ReactiveViewModel
    {
        #region Fields
        private string? name;
        private string? surname;
        private string? email;
        private string? password;

        private readonly IDataAccessManager dataManager;
        private readonly EntryFormValidator validator = new();
        private ValidationResult validationResult;

        private FacultyType faculty = FacultyType.None;
        private UniversityType university = UniversityType.None;
        private Gender selectedGender = Gender.None;

        private bool isSavingInProgress = false;
        private DateTime dateOfBirth;
        #endregion

        #region Properties
        public string? Name
        {
            get => name;
            set
            {
                SetTrigger(ref name, value);
                Validate();
            }
        }

        public Gender SelectedGender
        {
            get => selectedGender;
            set
            {
                SetTrigger(ref selectedGender, value);
                Validate();
            }

        }

        public bool IsSavingInProgress
        {
            get => isSavingInProgress;
            set => SetProperty(ref isSavingInProgress, value);
        }

        public string? Surname
        {
            get => surname;
            set
            {
                SetTrigger(ref surname, value);
                Validate();
            }
        }

        public string? Email
        {
            get => email;
            set
            {
                SetTrigger(ref email, value);
                Validate();
            }
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                SetProperty(ref dateOfBirth, value);
                Validate();
            }
        }

        public UniversityType University
        {
            get => university;
            set
            {
                SetTrigger(ref university, value);
                Validate();
            }
        }

        public FacultyType Faculty
        {
            get => faculty;
            set
            {
                SetTrigger(ref faculty, value);
                Validate();
            }
        }

        public bool HasErrors => !validationResult.IsValid;
        public ICommand SaveCommand { get; }
        public ICommand GenderSelectCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructors
        public EntryFormViewModel(IDataAccessManager dataManager, IMessageService messageService) : base(messageService)
        {
            this.dataManager = dataManager;
            SubsribeNotification(NotificationContext.RegistrationFailure, m => Notify(m, MessageType.Failed));
            validationResult = validator.Validate(this);

            SaveCommand = new RelayCommand(async () => await ExecuteExampleCommand(), () => !HasErrors);
            dataManager.OnProcessingStatusChanged += (isInProgress) => IsSavingInProgress = isInProgress;

            CancelCommand = new RelayCommand(() => RequestClose?.Invoke());
            GenderSelectCommand = new RelayCommand<Gender>((g) => SelectedGender = g);
        }
        #endregion

        #region Methods
        private async Task ExecuteExampleCommand()
        {
            try
            {
                var user = new UserDTO(Name!, Surname!, Email!, SelectedGender);
                await dataManager.RegisterUserAsync(user, University, Faculty);

                Notify(NotificationContext.RegistrationSuccess, "Registration was successful!");
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                Notify(NotificationContext.RegistrationFailure, $"Nastala chyba: {ex.Message}");
                AppLogger.Error(ex.Message);
            }
        }

        private void Validate()
        {
            validationResult = validator.Validate(this);
            OnPropertyChanged(nameof(HasErrors));
        }

        public string GetError(string propertyName)
        {
            var error = validationResult.Errors.FirstOrDefault(e => e.PropertyName == propertyName);
            return error != null ? error.ErrorMessage : string.Empty;
        }

        public override void Dispose() => UnsubsribeNotification(NotificationContext.RegistrationFailure);
        

        public override void RefreshCommand()
        {
            if (SaveCommand is RelayCommand command)
            {
                command.NotifyCanExecuteChanged();
            }
        }
        #endregion

        #region Events
        public event Action? RequestClose;
        #endregion
    }
}
