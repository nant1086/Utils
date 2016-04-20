using System;
using System.ServiceModel;

namespace Project
{
    public class WcfClient<T> : ClientBase<T> where T : class
    {
        private bool _disposed = false;
        public WcfClient()
            : base(typeof(T).FullName)
        {
        }
        public WcfClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }
        public T ChannelService
        {
            get { return base.Channel; }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (this.State == CommunicationState.Faulted)
                    {
                        base.Abort();
                    }
                    else
                    {
                        try
                        {
                            base.Close();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            base.Abort();
                        }
                    }
                    _disposed = true;
                }
            }
        }
    }

    
}