using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync;
using Amazon.CognitoSync.SyncManager;
using System.Collections.Generic;
using System.ComponentModel;
using Travlendar.Core.AppCore.Model;
using Travlendar.Framework.ViewModels;

namespace Travlendar.Core.AppCore.ViewModels
{
    /// <summary>
    /// CognitoSyncViewModel singleton class for handling and mapping the AWS Cognito service.
    /// </summary>
    sealed public class CognitoSyncViewModel : AViewModel<CognitoSyncModel>
    {
        private static CognitoSyncViewModel _instance = new CognitoSyncViewModel ();
        public override event PropertyChangedEventHandler PropertyChanged;

        CognitoAWSCredentials credentials;
        CognitoSyncManager syncManager;
        Dictionary<string, Dataset> datasets;

        private CognitoSyncViewModel ()
        {
            credentials = new CognitoAWSCredentials (Constants.AWS_IDENTITY_POOL_ID, RegionEndpoint.EUWest1);
            syncManager = new CognitoSyncManager (credentials, new AmazonCognitoSyncConfig { RegionEndpoint = RegionEndpoint.EUWest1 });
            datasets = new Dictionary<string, Dataset> ();
        }

        static internal CognitoSyncViewModel GetInstance ()
        {
            return _instance;
        }

        internal void AWSLogin (string providerName, string accessToken)
        {
            credentials.AddLogin (providerName, accessToken);
        }

        /// <summary>
        /// Open or Create an AWS Cognito Dataset
        /// </summary>
        public void CreateDataset (string name)
        {
            if ( name == null )
                throw new System.Exception ("Do not create empty named datasets");

            Dataset dataset;
            dataset = syncManager.OpenOrCreateDataset (name);
            dataset.SynchronizeAsync ();

            if ( !datasets.ContainsKey (name) )
                datasets.Add (name, dataset);
        }

        /// <summary>
        /// Writes a specific Dataset and synchronizes it.
        /// </summary>
        public void WriteDataset (string dataset, string key, string value)
        {
            Dataset local;
            if ( !datasets.TryGetValue (dataset, out local) )
                throw new System.Exception (string.Format ("Dataset: {0} not found.", dataset));

            if ( !string.IsNullOrEmpty (key) && !string.IsNullOrEmpty (value) )
            {
                local.Put (key, value);
                local.SynchronizeAsync ();
            }
        }

        /// <summary>
        /// Reads a specific Dataset.
        /// </summary>
        public string ReadDataset (string dataset, string key)
        {
            Dataset local;

            if ( !datasets.TryGetValue (dataset, out local) )
                throw new System.Exception (string.Format ("Dataset: {0} not found.", dataset));

            if ( string.IsNullOrEmpty (key) )
            {
                return string.Empty;
            }

            return local.Get (key);
        }

        /// <summary>
        /// Read the whole Dataset in the format (Key,Value)
        /// </summary>
        public IDictionary<string, string> ReadWholeDataset (string dataset)
        {
            Dataset local;
            if ( !datasets.TryGetValue (dataset, out local) )
                throw new System.Exception (string.Format ("Dataset: {0} not found.", dataset));

            return local.ActiveRecords;
        }

        /// <summary>
        /// Delete an element from the Dataset given the Key and synchronizes it.
        /// </summary>
        public void RemoveFromDataset (string dataset, string key)
        {
            Dataset local;

            if ( !datasets.TryGetValue (dataset, out local) )
                throw new System.Exception (string.Format ("Dataset: {0} not found.", dataset));

            if ( !string.IsNullOrEmpty (key) )
            {
                local.Remove (key);
                local.SynchronizeOnConnectivity ();
            }
        }

        /// <summary>
        /// Delete an entire Dataset and synchronized it.
        /// </summary>
        public void RemoveDataset (string dataset)
        {
            Dataset local;

            if ( !datasets.TryGetValue (dataset, out local) )
                throw new System.Exception (string.Format ("Dataset: {0} not found.", dataset));

            local.Delete ();
            local.SynchronizeOnConnectivity ();

            datasets.Remove (dataset);
        }

        public override string ToString ()
        {
            return base.ToString ();
        }
    }
}
