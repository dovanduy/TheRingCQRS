﻿namespace TheRing.CQRS.RavenDb.Eventing
{
    #region using

    using Raven.Client;
    using Raven.Client.Indexes;

    using TheRing.CQRS.Eventing;
    using TheRing.RavenDb;

    #endregion

    public class CqrsDocumentStoreFactory : ICqrsDocumentStoreFactory
    {
        #region Fields

        private readonly IDocumentStoreFactory documentStoreFactory;

        #endregion

        #region Constructors and Destructors

        public CqrsDocumentStoreFactory(
            IAddDocumenStoreFromParameters storeAdder,
            IDocumentStoreFactory documentStoreFactory,
            ICqrsDocumentStoreFactoryInitializer initializer = null)
        {
            this.documentStoreFactory = documentStoreFactory;
            storeAdder.AddStore(
                new DocumentStoreParameters
                {
                    DatabaseName = "EventStore",
                    FindTypeTagName = type => typeof(AbstractEvent).Name,
                    TransformTypeTagNameToDocumentKeyPrefix = s => null
                });

            IndexCreation.CreateIndexes(typeof(AbstractEvent_EventSourcedIdAndVersion).Assembly, this.EventStore);

            storeAdder.AddStore(new DocumentStoreParameters
                {
                    DatabaseName = "ReadModel",
                    FindIdentityPropertyNameFromEntityName = n => n + "Id",
                });

            storeAdder.AddStore(new DocumentStoreParameters
                {
                    DatabaseName = "SagaStore",
                    FindIdentityPropertyNameFromEntityName = n => "CorrelationId"
                });

            if (initializer == null)
            {
                return;
            }

            initializer.SetDocumentStoreFactory(this);
            initializer.Initialize();
        }

        #endregion

        #region Public Properties

        public IDocumentStore EventStore
        {
            get
            {
                return this.documentStoreFactory.GetStore("EventStore");
            }
        }

        public IDocumentStore ReadModel
        {
            get
            {
                return this.documentStoreFactory.GetStore("ReadModel");
            }
        }

        public IDocumentStore SagaStore
        {
            get
            {
                return this.documentStoreFactory.GetStore("SagaStore");
            }
        }

        #endregion
    }
}