using DiceMaster3600.Core.InterFaces;
using DiceMaster3600.Data.Adapter;

namespace DiceMaster3600.Model
{
    public class DataAccessManager : IDataAccessManager
    {
        #region Fields
        private readonly RepositoryAdapter repositoryAdapter;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public DataAccessManager(RepositoryAdapter repositoryAdapter) //AppSettings settings
        {
            this.repositoryAdapter = repositoryAdapter;
        }
        #endregion

        #region Methods
        #endregion

        #region Events
        #endregion

    }
}
