using System.Collections.Generic;
using Envoice.Conditions;
using Envoice.MongoRepository;
using Envoice.MongoRepository.Impl;

namespace MovieCatalog.Web.DataAccess.Entities
{
    /// <summary>
    /// Models a single course.
    /// </summary>
    public class Movie : Entity
    {
        public Movie(string title)
        {
            Condition.Requires(title, "title").IsNotNullOrWhiteSpace();
            this.Title = title;
        }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
