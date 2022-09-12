using System.Linq.Expressions;

namespace RadioArchive.Maui.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        private readonly object _globalLock = new();

        /// <summary>
        /// Title of this List
        /// </summary>
        [ObservableProperty]
        private string _displayTitle;

        /// <summary>
        /// Inidcates if view model is awating a call 
        /// </summary>
        [ObservableProperty]
        private bool _isBusy;

        #region CommandHelpers

        /// <summary>
        /// Runs the command if updating flag is not set.
        /// if the flag is true meaning the function is running then the action not run
        /// if the flag is false indicating no running function the action is run
        /// once the action finished if it was run then the flag is rest to false
        /// </summary>
        /// <param name="updatingFlag"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // Lock to ensure single access to check
            lock (_globalLock)
            {
                // Check if flag property value is true meaning the function is already running
                if (updatingFlag.GetPropertyValue())
                    return;

                // Set the flag to true to indicate we're running 
                updatingFlag.SetPropertyValue(true);
            }

            try
            {
                //runs the passed action
                await action();
            }
            finally
            {
                //sets the property value back to false now that is now finished
                updatingFlag.SetPropertyValue(false);
            }
        }

        /// <summary>
        /// Runs the command if updating flag is not set.
        /// if the flag is true meaning the function is running then the action not run
        /// if the flag is false indicating no running function the action is run
        /// once the action finished if it was run then the flag is rest to false
        /// </summary>
        /// <param name="updatingFlag"></param>
        /// <param name="action"></param>
        /// <typeparam name="T">The type of action return</typeparam>
        /// <returns></returns>
        protected async Task<T> RunCommand<T>(Expression<Func<bool>> updatingFlag, Func<Task<T>> action, T defualtValue = default(T))
        {
            // Lock to ensure single access to check
            lock (_globalLock)
            {
                // Check if flag property value is true meaning the function is already running
                if (updatingFlag.GetPropertyValue())
                    return defualtValue;

                // Set the flag to true to indicate we're running 
                updatingFlag.SetPropertyValue(true);
            }

            try
            {
                //runs the passed action
                return await action();
            }
            finally
            {
                //sets the property value back to false now that is now finished
                updatingFlag.SetPropertyValue(false);
            }
        }

        #endregion
    }
}
