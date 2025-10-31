using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKSamachar.DAL.Enitity
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column(Order = 0)]
        public Guid Id { get; set; }
        [Column(Order = 97)]
        public bool IsDeleted { get; set; } = false;
        [Column(Order = 98)]
        public bool IsActive { get; set; } = true;
        [Column(Order = 99)]
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        [Column(Order = 100)]
        public DateTime? ModifiedOn { get; set; } = DateTime.UtcNow;
    }
}
