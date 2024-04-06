using CommunityToolkit.Mvvm.Input;
using DiceMaster3600.Core.DTOs;
using DiceMaster3600.Core.Enum;
using DiceMaster3600.Core.InterFaces;
using System;
using System.Windows.Input;

namespace DiceMaster3600.ViewModel
{
    public class EntryFormViewModel : BaseViewModel
    {
        #region Fields
        private string? name;
        private string? surname;

        private DateTime dateOfBirth;
        private string? email;

        private UniversityType university = UniversityType.None;
        private FacultyType faculty = FacultyType.None;

        private Gender selectedGender = Gender.None;
        private readonly IDataAccessManager dataManager;
        #endregion

        #region Properties
        public string? Name
        {
            get => name;
            set => SetAndNotify(ref name, value);
        }

        public Gender SelectedGender
        {
            get => selectedGender;
            set => SetAndNotify(ref selectedGender, value);
        }

        public string? Surname
        {
            get => surname;
            set => SetAndNotify(ref surname, value);
        }

        public string? Email
        {
            get => email;
            set => SetAndNotify(ref email, value);
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set => SetProperty(ref dateOfBirth, value);
        }

        public UniversityType University
        {
            get => university;
            set => SetAndNotify(ref university, value);
        }

        public FacultyType Faculty
        {
            get => faculty;
            set => SetAndNotify(ref faculty, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand GenderSelectCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Constructors
        public EntryFormViewModel(IDataAccessManager dataManager)
        {
            this.dataManager = dataManager;

            SaveCommand = new RelayCommand(
                () => dataManager.AddUser(CreateUserDTO(), University, Faculty), 
                () => CanSaveExecute());

            CancelCommand = new RelayCommand(() => RequestClose?.Invoke());
            GenderSelectCommand = new RelayCommand<Gender>((g) => SelectedGender = g);
        }
        #endregion

        #region Methods
        private bool CanSaveExecute()
        {
            return !string.IsNullOrWhiteSpace(Name);
                //&& !string.IsNullOrWhiteSpace(Surname)
                //&& !string.IsNullOrWhiteSpace(Email)
                //&& Email.Contains('@')
                //&& SelectedGender != Gender.None
                //&& University != UniversityType.None
                //&& Faculty != FacultyType.None 
                //&& (DateTime.Now.Year - DateOfBirth.Year) >= 16;
        }

        private UserDTO CreateUserDTO()
        {
            return new UserDTO
            {
                Name = Name!,
                EmailAddress = Email!,
                Surname = Surname!,
                NumberOfPoints = 0
            };
        }

        public override void NotifyCommandCanExecuteChanged()
        {
            (SaveCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }
        #endregion

        #region Events
        public event Action? RequestClose;
        #endregion
    }
}
