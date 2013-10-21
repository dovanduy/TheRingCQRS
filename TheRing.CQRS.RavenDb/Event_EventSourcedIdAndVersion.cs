﻿namespace TheRing.CQRS.RavenDb
{
    #region using

    using System.Linq;

    using Raven.Client.Indexes;

    using TheRing.CQRS.Eventing;

    #endregion

    public class Event_EventSourcedIdAndVersion : AbstractIndexCreationTask<Event>
    {
        #region Constructors and Destructors

        public Event_EventSourcedIdAndVersion()
        {
            this.Map = events => from e in events
                orderby e.EventSourcedVersion
                select new { e.EventSourcedId, e.EventSourcedVersion };
        }

        #endregion
    }
}