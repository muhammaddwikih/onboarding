using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace onboarding.dal.Model
{
    public class MovieModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Genre { get; set; }

        public int ImdbId { get; set; }

        public Guid? NationalId { get; set; }

        [ForeignKey("NationalId")]
        public National Nation { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public MovieModel()
        {
            CreatedDate = DateTime.UtcNow;
        }
    }
}
