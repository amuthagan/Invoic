using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.Model
{
    public class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        public string ApplicationTitle = "SmartPD";
        //public string Message { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            ValidateProperty(propertyName);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangingEventHandler PropertyChange;
        protected void OnPropertyChanged(string propertyName)
        {
            ValidateProperty(propertyName);
            if (PropertyChange != null)
            {
                PropertyChange(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        ErrorsContainer<string> _errorsContainer;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        public IEnumerable GetErrors(string propertyName)
        {
            return ErrorsContainer.GetErrors(propertyName);
        }

        public void SetErrors<TProperty>(Expression<Func<TProperty>> propertyExpression, IEnumerable<string> errors)
        {
            ErrorsContainer.SetErrors(propertyExpression, errors);
        }

        public bool HasErrors
        {
            get { return ErrorsContainer.HasErrors; }
        }

        public Dictionary<string, List<string>> GetAllErrors()
        {
            return ErrorsContainer.GetAllErrors();
        }


        protected ErrorsContainer<string> ErrorsContainer
        {
            get
            {
                if (_errorsContainer == null)
                {
                    _errorsContainer =
                        new ErrorsContainer<string>(pn => this.RaiseErrorsChanged(pn));
                }

                return _errorsContainer;
            }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged(this, e);
        }


        public bool ValidateProperty(string propertyName)
        {
            bool isValid = false;
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            try
            {
                var propertyInfo = this.GetType().GetRuntimeProperty(propertyName);
                if (propertyInfo == null)
                {
                    throw new ArgumentException("Invalid property name", propertyName);
                }

                var propertyErrors = new List<string>();
                isValid = TryValidateProperty(propertyInfo, propertyErrors);
                ErrorsContainer.SetErrors(propertyInfo.Name, propertyErrors);
            }
            catch (Exception ex)
            {
                PropertyInfo propertyInfo = (from row in this.GetType().GetRuntimeProperties()
                                             where row.Name == propertyName
                                             select row).SingleOrDefault();
                if (propertyInfo == null)
                {

                    throw new ArgumentException("Invalid property name", propertyName);
                }
            }
            return isValid;
        }
        public string ValidateProperties(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            var propertyInfo = this.GetType().GetRuntimeProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            var propertyErrors = new List<string>();
            bool isValid = TryValidateProperty(propertyInfo, propertyErrors);
            ErrorsContainer.SetErrors(propertyInfo.Name, propertyErrors);
            var strings = propertyErrors.ToArray();
            return string.Join(Environment.NewLine, strings);
        }
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                var propertyInfo = this.GetType().GetRuntimeProperty(propertyName);
                PropertyInfos.Add(propertyInfo);
                return string.Empty;
            }
        }

        List<PropertyInfo> _propertyinfos;
        public List<PropertyInfo> PropertyInfos
        {
            get
            {
                if (_propertyinfos == null)
                {
                    _propertyinfos = new List<PropertyInfo>();
                }

                return _propertyinfos;
            }
        }
        public bool ValidateProperties()
        {
            var propertiesWithChangedErrors = new List<string>();

            // Get all the properties decorated with the ValidationAttribute attribute.
            if (PropertyInfos != null)
            {
                foreach (PropertyInfo propertyInfo in PropertyInfos)
                {
                    var propertyErrors = new List<string>();
                    TryValidateProperty(propertyInfo, propertyErrors);

                    // If the errors have changed, save the property name to notify the update at the end of this method.
                    ErrorsContainer.SetErrors(propertyInfo.Name, propertyErrors);
                }
            }

            var propertiesToValidate = this.GetType()
                .GetRuntimeProperties()
                .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (PropertyInfo propertyInfo in propertiesToValidate)
            {
                var propertyErrors = new List<string>();
                TryValidateProperty(propertyInfo, propertyErrors);

                // If the errors have changed, save the property name to notify the update at the end of this method.
                ErrorsContainer.SetErrors(propertyInfo.Name, propertyErrors);
            }

            return ErrorsContainer.HasErrors;
        }

        private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue(this);

            // Validate the property
            bool isValid = Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
            }

            return isValid;
        }

        public bool InitializeMandatoryFields(object paramType)
        {
            bool bReturnValue = false;
            try
            {
                foreach (PropertyInfo propertyInfo in paramType.GetType().GetProperties())
                {
                    var hasValidationAttribute = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), false).Length > 0;

                    if (propertyInfo != null && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.CanWrite && hasValidationAttribute)
                    {
                        bool isNullable = propertyInfo.PropertyType.IsGenericType &&
                            propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                        switch (propertyInfo.PropertyType.Name)
                        {
                            case "String":
                                propertyInfo.SetValue(paramType, isNullable ? null : string.Empty, null);
                                break;
                            case "Nullable`1":
                                propertyInfo.SetValue(paramType, isNullable ? null : DBNull.Value, null);
                                break;
                        }
                    }
                }
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bReturnValue;
        }

    }
}
