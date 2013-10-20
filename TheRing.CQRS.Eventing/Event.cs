namespace TheRing.CQRS.Eventing
{
    #region using

    using System;

    #endregion

    public abstract class Event
    {
        #region Public Properties

        public Guid CorrelationId { get; set; }

        public Guid EventSourcedId { get; set; }

        public int EventSourcedVersion { get; set; }

        public virtual int EventVersion
        {
            get { return 0; }
        }

        public DateTime TimeStamp { get; set; }

        public virtual bool Volatile
        {
            get { return false; }
        }

        #endregion
    }
}