﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CodeCamp.Core.Extensions;
using CodeCamp.Core.Messaging.Messages;
using CodeCamp.Core.ViewModels.Annotations;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace CodeCamp.Core.ViewModels
{
    public static class PresentationBundleFlagKeys
    {
        public const string ClearStack = "__CLEAR_STACK__";
    }

    public abstract class ViewModelBase : MvxViewModel
    {
        protected ViewModelBase(IMvxMessenger messenger)
        {
            Messenger = messenger;

            IsLoading = !this.HasAttribute<DoesNotRequireLoadingAttribute>();
        }

        public IMvxMessenger Messenger { get; private set; }
        public event EventHandler LoadingComplete;

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            private set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
        }

        protected void FinishedLoading(bool successful)
        {
            IsLoading = false;

            if (LoadingComplete != null)
                LoadingComplete.Invoke(this, EventArgs.Empty);
        }

        protected async Task<bool> SafeOperation(Task operation, Expression<Func<bool>> operationFlag = null)
        {
            Action<bool> setOperationFlag = isInProgress =>
            {
                if (operationFlag == null) return;

                var memberSelectorExpression = operationFlag.Body as MemberExpression;
                if (memberSelectorExpression == null) return;

                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property == null) return;

                property.SetValue(this, isInProgress, null);
            };

            try
            {
                setOperationFlag(true);

                await operation;

                return true;
            }
            catch (Exception ex)
            {
                ReportError("Something went wrong :(");
                Console.WriteLine( ex);
                return false;
            }
            finally
            {
                setOperationFlag(false);
            }
        }

        protected void ReportError(string message)
        {
            Messenger.Publish(new ErrorMessage(this, message));
        }
    }
}